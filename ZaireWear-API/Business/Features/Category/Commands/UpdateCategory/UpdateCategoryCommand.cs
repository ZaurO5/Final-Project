using Business.Wrappers;
using MediatR;

namespace Business.Features.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
