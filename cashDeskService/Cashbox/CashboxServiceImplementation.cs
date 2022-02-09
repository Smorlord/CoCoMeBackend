using cashDeskGrpcClient;
using cashDeskService.Display;
using cashDeskService.Printer;
using cashDeskService.Session;
using cashDeskService.CardReader;
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
        private IDisplayService displayService;
        private ISessionService sessionService;
        private IPrinterService printerService;
        private CardReader.ICardReaderService cardReaderService;
        private Tecan.Sila2.IIntermediateObservableCommand<CashboxButton> cashboxButtons;

        public CashboxServiceImplementation(MockServiceConnector mockService, IGrpcClientConnector grpcClientConnector, ISessionService sessionService, IDisplayService displayService, IPrinterService printerService, CardReader.ICardReaderService cardReaderService)
        {
            this.mockServiceConnector = mockService;
            this.grpcClientConnector = grpcClientConnector;
            this.sessionService = sessionService;
            this.displayService = displayService;
            this.printerService = printerService;
            this.cardReaderService = cardReaderService;
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

            List<ProductStoreDTOLookUpModel> products = new List<ProductStoreDTOLookUpModel>();
            foreach (var scannedProduct in sessionService.getScannedProducts())
            {
                ProductStoreDTOLookUpModel product = new ProductStoreDTOLookUpModel();
                product.Id = scannedProduct.Id;
                products.Add(product);
            }
            UpdateSaleStoreDTOLookUpModel data = new UpdateSaleStoreDTOLookUpModel();
            data.SaleId = sessionService.getSaleId();
            data.ProductStoreDTOLookUpModel.AddRange(products);
            UpdateSaleStoreDTOModel responseUpdate = this.grpcClientConnector.getSaleStoreDTOClient().UpdateSaleStore(data);
            displayService.showTotalInDisplay(responseUpdate.SalePriceTotal);
            printerService.printItems(responseUpdate.ProductStoreDTOModel.ToList());
            sessionService.setTotalPrice(responseUpdate.SalePriceTotal);
        }

        public void PayWithCard()
        {
            try
            {
                cardReaderService.pay(Convert.ToInt64(sessionService.getTotalPrice() * 100));
            }
            catch (Exception ex)
            {
                cardReaderService.abort(ex.Message);
            }
        }

        public void PayWithCash()
        {
            displayService.showFinishSale();
            this.sessionService.updateSaleId(-1);
            this.sessionService.clearScannedProduct();
        }

        public void StartNewSale()
        {
            CreateSaleStoreDTOLookUpModel model = new CreateSaleStoreDTOLookUpModel { StoreId = 1 };
            SaleStoreDTO.SaleStoreDTOClient client = this.grpcClientConnector.getSaleStoreDTOClient();
            CreateSaleStoreDTOModel response = client.CreateSaleStore(model);
            this.sessionService.updateSaleId(response.SaleId);
            displayService.showStartSale(sessionService.getSaleId());
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

