namespace Point_of_Sale.Model.Report
{
    public class Product_Export_Model
    {
        public long id { get; set; }
        public long category_id { get; set; }
        public string name { get; set; }
        public double export_price { get; set; }
        public double export_quantity { get; set; }
        public string category_code { get; set; }

    }
    public class Product_Import_Model
    {
        public long id { get; set; }
        public long category_id { get; set; }
        public string name { get; set; }
        public string category_code { get; set; }
        public double import_price { get; set; }
        public double import_quantity { get; set; }

    }
}
