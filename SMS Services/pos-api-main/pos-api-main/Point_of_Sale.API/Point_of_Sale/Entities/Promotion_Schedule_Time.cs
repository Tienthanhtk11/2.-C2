namespace Point_of_Sale.Entities
{
    public class Promotion_Schedule_Time : IAuditableEntity
    {
        public long promotion_id { get; set; }
        public TimeSpan? time_start { get; set; }
        public TimeSpan? time_end { get; set; }
    }
}
