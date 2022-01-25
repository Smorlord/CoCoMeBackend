﻿using data.EnterpriseData;
namespace data.StoreData
{
    public class StockItem
    {

        public int Id { get; set; }
        public double SalesPrice { get; set; }
        public int Amount { get; set; }

        public int MinStock { get; set; }

        public int MaxStock { get; set; }

        public List<Product> Products = new List<Product>();


    }
}