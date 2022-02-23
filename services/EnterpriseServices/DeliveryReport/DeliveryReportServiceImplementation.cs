using data;
using data.EnterpriseData;

namespace services.EnterpriseServices 
{
    public class DeliveryReportServiceImplementation : IDeliveryReportService
    {

        public void addDeliveryReport(TradingsystemDbContext context, DeliveryReports DeliveryReports)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                db.Add(DeliveryReports);
                db.SaveChanges();
            }
        }
        
        public List<DeliveryReports> getAllDeliveryReports(TradingsystemDbContext context)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.DeliveryReports != null ? db.DeliveryReports.ToList() : new List<DeliveryReports>();
            }
        }

        public DeliveryReports getDeliveryReportById(TradingsystemDbContext context, int DeliveryId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.DeliveryReports.First(s => s.Id == DeliveryId);
            }
        }
    }
}
