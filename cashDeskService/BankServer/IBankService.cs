using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole;

namespace cashDeskService.BankServer
{
    public interface IBankService
    {

        void init();
        TransactionContext createContext(long amount);
        void authorizePayment(string contextId, string data, string token);
    }
}
