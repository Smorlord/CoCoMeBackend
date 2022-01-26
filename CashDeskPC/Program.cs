using CashDeskPC;
using cashDeskService.BankServer;
using cashDeskService.BarcodeScanner;
using cashDeskService.CardReader;
using cashDeskService.Cashbox;
using cashDeskService.Display;
using cashDeskService.Printer;

var builder = WebApplication.CreateBuilder(args);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Manually create an instance of the Startup class
var startup = new Startup(builder.Configuration);

// Manually call ConfigureServices()
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Call Configure(), passing in the dependencies
startup.Configure(app, app.Lifetime);

app.Services.GetRequiredService<ICashboxService>().init();
app.Services.GetRequiredService<IBankService>().init();
app.Services.GetRequiredService<IBarcodeScannerService>().init();
app.Services.GetRequiredService<ICardReaderService>().init();
app.Services.GetRequiredService<IDisplayService>().init();
app.Services.GetRequiredService<IPrinterService>().init();

app.Run();


