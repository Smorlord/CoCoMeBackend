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

        public void showFinishPurchase()
        {
            displayControllerClient.SetDisplayText("Der Betrag wurde beglichen, vielen Dank für Ihren Einkauf");
        }

        public void showItemInDisplay(ProductScannedDTOModel item)
        {
            if (item.SalePrice == -1)
            {
                displayControllerClient.SetDisplayText($"{item.Name}: {item.SellingPrice}€");
            }
            else
            {
                displayControllerClient.SetDisplayText($"{item.Name}: {item.SalePrice}€ statt {item.SellingPrice}€");
            }
        }

        public void showNoPurchase()
        {
            displayControllerClient.SetDisplayText($"Kein Einkauf gefunden, bitte drücken Sie erst den Button 'Start New Sale' ");
        }

        public void showStartPurchase(int saleId)
        {
            displayControllerClient.SetDisplayText($"Einkauf {saleId} gestartet, bitte Items scannen");
        }

        public void showTotalInDisplay(double totalAmount)
        {
            displayControllerClient.SetDisplayText($"Der Gesamtbetrag des Einkaufs beträgt {totalAmount}€");
        }

        public void changeExpressCheckOut(Boolean expressCheckOut) 
        {
            if(expressCheckOut) 
            {
                displayControllerClient.SetDisplayText($"Express Checkout aktiviert!");
            }
            else
            {
                displayControllerClient.SetDisplayText($"Express Checkout deaktiviert!");
            }
        }

        public void showIsExpressLock()
        {
            displayControllerClient.SetDisplayText("Keine Kartenzahlung möglich, der Express Check Out ist aktiviert!");
        }
    }
}
