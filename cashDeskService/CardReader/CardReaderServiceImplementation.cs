using cashDeskService.BankServer;
using mockServiceConnector;
using TestConsole;
using TestConsole.CardReaderService;

namespace cashDeskService.CardReader
{
    public class CardReaderServiceImplementation : ICardReaderService
    {

        private MockServiceConnector mockServiceConnector;
        private CardReaderServiceClient cardReaderServiceClient;
        private IBankService bankService;

        public CardReaderServiceImplementation(MockServiceConnector mockService, IBankService bankService)
        {
            this.mockServiceConnector = mockService;
            this.bankService = bankService;
        }

        public void init()
        {
            cardReaderServiceClient = mockServiceConnector.GetCardReaderServiceClient();
        }

        public void pay(long amount)
        {
            TransactionContext context = bankService.createContext(amount);
            Tecan.Sila2.IObservableCommand<AuthorizationData> authorizeCommand = cardReaderServiceClient.Authorize(amount, context.Challenge);
            AuthorizationData token = authorizeCommand.Response.Result;
            try
            {
                bankService.authorizePayment(context.ContextId, token.Account, token.AuthorizationToken);
                cardReaderServiceClient.Confirm();
            }
            catch (Exception ex)
            {
                abort(ex.Message);
            }
        }

        public void abort(string message)
        {
            cardReaderServiceClient.Abort(message);
        }
    }
}
