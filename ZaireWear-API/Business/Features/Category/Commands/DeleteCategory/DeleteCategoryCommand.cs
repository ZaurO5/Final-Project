using Business.Wrappers;
using MediatR;

namespace Business.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
