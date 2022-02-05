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

        public void showInDisplay(ProductScannedDTOModel item)
        {
            displayControllerClient.SetDisplayText($"{item.Name}: {item.PurchasePrice}€");
        }
    }
}
