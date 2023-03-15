namespace Point_of_Sale.Model.Report
{
    public class Stock_Product
    {
        public DateTime datetime{ get; set; }  
        public string warehouse_code { get; set; }
        public string product_name { get; set; } = "";
        public string product_unit_code { get; set; } = "";
        public string product_category_code { get; set; }
        public string product_barcode { get; set; } 
        public double quantity { get; set; }
        public double import_price { get; set; }
        public double stock_cost { get; set; }
        public double export_price { get; set; }
        public long product_warehouse_id { get; set; }


    }
}
