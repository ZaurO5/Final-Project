using Business.Services.Abstract;
using Core.Constants.Enums;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Utilities.Stripe;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;

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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;

    public PaymentService(
        IOptions<StripeSettings> stripeSettings,
        UserManager<User> userManager,
        IActionContextAccessor actionContextAccessor,
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IOrderProductRepository orderProductRepository,
        IUrlHelperFactory urlHelperFactory,
        IHttpContextAccessor httpContextAccessor,
        IProductRepository productRepository,
        IBasketRepository basketRepository)
    {
        _stripeSettings = stripeSettings.Value;
        _userManager = userManager;
        _actionContextAccessor = actionContextAccessor;
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
        _urlHelperFactory = urlHelperFactory;
        _httpContextAccessor = httpContextAccessor;
        _productRepository = productRepository;
        _basketRepository = basketRepository;
    }

    public async Task<(int statusCode, string? description, string? id)> PayAsync()
    {
        var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);
        if (user == null) return (404, "User not found or not authorized", null);

        var basket = await _basketRepository.GetBasketByUserId(user.Id);
        if (basket == null || !basket.BasketProducts.Any()) return (400, "Basket is empty", null);

        // Проверка наличия товаров на складе перед оплатой
        foreach (var basketProduct in basket.BasketProducts)
        {
            var product = await _productRepository.GetByIdAsync(basketProduct.ProductId);
            if (product == null || product.StockCount < basketProduct.Count)
            {
                return (400, $"Not enough stock for product {product?.Title}", null);
            }
        }

        var order = new Order
        {
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.Now,
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

        var actionContext = _actionContextAccessor.ActionContext;
        var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

        var successUrl = urlHelper.Action("Success", "Payment", new { token = order.PaymentToken }, "https");
        var cancelUrl = urlHelper.Action("Cancel", "Payment", new { token = order.PaymentToken }, "https");

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "payment",
            LineItems = items,
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
        };

        try
        {
            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return (200, null, session.Id);
        }
        catch (StripeException e)
        {
            return (400, e.Message, null);
        }
    }

    public async Task<bool> PaySuccess(Guid token)
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

    public async Task<bool> PayCancel(Guid token)
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
}