using mockServiceConnector;
using data.EnterpriseData;

using TestConsole.DisplayController;

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

        public void showInDisplay(Product item)
        {
            displayControllerClient.SetDisplayText($"{item.Name}: ${item.PurchasePrice}€  scanned");
        }
    }
}
