using EbookApplication.interfaces;
using EbookApplication.Services;
using EbookDomain.Interfaces;
using EbookInfraData.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EbookInfraIOC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Application Layer 
            services.AddScoped<IBookService, BookService>();

            // Infra.Data Layer
            services.AddScoped<IBookRepository, BookRepository>();
        }
    }
}