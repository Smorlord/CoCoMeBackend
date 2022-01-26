
using cashDeskService.Cashbox;
using mockServiceConnector;

namespace CashDeskPC
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
            services.AddSingleton<MockServiceConnector, MockServiceConnectorImplementation>();
            services.AddSingleton<ICashboxService, CashboxServiceImplementation>();
            /*services.AddSingleton<LightDisplayService, LightDisplayServiceImplementation>();
            services.AddSingleton<CardReaderService, CardReaderServiceImplementation>();
            
            services.AddSingleton<CashDeskGUIService, CashDeskGUIServiceImplemenation>();
            services.AddSingleton<PrinterService, PrinterServiceImplementation>();
            services.AddSingleton<ScannerService, ScannerServiceImplementation>();*/
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
