using mockServiceConnector;
using Tecan.Sila2;
using TestConsole;
using TestConsole.CashboxService;

namespace cashDeskService.Cashbox
{
    public class CashboxServiceImplementation : ICashboxService
    {
        private MockServiceConnector mockServiceConnector;
        private CashboxServiceClient cashboxClient;
        private Tecan.Sila2.IIntermediateObservableCommand<CashboxButton> cashboxButtons;
        public CashboxServiceImplementation(MockServiceConnector mockService)
        {
            this.mockServiceConnector = mockService;

        }
        public void init()
        {
            cashboxClient = mockServiceConnector.getCashBoxServiceClient();
            cashboxButtons = cashboxClient.ListenToCashdeskButtons();
            ButtonListener(cashboxButtons);
        }

        public void DisableExpress()
        {
            Console.WriteLine("DisableExpress");
        }

        public void FinishSale()
        {
            Console.WriteLine("FinishSale");
        }

        public void PayWithCard()
        {
            throw new NotImplementedException();
        }

        public void PayWithCash()
        {
            throw new NotImplementedException();
        }

        public void StartNewSale()
        {
            /*Console.WriteLine("My StartNewSale");
            var reply = await client.GetProductDTOInfoAsync(
                    new ProductDTOLookUpModel { Barcode = 1111 });*/
        }

        async private void ButtonListener(IIntermediateObservableCommand<CashboxButton> cashboxButtons)
        {
            while (await cashboxButtons.IntermediateValues.WaitToReadAsync())
            {
                if (cashboxButtons.IntermediateValues.TryRead(out var button))
                {
                    switch (button)
                    {
                        case CashboxButton.FinishSale:
                            FinishSale();
                            break;

                        case CashboxButton.PayWithCard:
                            PayWithCard();
                            break;

                        case CashboxButton.PayWithCash:
                            PayWithCash();
                            break;

                        case CashboxButton.StartNewSale:
                            StartNewSale();
                            break;

                        case CashboxButton.DisableExpressMode:
                            DisableExpress();
                            break;

                    }
                }
            }
        }
    }
}

