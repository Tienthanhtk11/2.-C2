namespace Point_of_Sale.Model.Report
{
    public class InOutInventoryProductModel
    {
        public long product_warehouse_id { get; set; }
        public long product_id { get; set; }
        public long warehouse_id { get; set; }
        public string name { get; set; }
        public string warehouse_name { get; set; } 
        public string? barcode { get; set; }
        public string? category_name { get; set; }
        public string? category_code { get; set; }
        public string? unit_code { get; set; }
        
        public double quantity_in { get; set; }
        public double quantity_out { get; set; }
        public double quantity_sold { get; set; }
        public double quantity_inventory { get; set; }
        public double quantity_return { get; set; } 
    }
}
