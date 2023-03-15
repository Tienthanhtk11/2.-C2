using System.ComponentModel.DataAnnotations.Schema;
namespace Point_of_Sale.Entities
{
    [Table("warehouse_export_product")]
    public class Warehouse_Export_Product : IAuditableEntity
    {
        public long products_warehouse_id { get; set; }
        public long partner_id { get; set; }
        public long export_id { get; set; }
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
        public bool is_promotion { set; get; } = false;

    }
}
