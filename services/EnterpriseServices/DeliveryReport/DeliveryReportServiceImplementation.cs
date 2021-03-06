using data;
using data.EnterpriseData;
using services.StoreServices;

namespace services.EnterpriseServices 
{
    public class DeliveryReportServiceImplementation : IDeliveryReportService
    {

        private IOrderService orderService;

        public DeliveryReportServiceImplementation(IOrderService orderSerivce)
        {
            this.orderService = orderSerivce;

        }

        public void addDeliveryReport(TradingsystemDbContext context, DeliveryReports DeliveryReports)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                db.Add(DeliveryReports);
                db.SaveChanges();
            }
        }

        public DeliveryReports createDeliveryReport(TradingsystemDbContext context, int storeId)
        {
            using (var db = TradingsystemDbContext.GetContext(context)) {
                DeliveryReports deliveryReports = new DeliveryReports { DeliveryReport = new List<DeliveryReport>() };

                /* 
                 * Für jeden Supplier wird die Produktpalette durchlaufen, als auch die 
                 * die OrderEntries in einer ProductOrder.
                 * Anhand der ProduktId werden überprüft, ob es das Produkt in einer Bestellung gibt
                 * ist dem so, kann die Dauer der Zulieferung in Tagen berechnet und dem suppliert zugeordnet werden. 
                 * Am Ende kann die Durchschnittslieferung für einen Suppliert berechnet und in den 
                 * DeliveryReport hinzugefügt werden
                */
                db.ProductSuppliers.ToList().ForEach(supplier =>
                {
                    DeliveryReport deliveryReport = new DeliveryReport();
                    deliveryReport.creationDate = DateTime.Now;
                    deliveryReport.supplierName = supplier.Name;
                    double totalDays = 0;
                    int count = 0;

                    // Alle ProduktOders von OrderService um die OrderEntries durchlaufen zu können
                    orderService.getAllProductOrdersByStoreId(storeId).ToList().ForEach(productOrder =>
                    {
                        {
                            // for each order in orderentries(orderservice)
                            if (productOrder.DeliveryDate is not null)
                            {
                                productOrder.OrderEntries.ToList().ForEach(order =>
                                {
                                    // for each product in products (supplier)
                                    supplier.Products.ToList().ForEach(product =>
                                    {
                                        // if orderEntry(order).productId == product.id
                                        if (product.Id == order.ProductId)
                                        {
                                            var diff = productOrder.DeliveryDate - productOrder.OrderingDate;
                                            totalDays += (diff ??= TimeSpan.Zero).TotalDays;
                                            count++;
                                        }
                                    });
                                });
                            }
                        }
                       ;
                    });

                    deliveryReport.meanTime = totalDays / count;
                    deliveryReports.DeliveryReport.Add(deliveryReport);
                });

                // kann wenn gewünscht auch in DB gespeichert werden
                // deliveryReportService.createDeliveryReport(context, deliveryReports);
                // db.SaveChanges();

                return deliveryReports;
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
