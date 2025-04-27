using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Data.Interfaces;
using ProductManagement.Data.Repositories;
using ProductManagement.Services.Interfaces;
using ProductManagement.Services.Services;
using ProductManagement.WebApi.Middlewares;

namespace ProductManagement.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddScoped<ProductDbContext>();

            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("connection"));
            });

            MapServices(services);

            MapRepositories(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            var option = new RewriteOptions();

            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void MapServices(IServiceCollection services)
        {
            services.AddScoped<ExceptionMiddleware>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStockService, StockService>();
        }

        private static void MapRepositories(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
