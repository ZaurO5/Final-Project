using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Business.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationExtensions(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
