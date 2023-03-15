namespace Point_of_Sale.Model.Report
{
    public class Revenue_Book_Report
    {
        public long warehouse_id { get; set; } = 0;  // id điểm bán
        public string warehouse_name { get; set; }  // tên kho
        public double opening_cash { get; set; } = 0; //Dư đầu kỳ
        public double session_cash { get; set; } = 0;// Tồn Trong Kỳ
        public double closing_cash { get; set; } = 0;// Dư cuối kỳ
        public List<Revenue_Book_Report_Detail> details { get; set; }
    }

    public class Revenue_Book_Report_Detail
    {
        public long? id { get; set; }   
        public DateTime date { get; set; }
        public string code { get; set; }   // mã đơn hàng
        public long customer_id { get; set; }
        public string customer_name { get; set; }
        public long warehouse_id { get; set; } = 0;  // id điểm bán
        public string warehouse_name { get; set; }  // tên kho
        public string warehouse_code { get; set; }  // mã kho
        public string customer_code { get; set; }
        public double sale_cost { get; set; }  //tổng tiền giảm qua sản phẩm
        public int member_point { get; set; }  //số điểm member_point sử dụng
        public double member_point_value { get; set; }  //tổng tiền giảm qua member_point
        public double product_total_cost { get; set; }  //tổng tiền hàng
        public double voucher_cost { get; set; }  //tổng tiền giảm qua voucher
        public byte payment_type { get; set; }  //0 cash, 1 card, 2 internet banking,3 momo
        public double total_amount { get; set; }  //tổng tiền cần thanh toán = Tiền hàng + tiền ship + tiền lưu kho + tiền bảo hiểm - tiền discount voucher - tiền flashsale
    }
}
