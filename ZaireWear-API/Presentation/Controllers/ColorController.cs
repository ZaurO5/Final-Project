using Business.Features.Color.Commands.CreateColor;
using Business.Features.Color.Commands.DeleteColor;
using Business.Features.Color.Commands.UpdateColor;
using Business.Features.Color.Dtos;
using Business.Features.Color.Queries.GetAllColors;
using Business.Features.Color.Queries.GetColor;
using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ColorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Documentation
        /// <summary>
        /// Colors List
        /// </summary>
        [ProducesResponseType(typeof(Response<List<ColorDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet]
        public async Task<Response<List<ColorDto>>> GetAllAsync()
            => await _mediator.Send(new GetAllColorsQuery());

        #region Documentation
        /// <summary>
        /// Get color by ID
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Response<ColorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpGet("{id}")]
        public async Task<Response<ColorDto>> GetByIdAsync(int id)
            => await _mediator.Send(new GetColorQuery { Id = id });

        #region Documentation
        /// <summary>
        /// Create new color
        /// </summary>
        /// <param name="request"></param>
        [ProducesResponseType(typeof(Response<ColorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> CreateColorAsync(CreateColorCommand request)
            => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Update existing color
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">Updated color data</param>
        [ProducesResponseType(typeof(Response<ColorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpPut("{id}")]
        public async Task<Response> UpdateColorAsync(int id, UpdateColorCommand request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

        #region Documentation
        /// <summary>
        /// Delete color
        /// </summary>
        /// <param name="id">Color identifier</param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpDelete("{id}")]
        public async Task<Response> DeleteColorAsync(int id)
            => await _mediator.Send(new DeleteColorCommand { Id = id });
    }
}
