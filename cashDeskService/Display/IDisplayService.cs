using data.EnterpriseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cashDeskService.Display
{
    public interface IDisplayService
    {
        void init();
        void showInDisplay(Product item);
    }
}
