namespace Point_of_Sale.Model.Report
{
    public class Receipt_Cash_Form
    {
        public string ma_dvcs { get; set; } = "SmartGap";
        public string ma_gd { get; set; } 
        public string ma_kh { get; set; } 
        public string ong_ba { get; set; }
        public string dien_giai { get; set; } = "Bán hàng siêu thị";
        public DateTime ngay_ct { get; set; }
        public string ma_qs { get; set; }
        public string so_ct { get; set; }
        public string Tk { get; set; } = "13111";
        public string Tk_i { get; set; } = "5111";
        public double tien_tt { get; set; }
        public string dien_giaii { get; set; } = "";
        public string ma_vv_i { get; set; } 
        public long warehouse_id { get; set; }
        public long id { get; set; }
    }
}
