namespace cashDeskService.BarcodeScanner
{
    public interface IBarcodeScannerService
    {

        void init();
        void barcodeScanned(int barcode);
    }
}
