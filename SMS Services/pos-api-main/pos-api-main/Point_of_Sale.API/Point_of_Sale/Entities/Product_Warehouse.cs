using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("product_warehouse")]
    public class Product_Warehouse : IAuditableEntity// số lượng trong kho
    { 
        public string? barcode { get; set; } // mã sản phẩm bán
        public long product_id { get; set; }
        public double quantity_sold { set; get; } = 0;// số lượng đã bán
        public double quantity_stock { set; get; } = 0;// số lượng trong kho
        public double import_price { get; set; } // giá nhập
        public double price { get; set; }// giá bán
        public double sale_price { get; set; } = 0;// giá khuyến mại
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public DateTime? exp_date { get; set; }// hạn sử dung
        public long warehouse_id { get; set; }// kho
        public string? batch_number { set; get; }// mã code
        public bool? is_weigh { set; get; } = false;
        public bool? is_printed { set; get; } = false;
        public int? print_bacode { set; get; } = 0;
        public bool is_promotion { set; get; } = false;
    }
}
