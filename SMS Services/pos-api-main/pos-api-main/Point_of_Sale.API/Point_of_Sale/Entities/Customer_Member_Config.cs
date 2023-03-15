using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("customer_member_config")]

    public class Customer_Member_Config : IAuditableEntity
    {
        public DateTime end_date { get; set; }  // thoi gian bat dau ap dung
        public DateTime start_date { get; set; } // thoi gian ket thuc ap dung
        public double min_apply_value { get; set; } // gia tri don hang toi thieu duoc ap dung
        public double ratio_point { get; set; } // ti le % duoc tinh tich diem, don vi la 1000d, vd: 1% cua 100k la 1diem
        public bool is_active { get; set; } = false;
        public int value_of_point {get; set; } // gia tri quy doi tuong duong cua 1 diem
        public bool is_total_amount { get; set; } // giá trị áp dụng là Tổng tiền thanh toán hay tổng tiền hàng. True là tổng tiền thanh toán

    }
}
