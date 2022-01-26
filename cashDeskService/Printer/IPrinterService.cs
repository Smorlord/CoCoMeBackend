using data.StoreData.Sale;

namespace cashDeskService.Printer
{
    public interface IPrinterService
    {
        void init();
        void print(Sale sale);
    }
}
