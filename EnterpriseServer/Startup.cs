using services.EnterpriseServices;
using services.StoreServices;

namespace EnterpriseServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IProductService, ProductServiceImplementation>();
            services.AddSingleton<IPurchaseService, PurchaseServiceImplementation>();
            services.AddSingleton<IStoreService, StoreServiceImplementation>();
            services.AddSingleton<IOrderService, OrderServiceImplementation>();
            services.AddSingleton<IDeliveryReportService, DeliveryReportServiceImplementation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
 
}
