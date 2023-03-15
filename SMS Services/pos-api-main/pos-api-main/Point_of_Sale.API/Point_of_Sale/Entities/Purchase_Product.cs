using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("purchase_product")]
    public class Purchase_Product:IAuditableEntity
    {
        public long purchase_id { set; get; } 
        public long product_id { get; set; }
        public double quantity { set; get; }
        public double import_price { set; get; }
        public double price { set; get; }
        public string? barcode { set; get; }
        public string category_unit_code { set; get; }
        public string category_packing_code { set; get; } 
        public long warehouse_id { get; set; }
        public bool is_weigh { set; get; } = false;
        public bool is_promotion { set; get; } = false;
        public string note { set; get; }
/// public DateTime exp_date { set; get; }
     //   public int warning_date { set; get; }
       // public string batch_number { set; get; }
    }
}
