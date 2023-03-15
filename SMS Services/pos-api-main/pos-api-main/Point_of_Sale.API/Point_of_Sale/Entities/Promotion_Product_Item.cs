namespace Point_of_Sale.Entities
{
    public class Promotion_Product_Item : IAuditableEntity
    {
        public long promotion_id { get; set; } 
        public long product_id { get; set; } 
        public double quantity { get; set; }//số lượng km
        public double quantity_sold { set; get; } = 0;// số lượng đã bán
        public double quantity_max { get; set; }//số lượng km tối đa 
    }
}
 