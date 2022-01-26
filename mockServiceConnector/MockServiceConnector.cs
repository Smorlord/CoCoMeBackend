
using TestConsole.BankServer;
using TestConsole.BarcodeScannerService;
using TestConsole.CardReaderService;
using TestConsole.CashboxService;
using TestConsole.DisplayController;
using TestConsole.PrintingService;

namespace mockServiceConnector

{
    public interface MockServiceConnector
    {

        public void connect();

        public CashboxServiceClient getCashBoxServiceClient();
        public DisplayControllerClient GetDisplayControllerClient();
        public PrintingServiceClient GetPrintingServiceClient();
        public BarcodeScannerServiceClient GetBarcodeScannerServiceClient();
        public CardReaderServiceClient GetCardReaderServiceClient();
        public BankServerClient GetBankServerClient();
    }
}
