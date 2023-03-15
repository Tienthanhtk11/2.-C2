namespace Point_of_Sale.Model.Promotion
{
    public class PromotionSearch: SearchBase
    {
        public int? type { get; set; }// 0: tặng kèm, 1: giảm giá
    }
}
