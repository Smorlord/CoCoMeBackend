using GRPC_Client;
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

        public void printItems(List<ProductScannedDTOModel> ProductScannedDTOModel)
        {
            this.printingServiceClient.PrintLine("------------Neuer Einkauf--------------");

            foreach (var product in ProductScannedDTOModel)
            {
                if (product.SalePrice == -1)
                {
                    this.printingServiceClient.PrintLine($"{product.Name}: {product.SellingPrice}€");
                } else
                {
                    this.printingServiceClient.PrintLine($"{product.Name}: {product.SalePrice}€ statt {product.SellingPrice}€");
                }
            }

            this.printingServiceClient.PrintLine("------------Ende des Einkaufs--------------");
        }
    }
}
