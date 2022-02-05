namespace cashDeskService.Session
{
    public class SessionServiceImplementation : ISessionService
    {
        private int storeId;
        private int saleId;

        public void init()
        {
            this.storeId = 1;
        }

        public int getSaleId()
        {
            return this.saleId;
        }

        public int getStoreId()
        {
           return this.storeId;
        }

        public void updateSaleId(int saleId)
        {
            this.saleId = saleId;
        }
    }
}
