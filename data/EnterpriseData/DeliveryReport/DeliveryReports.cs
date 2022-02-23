using data.EnterpriseData;
namespace data.EnterpriseData
{
    public class DeliveryReports
    {
        public int Id { get; set; }
        public virtual List<DeliveryReport> DeliveryReport { get; set; }
    }
}
