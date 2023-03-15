using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.SaleSession
{
    public class Sales_SessionModel : IAuditableEntity
    {
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public long staff_id { get; set; }
        public long warehouse_id { get; set; }
        public string? note { get; set; }
        public string? warehouse_name { get; set; }
        public double opening_cash { get; set; } //tiền mặt lúc bắt đầu ca
        public double closing_cash { get; set; } // tiền mặt lúc kết thúc ca
        public double closing_total_proceeds { get; set; }// Tổng tiền thu trong ca (bằng tổng tiền theo đơn)
        public string staff_name { get; set; }
        public string code { get; set; } = "";

        public double closing_card { get; set; } = 0; // tiền nhận qua hình thức thanh toán bằng thẻ
        public double closing_online_transfer { get; set; } = 0;   // tiền nhận qua hình thức chuyển khoản ngân hàng
        public byte? status { get; set; } // 0 la dang hoat dong, 1 la da ket thuc

        //public long pos_id { get; set; } = 0; // id máy pos
    }
}
