using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Product
{
    public class Product_Combo_Model : IAuditableEntity
    {
        public string search_name { get; set; } = string.Empty;

        public long combo_id { get; set; }
        public long product_id { get; set; }
        public double product_quantity { get; set; }
        public string note { get; set; } = "";
        public string? unit_code { get; set; } // mã đon tính 
        public string? product_name { get; set; } // mã đon tính 
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public long? category_id { get; set; } = 0;
        public long? category_stalls_id { get; set; } = 0;
        public long? category_group_id { get; set; } = 0;
    }
}
