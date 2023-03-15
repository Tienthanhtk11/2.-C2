namespace Point_of_Sale.Entities
{
    public class Promotion_Warehouse : IAuditableEntity
    {
        public long promotion_id { get; set; }
        public long warehouse_id { get; set; }
    }
}
