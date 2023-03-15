using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Product
{
    public class ProductViewModel
    {
        public long id { set; get; }
        public long userAdded { set; get; }
        public long? userUpdated { set; get; }
        public DateTime dateAdded { get; set; }
        public string code { get; set; } = string.Empty;
        public string? name { get; set; }
        public string? note { get; set; }
        public string? category_code { get; set; }
        public string? category_name { get; set; }
        public string? item_code { get; set; }
        public bool is_active { get; set; }
        public double price { get; set; } = 0;
        public string? partner_name { get; set; }
        public long? partner_id { get; set; }
        public string search_name { get; set; } = string.Empty;

        public string? barcode { get; set; }
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
    }

    public class ProductModel
    {
        public string search_name { get; set; } = string.Empty;
        public string? item_code { get; set; }
        public long? category_id { get; set; } = 0;
        public long? category_stalls_id { get; set; } = 0;
        public long? category_group_id { get; set; } = 0;
        public long id { set; get; }
        public long userAdded { set; get; }
        public long? userUpdated { set; get; }
        public DateTime dateAdded { get; set; }
        public DateTime? dateUpdated { get; set; }
        public bool is_delete { get; set; } = false;
        public string code { get; set; } = string.Empty;
        public string? name { get; set; }
        public string? note { get; set; }
        public bool is_active { get; set; }
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public double price { get; set; } = 0;
        public string? category_code { get; set; }
        public string? barcode { get; set; }

        public List<Product_Warehouse>? listDetail { get; set; }
    }
    public class ProductWarehouseModel : IAuditableEntity
    {
        public string search_name { get; set; } = string.Empty;

        public string? name { get; set; }
        public long? partner_id { get; set; }
        public string? partner_name { get; set; }
        public string? category_product_name { get; set; }
        public string? barcode { get; set; }
        public long product_id { get; set; }
        public double quantity_sold { set; get; } = 0;// số lượng đã bán
        public double quantity_stock { set; get; } = 0;// số lượng trong kho
        public double price { get; set; }// giá bán
        public double import_price { get; set; }// giá nhap
        public double sale_price { get; set; } = 0;// giá khuyến mại
        public string? unit_code { get; set; } // mã đon tính
        public string? unit_name { get; set; } // mã đon tính
        public string? category_code { set; get; }
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public string? packing_name { get; set; }// mã quy cách đóng gói
        public DateTime? exp_date { get; set; }// hạn sử dung
        public int warning_date { set; get; }// ngày cảnh báo trước khi hết hạn
        public long warehouse_id { get; set; }// kho
        public string? batch_number { set; get; }// mã code
        public bool? is_weigh { set; get; } = false;
        public bool? is_printed { set; get; } = false;
        public int? print_bacode { set; get; }
        public bool is_promotion { set; get; } = false;


    }
    public class Warehouse_Receipt_View : IAuditableEntity
    {
        public long? partner_id { get; set; }
        public long? product_id { get; set; }
        public string? partner_name { get; set; }
    }
    public class ProductWarehouseModel2 : IAuditableEntity
    {
        public string search_name { get; set; } = string.Empty;

        public string? code  { get; set; }
        public string? name { get; set; }
        public string? barcode { get; set; }
        public long product_id { get; set; }
        public double price { get; set; }// giá bán
        public double sale_price { get; set; } = 0;// giá khuyến mại
        public string? unit_code { get; set; } // mã đon tính
        public string? unit_name { get; set; } // mã đon tính
        public string? category_code { set; get; }
        public string? category_name { set; get; }
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public string? packing_name { get; set; }// mã quy cách đóng gói
        public long warehouse_id { get; set; }// kho
        public string? batch_number { set; get; }// mã code
        public bool is_promotion { set; get; } = false;
        public string? note { get; set; }

        public List<Product_Combo_Model>? childs { get; set; }

    }

    public class ProductSearchModel
    {
        public string? keyword { set; get; }
        public string? category_code { set; get; }
        public long? warehouse_id { set; get; }
        public long? partner_id { set; get; }
        public bool? is_scale { set; get; }
        public byte status_price { set; get; } = 0; // nếu =0 thì tất cả. 1 chưa có giá, 2 là đã có giá
        public DateTime? start_date { set; get; }
        public DateTime? end_date { set; get; }
        public int page_size { set; get; }
        public int page_number { set; get; }

    }
}
