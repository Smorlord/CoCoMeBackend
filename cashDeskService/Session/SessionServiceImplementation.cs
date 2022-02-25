using GRPC_Client;

namespace cashDeskService.Session
{
    public class SessionServiceImplementation : ISessionService
    {
        private int storeId;
        private int saleId = -1;
        private Boolean saleFinish = false;
        private double totalPrice = 0;
        private List<ProductScannedDTOModel> scannedProducts = new List<ProductScannedDTOModel>();
        private Boolean expressCheckOut = false;

        public void init()
        {
            this.storeId = 1;
        }

        public int getPurchaseId()
        {
            return this.saleId;
        }

        public int getStoreId()
        {
           return this.storeId;
        }

        public void updatePurchaseId(int saleId)
        {
            this.saleId = saleId;
        }

        public void addScannedProduct(ProductScannedDTOModel scannedProduct)
        {
            this.scannedProducts.Add(scannedProduct);
            if (scannedProduct.SalePrice == -1)
            {
                this.totalPrice += scannedProduct.SellingPrice;
            }
            else
            {
                this.totalPrice += scannedProduct.SalePrice;
            }

        }

        public void clearScannedProduct()
        {
            this.scannedProducts.Clear();
        }

        public List<ProductScannedDTOModel> getScannedProducts()
        {
            return scannedProducts;
        }

        public void setTotalPrice(double totalPrice)
        {
            this.totalPrice = totalPrice;
        }

        public double getTotalPrice()
        {
            return this.totalPrice;
        }

        public Boolean getExpressCheckOut()
        {
            return this.expressCheckOut;
        }

        public void setExpressCheckOut(Boolean expressCheckOut) { 
            this.expressCheckOut = expressCheckOut; 
        }

        public Boolean getSaleFinish()
        {
            return saleFinish;
        }

        public void setSaleFinish(Boolean saleFinish)
        {
            this.saleFinish = saleFinish;
        }

    }
}
