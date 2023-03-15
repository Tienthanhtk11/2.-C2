using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("voucher")]
    public class Voucher : IAuditableEntity
    {
        public string name { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int reduction_rate { get; set; }  // tỉ lệ giảm
        public double reduction_price { get; set; }  // số lượng giảm
        public double maximum_reduction { get; set; }  // số lượng giảm tối đa
        public int used_quantity { get; set; } = 0; // số lượng đã sử dụng
        public byte type { get; set; } // Loại voucher : 0: voucher giảm giá theo %, 1 voucher giảm giá fix giá trị
        public byte status_id { get; set; } // trạng thái : 0: đang xét duyêt, 1  đang áp dụng, 2 đã từ chối
        public DateTime active_date { get; set; }    // ngày bắt đầu có hiệu lực
        public DateTime end_date { get; set; }   // ngày kết thúc hiệu lực

    }
}
