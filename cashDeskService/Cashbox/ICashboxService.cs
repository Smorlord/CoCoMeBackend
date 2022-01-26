using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cashDeskService.Cashbox
{
    public interface ICashboxService
    {
        void init();
        void StartNewSale();
        void FinishSale();
        void PayWithCash();
        void PayWithCard();
        void DisableExpress();
    }
}
