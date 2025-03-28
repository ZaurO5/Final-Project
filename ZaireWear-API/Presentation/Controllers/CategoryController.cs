using Business.Features.Category.Commands.CreateCategory;
using Business.Features.Category.Commands.DeleteCategory;
using Business.Features.Category.Commands.UpdateCategory;
using Business.Features.Category.Dtos;
using Business.Features.Category.Queries.GetAllCategories;
using Business.Features.Category.Queries.GetCategory;
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
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Documentation
        /// <summary>
        /// Categories List
        /// </summary>
        [ProducesResponseType(typeof(Response<List<CategoryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet]
        public async Task<Response<List<CategoryDto>>> GetAllAsync()
            => await _mediator.Send(new GetAllCategoriesQuery());

        #region Documentation
        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Response<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpGet("{id}")]
        public async Task<Response<CategoryDto>> GetByIdAsync(int id)
            => await _mediator.Send(new GetCategoryQuery { Id = id });

        #region Documentation
        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="request"></param>
        [ProducesResponseType(typeof(Response<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> CreateCategoryAsync(CreateCategoryCommand request)
        => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Update existing category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">Updated category data</param>
        [ProducesResponseType(typeof(Response<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpPut("{id}")]
        public async Task<Response> UpdateCategoryAsync(int id, UpdateCategoryCommand request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

        #region Documentation
        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id">Category identifier</param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        #endregion
        [HttpDelete("{id}")]
        public async Task<Response> DeleteCategoryAsync(int id)
            => await _mediator.Send(new DeleteCategoryCommand { Id = id });
    }
}
