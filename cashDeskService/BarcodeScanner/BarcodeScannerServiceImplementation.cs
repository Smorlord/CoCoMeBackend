using mockServiceConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole.BarcodeScannerService;

namespace cashDeskService.BarcodeScanner
{
    public class BarcodeScannerServiceImplementation : IBarcodeScannerService
    {

        private MockServiceConnector mockServiceConnector;
        private BarcodeScannerServiceClient barcodeScannerClient;
        private Tecan.Sila2.IIntermediateObservableCommand<string> readBarcodes;

        public BarcodeScannerServiceImplementation(MockServiceConnector mockService)
        {
            this.mockServiceConnector = mockService;
        }

        public void init()
        {
            barcodeScannerClient = mockServiceConnector.GetBarcodeScannerServiceClient();
            readBarcodes = barcodeScannerClient.ListenToBarcodes();
        }

        public void barcodeScanned(int barcode)
        {
            throw new NotImplementedException();
        }

        async private void listenBarcodes(Tecan.Sila2.IIntermediateObservableCommand<string> readBarcodes)
        {
            while (await readBarcodes.IntermediateValues.WaitToReadAsync())
            {
                if (readBarcodes.IntermediateValues.TryRead(out var barcode))
                {
                    barcodeScanned(Int16.Parse(barcode));
                }
            }
        }
    }
}
