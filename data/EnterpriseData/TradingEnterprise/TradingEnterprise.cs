using data.StoreData;
namespace data.EnterpriseData
{
    public class TradingEnterprise
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Store> products { get; }


    }
}