using cashDeskGrpcClient;
using cashDeskService.Display;
using cashDeskService.Printer;
using cashDeskService.Session;
using cashDeskService.CardReader;
using GRPC_PurchaseStoreClient;
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
            if (sessionService.getExpressCheckOut())
            {
                sessionService.setExpressCheckOut(false);
                displayService.changeExpressCheckOut(false);
                Console.WriteLine("Disable Express");
            }
            else
            {
                sessionService.setExpressCheckOut(true);
                displayService.changeExpressCheckOut(true);
                Console.WriteLine("Enable Express");
            }
        }

        public void FinishPurchase()
        {
            sessionService.setSaleFinish(true);
            displayService.showTotalInDisplay(sessionService.getTotalPrice());
            printerService.printItems(sessionService.getScannedProducts());
        }

        public void PayWithCard()
        {
            if (sessionService.getSaleFinish())
            {
                if (!sessionService.getExpressCheckOut())
                {
                    try
                    {
                        cardReaderService.pay(Convert.ToInt64(sessionService.getTotalPrice() * 100));
                        updateInventory();
                        clearPurchase();
                    }
                    catch (Exception ex)
                    {
                        cardReaderService.abort(ex.Message);
                    }
                }
                else
                {
                    displayService.showIsExpressLock();
                }
            } else
            {
                displayService.showNoFinish();
            }
        }

        public void PayWithCash()
        {
            if (sessionService.getSaleFinish())
            {
                updateInventory();
                clearPurchase();
            } else
            {
                displayService.showNoFinish();
            }
        }

        public void StartNewPurchase()
        {
            if (sessionService.getPurchaseId() == -1)
            {
                sessionService.setSaleFinish(false);
                CreatePurchaseStoreDTOLookUpModel model = new CreatePurchaseStoreDTOLookUpModel { StoreId = 1 };
                PurchaseStoreDTO.PurchaseStoreDTOClient client = this.grpcClientConnector.getPurchaseStoreDTOClient();
                CreatePurchaseStoreDTOModel response = client.CreatePurchaseStore(model);
                this.sessionService.updatePurchaseId(response.PurchaseId);
                displayService.showStartPurchase(sessionService.getPurchaseId());
            }
            else
            {
                displayService.purchaseAlreadyExist();
            }
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
                            FinishPurchase();
                            break;

                        case CashboxButton.PayWithCard:
                            PayWithCard();
                            break;

                        case CashboxButton.PayWithCash:
                            PayWithCash();
                            break;

                        case CashboxButton.StartNewSale:
                            StartNewPurchase();
                            break;

                        case CashboxButton.DisableExpressMode:
                            DisableExpress();
                            break;

                    }
                }
            }
        }

        private void clearPurchase()
        {
            displayService.showFinishPurchase();
            this.sessionService.updatePurchaseId(-1);
            this.sessionService.clearScannedProduct();
        }

        private void updateInventory()
        {
            List<ProductStoreDTOLookUpModel> products = new List<ProductStoreDTOLookUpModel>();
            foreach (var scannedProduct in sessionService.getScannedProducts())
            {
                ProductStoreDTOLookUpModel product = new ProductStoreDTOLookUpModel();
                product.Id = scannedProduct.Id;
                products.Add(product);
            }
            UpdatePurchaseStoreDTOLookUpModel data = new UpdatePurchaseStoreDTOLookUpModel();
            data.PurchaseId = sessionService.getPurchaseId();
            data.ProductStoreDTOLookUpModel.AddRange(products);
            UpdatePurchaseStoreDTOModel responseUpdate = this.grpcClientConnector.getPurchaseStoreDTOClient().UpdatePurchaseStore(data);
        }
    }
}

