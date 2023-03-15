namespace Point_of_Sale.Model.Purchase
{
    public class PurchaseModel
    {
        public long id { set; get; }
        public long userAdded { set; get; }
        public long? userUpdated { set; get; }
        public string? code { set; get; }
        public string content { set; get; }
        public string note { set; get; }
        public long warehouse_id { set; get; }
        public long? warehouse_source_id { set; get; } 
        public DateTime transfer_date { set; get; }
        public long partner_id { set; get; }
        public byte status_id { set; get; } 
        public List<PurchaseProductModel>? Products { set; get; } = new List<PurchaseProductModel>();   
    }
    public class PurchaseViewModel
    {
        public string? code { set; get; }
        public string? search_name { set; get; }

        public long id { set; get; }
        public long userAdded { set; get; }
        public long? userUpdated { set; get; }
        public string content { set; get; }
        public string note { set; get; }
        public long warehouse_id { set; get; }
        public string? warehouse_name { set; get; }
        public DateTime transfer_date { set; get; }
        public long partner_id { set; get; }
        public byte status_id { set; get; }
        public string? partner_name { set; get; }
        public double total_amount { set; get; }// tổng tiền phiếu
    }
    public class PurchaseProductModel
    {
        public long id { set; get; }
        public long userAdded { set; get; }
        public long? userUpdated { set; get; }
        public long purchase_id { set; get; } 
        public long product_id { get; set; }
        public double quantity { set; get; }
        public double? quantity_inventory { set; get; } = 0;
        public double import_price { set; get; }
        public double price { set; get; }
        public string? barcode { set; get; }
        public string category_unit_code { set; get; }
        public string category_packing_code { set; get; }
        public string product_name { set; get; } = "";
        public long warehouse_id { get; set; }
        public bool is_weigh { set; get; } 
        public bool is_promotion { set; get; } 
        public string note { set; get; } = "";
     //   public DateTime exp_date { set; get; }
      //  public int warning_date { set; get; }
    //    public string batch_number { set; get; }
        public bool is_delete { get; set; } = false;

    }
}
