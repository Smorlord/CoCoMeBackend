
using StoreServer;
using grpcClientStore;
using GRPC_Client;
using Grpc.Net.Client;
using grpcServiceStore.Services;

var builder = WebApplication.CreateBuilder(args);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

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

app.MapGrpcService<ProductScannedGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Services.GetRequiredService<IGrpcClientConnector>().connect();

// The port number must match the port of the gRPC server.
/*using var channel = GrpcChannel.ForAddress("https://localhost:7244");
var client = new ProductDTO.ProductDTOClient(channel);
var reply = client.GetProductDTOInfo(
                    new ProductDTOLookUpModel { Barcode = 1111 });
Console.WriteLine("Store Product: " + reply);*/

app.Run();



