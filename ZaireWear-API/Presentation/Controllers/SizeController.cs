using Business.Features.Size.Commands.CreateSize;
using Business.Features.Size.Commands.DeleteSize;
using Business.Features.Size.Commands.UpdateSize;
using Business.Features.Size.Dtos;
using Business.Features.Size.Queries.GetAllSizes;
using Business.Features.Size.Queries.GetSize;
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
    public class SizesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SizesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Documentation
        /// <summary>
        /// Sizes List
        /// </summary>
        [ProducesResponseType(typeof(Response<List<SizeDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet]
        public async Task<Response<List<SizeDto>>> GetAllAsync()
            => await _mediator.Send(new GetAllSizesQuery());

        #region Documentation
        /// <summary>
        /// Get size by ID
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Response<SizeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpGet("{id}")]
        public async Task<Response<SizeDto>> GetByIdAsync(int id)
            => await _mediator.Send(new GetSizeQuery { Id = id });

        #region Documentation
        /// <summary>
        /// Create new size
        /// </summary>
        /// <param name="request"></param>
        [ProducesResponseType(typeof(Response<SizeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> CreateSizeAsync(CreateSizeCommand request)
            => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Update existing size
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">Updated size data</param>
        [ProducesResponseType(typeof(Response<SizeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpPut("{id}")]
        public async Task<Response> UpdateSizeAsync(int id, UpdateSizeCommand request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

        #region Documentation
        /// <summary>
        /// Delete size
        /// </summary>
        /// <param name="id">Size identifier</param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpDelete("{id}")]
        public async Task<Response> DeleteSizeAsync(int id)
            => await _mediator.Send(new DeleteSizeCommand { Id = id });
    }
}
