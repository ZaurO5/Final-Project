using Business.Features.Auth.Commands.AuthConfirm;
using Business.Features.Auth.Commands.AuthForget;
using Business.Features.Auth.Commands.AuthLogin;
using Business.Features.Auth.Commands.AuthRegister;
using Business.Features.Auth.Commands.AuthReset;
using Business.Features.Auth.Dtos;
using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Documentation
        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="request"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("register")]
        public async Task<Response> RegisterAsync(AuthRegisterCommand request)
            => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// User login
        /// </summary>
        /// <param name="request"></param>
        [ProducesResponseType(typeof(Response<ResponseTokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("login")]
        public async Task<Response<ResponseTokenDto>> LoginAsync(AuthLoginCommand request)
            => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Confirm user's email
        /// </summary>
        /// <param name="request"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("confirm-email")]
        public async Task<Response> ConfirmEmail(AuthConfirmEmailCommand request)
            => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Send a password reset link
        /// </summary>
        /// <param name="request"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("forget-password")]
        public async Task<Response> ForgetPassword(AuthForgetPasswordCommand request)
            => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Reset user's password
        /// </summary>
        /// <param name="request"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("reset-password")]
        public async Task<Response> ResetPassword(AuthResetPasswordCommand request)
            => await _mediator.Send(request);
    }

}
