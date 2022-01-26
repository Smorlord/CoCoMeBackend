using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole;

namespace cashDeskService.BankServer
{
    public class BankServiceImplementation : IBankService
    {
        public void authorizePayment(string contextId, string data, string token)
        {
            throw new NotImplementedException();
        }

        public TransactionContext createContext(int amount)
        {
            throw new NotImplementedException();
        }

        public void init()
        {
            throw new NotImplementedException();
        }
    }
}
