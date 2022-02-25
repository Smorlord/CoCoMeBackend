using data;
using data.EnterpriseData;

namespace services.EnterpriseServices
{
    public interface IDeliveryReportService
    {
        void addDeliveryReport(TradingsystemDbContext context, DeliveryReports deliveryReports);
        List<DeliveryReports> getAllDeliveryReports(TradingsystemDbContext context);
        DeliveryReports getDeliveryReportById(TradingsystemDbContext context, int DeliveryId);

        DeliveryReports createDeliveryReport(TradingsystemDbContext context, int storeId);
    }
}
