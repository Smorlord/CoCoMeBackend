namespace cashDeskService.Cashbox
{
    public interface ICashboxService
    {
        void init();
        void StartNewPurchase();
        void FinishPurchase();
        void PayWithCash();
        void PayWithCard();
        void DisableExpress();
    }
}
