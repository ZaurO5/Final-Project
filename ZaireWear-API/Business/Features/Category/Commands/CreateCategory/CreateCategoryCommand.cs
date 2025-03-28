using Business.Wrappers;
using MediatR;

namespace Business.Features.Category.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Response>
    {
        public string Name { get; set; }
    }
}
