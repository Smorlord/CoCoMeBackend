
using cashDeskService.BankServer;
using cashDeskService.BarcodeScanner;
using cashDeskService.CardReader;
using cashDeskService.Cashbox;
using cashDeskService.Display;
using cashDeskService.Printer;
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
            services.AddSingleton<IDisplayService, DisplayServiceImplementation>();
            services.AddSingleton<ICardReaderService, CardReaderServiceImplementation>();
            services.AddSingleton<IPrinterService, PrinterServiceImplementation>();
            services.AddSingleton<IBarcodeScannerService, BarcodeScannerServiceImplementation>();
            services.AddSingleton<IBankService, BankServiceImplementation>();
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
