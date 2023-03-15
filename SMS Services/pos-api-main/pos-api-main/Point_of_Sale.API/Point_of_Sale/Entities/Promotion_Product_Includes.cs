namespace Point_of_Sale.Entities
{
    public class Promotion_Product_Includes : IAuditableEntity
    {
        public long promotion_product_id { get; set; }
        public long promotion_product_item_id { get; set; }
    }
}
