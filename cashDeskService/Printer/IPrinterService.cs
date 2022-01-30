using data.StoreData;

namespace cashDeskService.Printer
{
    public interface IPrinterService
    {
        void init();
        void print(Sale sale);
    }
}
