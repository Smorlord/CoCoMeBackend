using cashDeskGrpcClient;
using cashDeskService.Session;
using GRPC_SaleStoreClient;
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
        private IGrpcClientConnector grpcClientConnector;
        private ISessionService sessionService;
        private Tecan.Sila2.IIntermediateObservableCommand<CashboxButton> cashboxButtons;

        public CashboxServiceImplementation(MockServiceConnector mockService, IGrpcClientConnector grpcClientConnector, ISessionService sessionService)
        {
            this.mockServiceConnector = mockService;
            this.grpcClientConnector = grpcClientConnector;
            this.sessionService = sessionService;

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
            CreateSaleStoreDTOLookUpModel model = new CreateSaleStoreDTOLookUpModel { StoreId = 1 };
            SaleStoreDTO.SaleStoreDTOClient client = this.grpcClientConnector.getSaleStoreDTOClient();
            CreateSaleStoreDTOModel response = client.CreateSaleStore(model);
            this.sessionService.updateSaleId(response.SaleId);
            Console.WriteLine(response.SaleId.ToString());
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

