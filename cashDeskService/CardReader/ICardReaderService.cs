using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole;

namespace cashDeskService.CardReader
{
    public interface ICardReaderService
    {

        void init();
        void pay(long amount);

        void abort(string message);
    }
}
