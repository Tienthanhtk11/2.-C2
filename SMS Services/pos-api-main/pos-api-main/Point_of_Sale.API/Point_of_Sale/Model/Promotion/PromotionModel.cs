using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Promotion
{
    public class PromotionModel : IAuditableEntity
    {
        public string code { get; set; }
        public string name { get; set; }
        public string? note { get; set; }
        public DateTime? date_start { get; set; }
        public DateTime? date_end { get; set; }
        public int schedule_type { get; set; }  // 0: hàng ngày, 1: hàng tuần (chọn ngày trong tuần), 2: hàng tháng (chọn ngày trong tháng)
        public int type { get; set; } // 0: tặng kèm, 1: giảm giá
        public byte status { get; set; }
        public bool all_warehouse { get; set; }
        public string user_name { get; set; }
        public List<Promotion_Warehouse>? warehouses { get; set; }
        public List<PromotionProductIncludes>? productIncludes { get; set; }
        public List<Promotion_Schedule_Time>? scheduleTimes { get; set; }
        public List<Promotion_Schedule>? schedules { get; set; }
         
    }
}
