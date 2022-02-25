using data;
using data.EnterpriseData;
using Microsoft.AspNetCore.Mvc;
using services.EnterpriseServices;
using services.StoreServices;

namespace EnterpriseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryReportController
    {
        private readonly ILogger<OrderController> _logger;
        private IOrderService orderService;
        private IDeliveryReportService deliveryReportService;

        public DeliveryReportController(
            ILogger<OrderController> logger,
            IOrderService orderService,
            IDeliveryReportService deliveryReportService)
        {
            _logger = logger;
            this.orderService = orderService;
            this.deliveryReportService = deliveryReportService;
        }

        [HttpPost]
        [Route("/delivery-report")]
        public DeliveryReports GetDeliveryReport(TradingsystemDbContext context)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                
                DeliveryReports deliveryReports = new DeliveryReports();

                /* 
                 * Für jeden Supplier wird die Produktpalette durchlaufen, als auch die 
                 * die OrderEntries in einer ProductOrder.
                 * Anhand der ProduktId werden überprüft, ob es das Produkt in einer Bestellung gibt
                 * ist dem so, kann die Dauer der Zulieferung in Tagen berechnet und dem suppliert zugeordnet werden. 
                 * Am Ende kann die Durchschnittslieferung für einen Suppliert berechnet und in den 
                 * DeliveryReport hinzugefügt werden
                */
                db.ProductSuppliers.ToList().ForEach( supplier =>
                {
                    DeliveryReport deliveryReport = new DeliveryReport();
                    deliveryReport.creationDate = DateTime.Now;
                    deliveryReport.supplierName = supplier.Name;
                    double totalDays = 0;
                    int count = 0;
                    
                    // Alle ProduktOders von OrderService um die OrderEntries durchlaufen zu können
                    orderService.getAllProductOrders().ToList().ForEach( productOrder =>
                    {
                        // for each order in orderentries(orderservice)
                        productOrder.OrderEntries.ToList().ForEach( order =>
                        {
                            // for each product in products (supplier)
                            supplier.Products.ToList().ForEach( product =>
                            {
                                // if orderEntry(order).productId == product.id
                                if (product.Id == order.ProductId)
                                {
                                    totalDays += (productOrder.DeliveryDate - productOrder.OrderingDate).TotalDays;
                                    count ++;
                                }
                            });
                        });
                    });

                    deliveryReport.meanTime = totalDays / count;
                    deliveryReports.DeliveryReport.Add(deliveryReport);
                    db.SaveChanges();

                });

                // kann wenn gewünscht auch in DB gespeichert werden
                // deliveryReportService.createDeliveryReport(context, deliveryReports);
                // db.SaveChanges();

                return deliveryReports;
                
                
                
            }
        }
    }
}
