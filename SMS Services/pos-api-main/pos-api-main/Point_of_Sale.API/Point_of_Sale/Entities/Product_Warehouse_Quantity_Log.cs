﻿namespace Point_of_Sale.Entities
{
    public class Product_Warehouse_Quantity_Log
    {
        public string code { get; set; }// mã sản phẩm bán
        public string? barcode { get; set; }
        public long product_id { get; set; } //id san pham dong bo ecom
        public long product_warehouse_id { get; set; } // id trong bang product_warehouse
        public double quantity { set; get; } = 0;// số lượng đã bán
        public double import_price { get; set; } // giá nhập
        public double price { get; set; }// giá bán
        public double sale_price { get; set; }// giá khuyến mại
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public DateTime? exp_date { get; set; }// hạn sử dung
        public int warning_date { set; get; }// ngày cảnh báo trước khi hết hạn
        public long warehouse_id { get; set; }// kho
        public string? batch_number { set; get; }// mã code
        public bool is_promotion { set; get; } = false; // sản phẩm khuyến mại
    }
}
