using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Promotion
{
    public class PromotionProductIncludes
    {
        public List<Promotion_Product>? products { get; set; }
        public List<Promotion_Product_Item>? product_items { get; set; }
    }
}
