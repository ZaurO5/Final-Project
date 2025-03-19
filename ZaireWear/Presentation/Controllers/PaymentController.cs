using Business.Utilities.Stripe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Pay()
        {
            try
            {
                var result = await _paymentService.PayAsync();

                _logger.LogInformation("Payment result: {StatusCode}", result.statusCode);

                return result.statusCode switch
                {
                    200 => Json(new { url = result.url }),
                    400 => BadRequest(result.description),
                    401 => Unauthorized(result.description),
                    _ => StatusCode(result.statusCode, result.description)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment controller error");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Success(Guid token)
        {
            try
            {
                var isSucceeded = await _paymentService.PaySuccess(token);
                return isSucceeded ? View() : RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Success action");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Cancel(Guid token)
        {
            try
            {
                var isCancelled = await _paymentService.PayCancel(token);
                return isCancelled ? View() : RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Cancel action");
                return RedirectToAction("Error", "Home");
            }
        }
    }

}

