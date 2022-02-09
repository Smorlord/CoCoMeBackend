using GRPC_PurchaseStoreClient;
using mockServiceConnector;
using TestConsole.PrintingService;

namespace cashDeskService.Printer
{
    public class PrinterServiceImplementation : IPrinterService
    {
        private MockServiceConnector mockServiceConnector;
        private PrintingServiceClient printingServiceClient;
        public PrinterServiceImplementation(MockServiceConnector mockService)
        {
            this.mockServiceConnector = mockService;
            
        }
        public void init()
        {
            this.printingServiceClient = this.mockServiceConnector.GetPrintingServiceClient();
        }

        public void printItems(List<ProductStoreDTOModel> ProductStoreDTOModels)
        {
            this.printingServiceClient.PrintLine("------------Neuer Einkauf--------------");

            foreach (var product in ProductStoreDTOModels)
            {
                if (product.SalePrice == -1)
                {
                    this.printingServiceClient.PrintLine($"{product.Name}: {product.PurchasePrice}€");
                } else
                {
                    this.printingServiceClient.PrintLine($"{product.Name}: {product.SalePrice}€ statt {product.PurchasePrice}€");
                }
            }

            this.printingServiceClient.PrintLine("------------Ende des Einkaufs--------------");
        }
    }
}
