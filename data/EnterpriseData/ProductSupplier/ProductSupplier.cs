using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EnterpriseData
{
    public class ProductSupplier
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }

    }
}
