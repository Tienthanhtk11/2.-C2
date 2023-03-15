using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("product_partner")]

    public class Product_Partner : IAuditableEntity
    {
        public long product_id { get; set; }
        public long partner_id { get; set; }
        public decimal price { get; set; }
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public string note { get; set; } = "";
    }
}
