namespace Point_of_Sale.Model.Report
{
    public class Export_Product_Fast_Model
    {
        public string parent_code { get; set; } 
        public string date { get; set; }//Ngày chứng từ
        public string note { get; set; } //Diễn giải
        public string receipt_name { get; set; } //Người giao hàng
        public string warehouse_code { get; set; }  //Mã kho
        public string partner_code { get; set; } //Mã ncc
        public string partner_name { get; set; } //Mã ncc
        public string warehouse_name { get; set; }   //Mã ĐVCS
        public long product_id { get; set; }
        public string product_code { get; set; } //Mã vật tư
        public string product_name { get; set; }
        public string product_unit { get; set; }
        public double quantity { get; set; } //Số lượng:Q
        public double price { get; set; } //Giá:P0
        public double total_cost { get; set; } = 0; //Tiền:N0
        public long warehouse_id { get; set; }
    }
   
}
