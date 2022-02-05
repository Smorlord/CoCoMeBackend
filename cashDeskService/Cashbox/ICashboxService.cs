namespace cashDeskService.Cashbox
{
    public interface ICashboxService
    {
        void init();
        void StartNewSale();
        void FinishSale();
        void PayWithCash();
        void PayWithCard();
        void DisableExpress();
    }
}
