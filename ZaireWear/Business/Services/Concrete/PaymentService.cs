using Business.Services.Abstract;
using Core.Constants.Enums;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using Business.Utilities.Stripe;
using Microsoft.Extensions.Logging;

namespace Business.Services.Concrete;

public class PaymentService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;
    private readonly UserManager<User> _userManager;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IOptions<StripeSettings> stripeSettings,
        UserManager<User> userManager,
        IActionContextAccessor actionContextAccessor,
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IOrderProductRepository orderProductRepository,
        IUrlHelperFactory urlHelperFactory,
        IProductRepository productRepository,
        IBasketRepository basketRepository,
        ILogger<PaymentService> logger)
    {
        _stripeSettings = stripeSettings.Value;
        _userManager = userManager;
        _actionContextAccessor = actionContextAccessor;
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
        _urlHelperFactory = urlHelperFactory;
        _productRepository = productRepository;
        _basketRepository = basketRepository;
        _logger = logger;
    }

    public async Task<(int statusCode, string? description, string? url)> PayAsync()
    {
        try
        {
            var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);
            if (user == null)
            {
                _logger.LogWarning("Payment attempt by unauthorized user");
                return (401, "User not authorized", null);
            }

            var basket = await _basketRepository.GetBasketByUserId(user.Id);
            if (basket == null || !basket.BasketProducts.Any())
            {
                _logger.LogWarning("Empty basket for user {UserId}", user.Id);
                return (400, "Basket is empty", null);
            }

            foreach (var basketProduct in basket.BasketProducts)
            {
                var product = await _productRepository.GetByIdAsync(basketProduct.ProductId);
                if (product == null || product.StockCount < basketProduct.Count)
                {
                    _logger.LogWarning("Insufficient stock for product {ProductId}", basketProduct.ProductId);
                    return (400, $"Not enough stock for product {product?.Title}", null);
                }
            }

            var order = new Order
            {
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                PaymentToken = Guid.NewGuid()
            };

            await _orderRepository.CreateAsync(order);

            var items = new List<SessionLineItemOptions>();
            foreach (var basketProduct in basket.BasketProducts)
            {
                var orderProduct = new OrderProduct
                {
                    Order = order,
                    Price = basketProduct.Product.Price,
                    Count = basketProduct.Count,
                    ProductId = basketProduct.ProductId,
                };
                await _orderProductRepository.CreateAsync(orderProduct);

                items.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = basketProduct.Product.Price * 100,
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = basketProduct.Product.Title
                        }
                    },
                    Quantity = basketProduct.Count
                });
            }

            await _unitOfWork.CommitAsync();

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            var successUrl = urlHelper.Action(
                "Success",
                "Payment",
                new { token = order.PaymentToken },
                "https"
            );

            var cancelUrl = urlHelper.Action(
                "Cancel",
                "Payment",
                new { token = order.PaymentToken },
                "https"
            );

            _logger.LogInformation("Creating Stripe session with URLs: Success={SuccessUrl}, Cancel={CancelUrl}",
                successUrl, cancelUrl);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = items,
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            _logger.LogInformation("Stripe session created: {SessionId}", session.Id);

            return (200, null, session.Url);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe error: {Message}", ex.Message);
            return (500, "Payment processing error", null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical payment error: {Message}", ex.Message);
            return (500, "Internal server error", null);
        }
    }

    public async Task<bool> PaySuccess(Guid token)
    {
        try
        {
            var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);
            if (user == null) return false;

            var order = await _orderRepository.GetOrderWithOrderProductsAsync(token, user.Id);
            if (order == null) return false;

            foreach (var orderProduct in order.OrderProducts)
            {
                var product = await _productRepository.GetByIdAsync(orderProduct.ProductId);
                if (product == null || product.StockCount < orderProduct.Count)
                {
                    order.Status = OrderStatus.Failed;
                    _orderRepository.Update(order);
                    await _unitOfWork.CommitAsync();
                    return false;
                }

                product.StockCount -= orderProduct.Count;
                _productRepository.Update(product);
            }

            order.Status = OrderStatus.Success;
            _orderRepository.Update(order);

            var basket = await _basketRepository.GetBasketByUserId(user.Id);
            if (basket != null)
            {
                _basketRepository.Delete(basket);
            }

            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing successful payment");
            return false;
        }
    }

    public async Task<bool> PayCancel(Guid token)
    {
        try
        {
            var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);
            if (user == null) return false;

            var order = await _orderRepository.GetOrderWithOrderProductsAsync(token, user.Id);
            if (order == null) return false;

            order.Status = OrderStatus.Failed;
            _orderRepository.Update(order);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing canceled payment");
            return false;
        }
    }
}