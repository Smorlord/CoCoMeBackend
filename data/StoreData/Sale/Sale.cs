using data.EnterpriseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.StoreData.Sale
{
    public class Sale
    {
        public int id { get; set; }
        public int saleDateTime { get; set; }
        public Store store { get; set; }
        public List<Product> products { get; } = new List<Product>();
    }
}
