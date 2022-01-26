using mockServiceConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole;
using TestConsole.BankServer;

namespace cashDeskService.BankServer
{
    public class BankServiceImplementation : IBankService
    {

        private MockServiceConnector mockServiceConnector;
        private BankServerClient bankServerClient;

        public BankServiceImplementation(MockServiceConnector mockService)
        {
            this.mockServiceConnector = mockService;
        }

        public void init()
        {
            bankServerClient = mockServiceConnector.GetBankServerClient();
        }

        public void authorizePayment(string contextId, string data, string token)
        {
            bankServerClient.AuthorizePayment(contextId, data, token);
        }

        public TransactionContext createContext(int amount)
        {
            return bankServerClient.CreateContext(amount);
        }
    }
}
