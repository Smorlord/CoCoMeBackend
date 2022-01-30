using data.StoreData;
using mockServiceConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void print(Sale sale)
        {
            foreach (var productSale in sale.products)
                this.printingServiceClient.PrintLine(productSale.Product.Name + " " + productSale.Product.PurchasePrice + "€");
        }
    }
}
