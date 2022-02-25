namespace data.EnterpriseData
{
    public class Product
    {
        public int Id { get; set; }
        public int Barcode { get; set; }
        public double SellingPrice { get; set; }
        public string Name { get; set; }
        //public virtual ProductSupplier ProductSupplier { get; set; }
    }
}