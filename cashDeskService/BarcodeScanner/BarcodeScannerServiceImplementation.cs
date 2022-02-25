using cashDeskGrpcClient;
using cashDeskService.Display;
using cashDeskService.Session;
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
        private ISessionService sessionService;
        private BarcodeScannerServiceClient barcodeScannerClient;
        private Tecan.Sila2.IIntermediateObservableCommand<string> readBarcodes;

        public BarcodeScannerServiceImplementation(MockServiceConnector mockService, IGrpcClientConnector grpcClientConnector, IDisplayService displayService, ISessionService sessionService)
        {
            this.mockServiceConnector = mockService;
            this.grpcClientConnector = grpcClientConnector;
            this.displayService = displayService;
            this.sessionService = sessionService;
        }

        public void init()
        {
            barcodeScannerClient = mockServiceConnector.GetBarcodeScannerServiceClient();
            readBarcodes = barcodeScannerClient.ListenToBarcodes();
            listenBarcodes(readBarcodes);
        }

        public void barcodeScanned(int barcode)
        {
            if (sessionService.getPurchaseId() != -1)
            {
                if (!sessionService.getSaleFinish())
                {
                    if ((sessionService.getExpressCheckOut() && sessionService.getScannedProducts().Count() < 7) || !sessionService.getExpressCheckOut())
                    {
                        ProductScannedDTO.ProductScannedDTOClient client = grpcClientConnector.getProductScannedDTOClient();
                        ProductScannedDTOModel response = client.GetProductScannedDTOInfo(new ProductScannedDTOLookUpModel { Barcode = barcode, StoreId = sessionService.getStoreId() });
                        displayService.showItemInDisplay(response);
                        sessionService.addScannedProduct(response);
                    }
                } else
                {
                    displayService.showNoPay();
                }
            } else
            {
                displayService.showNoPurchase();
            }
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
