namespace Point_of_Sale.Model.Receipt
{
    public class Receipt_Print_Model
    {
        public long id { get; set; }
        public long partner_id { get; set; }
        public string? partner_name { get; set; }
        public string? partner_phone { get; set; }
        public string? code { get; set; }
        public long warehouse_id { get; set; }
        public string? warehouse_name { get; set; }
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime receipt_date { get; set; }
        public string? delivery_address { get; set; }
        public byte type { get; set; }
        public long userAdded { set; get; }
        public string user_name { get; set; }
        public List<Receipt_Print_Product>? products { set; get; }
    }

    public class Receipt_Print_Product
    {
        public long products_warehouse_id { get; set; }
        public long partner_id { get; set; }
        public long receipt_id { get; set; }
        public string name { get; set; }
        public long product_id { get; set; }
        public double quantity { set; get; }
        public double import_price { set; get; }
        public double price { set; get; }
        public string unit_code { set; get; }
        public string packing_code { set; get; }
        public long warehouse_id { get; set; }
        public string? note { set; get; }
        public DateTime? exp_date { set; get; }
        public int warning_date { set; get; }
        public string batch_number { set; get; }
        public string? barcode { set; get; }
        public bool? is_weigh { set; get; }
    }

}
