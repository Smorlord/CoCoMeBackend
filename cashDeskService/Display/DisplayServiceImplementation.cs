using mockServiceConnector;
using TestConsole.DisplayController;
using GRPC_Client;

namespace cashDeskService.Display
{
    public class DisplayServiceImplementation : IDisplayService
    {
        private MockServiceConnector mockServiceConnector;
        private DisplayControllerClient displayControllerClient;

        public DisplayServiceImplementation(MockServiceConnector mockServiceConnector)
        {
            this.mockServiceConnector = mockServiceConnector;
        }
        public void init()
        {
            displayControllerClient = mockServiceConnector.GetDisplayControllerClient();
        }

        public void showFinishSale()
        {
            displayControllerClient.SetDisplayText("Der Betrag wurde beglichen, vielen Dank für Ihren Einkauf");
        }

        public void showItemInDisplay(ProductScannedDTOModel item)
        {
            if (item.SalePrice == -1) { 
                displayControllerClient.SetDisplayText($"{item.Name}: {item.SellingPrice}€");
            } else
            {
                displayControllerClient.SetDisplayText($"{item.Name}: {item.SalePrice}€ statt {item.SellingPrice}€");
            }
        }

        public void showStartSale(int saleId)
        {
            displayControllerClient.SetDisplayText($"Einkauf {saleId} gestartet, bitte Items scannen");
        }

        public void showTotalInDisplay(double totalAmount)
        {
            displayControllerClient.SetDisplayText($"Der Gesamtbetrag des Einkaufs beträgt {totalAmount}€");
        }
    }
}
