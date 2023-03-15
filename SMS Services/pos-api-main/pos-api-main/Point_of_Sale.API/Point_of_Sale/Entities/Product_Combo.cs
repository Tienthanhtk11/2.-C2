using Point_of_Sale.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Controllers
{
    [Table("product_combo")]

    public class Product_Combo : IAuditableEntity
    {
        public long combo_id { get; set; }
        public long product_id { get; set; }
        public double product_quantity { get; set; }
        public string note { get; set; } = "";
        public string? unit_code { get; set; } // mã đon tính 
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public long? category_id { get; set; } = 0;
        public long? category_stalls_id { get; set; } = 0;
        public long? category_group_id { get; set; } = 0;
        public string? item_code { get; set; }

    }
}
