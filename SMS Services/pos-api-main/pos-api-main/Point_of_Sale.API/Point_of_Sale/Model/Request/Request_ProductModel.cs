namespace Point_of_Sale.Model.Request
{
    public class Request_ProductModel
    {
        public long id { set; get; }
        public long receipt_id { get; set; } 
        public long product_id { get; set; } 
        public long partner_id { get; set; }
        public double quantity { set; get; }
        public double weight { set; get; }
        public string category_unit_code { set; get; }
        public string category_packing_code { set; get; }
        public long warehouse_id { get; set; }
        public string note { set; get; }
        public DateTime exp_date { set; get; }
        public string barcode { set; get; }
    }
}
