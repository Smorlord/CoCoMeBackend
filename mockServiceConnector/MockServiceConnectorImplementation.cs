using System.Net.NetworkInformation;
using Tecan.Sila2;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;
using TestConsole;
using TestConsole.BankServer;
using TestConsole.BarcodeScannerService;
using TestConsole.CardReaderService;
using TestConsole.CashboxService;
using TestConsole.DisplayController;
using TestConsole.PrintingService;


namespace mockServiceConnector
{
    public class MockServiceConnectorImplementation : MockServiceConnector
    {

        private CashboxServiceClient cashboxClient;
        private DisplayControllerClient displayClient;
        private PrintingServiceClient printerClient;
        private BarcodeScannerServiceClient barcodeClient;
        private CardReaderServiceClient cardReaderClient;
        private BankServerClient bankServerClient;


        public MockServiceConnectorImplementation()
        {
            connect();
        }

        public void connect()
        {
            var connector = new ServerConnector(new DiscoveryExecutionManager());
            var discovery = new ServerDiscovery(connector);
            var executionManagerFactory = new ExecutionManagerFactory(Enumerable.Empty<IClientRequestInterceptor>());

            var servers = discovery.GetServers(TimeSpan.FromSeconds(10), n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);

            var terminalServer = servers.First(s => s.Info.Type == "Terminal");
            var bankServer = servers.FirstOrDefault(s => s.Info.Type == "BankServer");

            var terminalServerExecutionManager = executionManagerFactory.CreateExecutionManager(terminalServer);

            cashboxClient = new CashboxServiceClient(terminalServer.Channel, terminalServerExecutionManager);
            displayClient = new DisplayControllerClient(terminalServer.Channel, terminalServerExecutionManager);
            printerClient = new PrintingServiceClient(terminalServer.Channel, terminalServerExecutionManager);
            barcodeClient = new BarcodeScannerServiceClient(terminalServer.Channel, terminalServerExecutionManager);
            cardReaderClient = new CardReaderServiceClient(terminalServer.Channel, terminalServerExecutionManager);
            bankServerClient = new BankServerClient(bankServer.Channel, executionManagerFactory.CreateExecutionManager(bankServer));
        }

        public BankServerClient GetBankServerClient()
        {
            return bankServerClient;
        }

        public BarcodeScannerServiceClient GetBarcodeScannerServiceClient()
        {
            return barcodeClient;
        }

        public CardReaderServiceClient GetCardReaderServiceClient()
        {
            return cardReaderClient;
        }

        public CashboxServiceClient getCashBoxServiceClient()
        {
            return cashboxClient;
        }

        public DisplayControllerClient GetDisplayControllerClient()
        {
            return displayClient;
        }

        public PrintingServiceClient GetPrintingServiceClient()
        {
            return printerClient;
        }
    }
}
