﻿using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Inventory
{
    public class Warehouse_Inventory_Product_Model : IAuditableEntity
    {
        public long warehouse_inventory_id { get; set; }
        public long warehouse_id { get; set; }
        public long products_warehouse_id { get; set; }
        public long product_id { get; set; }
        public double quantity_stock { set; get; }// số lượng trong kho
        public double quantity { set; get; }// số lượng thay đổi
        public double import_price { set; get; }
        public double price { set; get; }
        public double sale_price { get; set; } = 0;// giá khuyến mại
        public string unit_code { set; get; }
        public string packing_code { set; get; }
        public bool is_weigh { set; get; } = false;
        public string note { set; get; }
        public string batch_number { set; get; }
        public string barcode { set; get; }
        public string name { set; get; }
        
    }
}
