using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cashDeskService.BarcodeScanner
{
    public interface IBarcodeScannerService
    {

        void init();
        void barcodeScanned(int barcode);
    }
}
