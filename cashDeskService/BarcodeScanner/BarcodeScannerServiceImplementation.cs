using cashDeskGrpcClient;
using cashDeskService.Display;
using GRPC_Client;
using mockServiceConnector;
using TestConsole.BarcodeScannerService;

namespace cashDeskService.BarcodeScanner
{
    public class BarcodeScannerServiceImplementation : IBarcodeScannerService
    {

        private MockServiceConnector mockServiceConnector;
        private IGrpcClientConnector grpcClientConnector;
        private IDisplayService displayService;
        private BarcodeScannerServiceClient barcodeScannerClient;
        private Tecan.Sila2.IIntermediateObservableCommand<string> readBarcodes;

        public BarcodeScannerServiceImplementation(MockServiceConnector mockService, IGrpcClientConnector grpcClientConnector, IDisplayService displayService)
        {
            this.mockServiceConnector = mockService;
            this.grpcClientConnector = grpcClientConnector;
            this.displayService = displayService;
        }

        public void init()
        {
            barcodeScannerClient = mockServiceConnector.GetBarcodeScannerServiceClient();
            readBarcodes = barcodeScannerClient.ListenToBarcodes();
            listenBarcodes(readBarcodes);
        }

        public void barcodeScanned(int barcode)
        {
            ProductScannedDTOModel response = grpcClientConnector.getProductScannedDTOClient().GetProductScannedDTOInfo(new ProductScannedDTOLookUpModel { Barcode = barcode });
            displayService.showInDisplay(response);
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
