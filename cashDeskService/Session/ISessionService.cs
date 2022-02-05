namespace cashDeskService.Session
{
    public interface ISessionService
    {
        void init();
        void updateSaleId(int saleId);
        int getStoreId();
        int getSaleId();
    }
}
