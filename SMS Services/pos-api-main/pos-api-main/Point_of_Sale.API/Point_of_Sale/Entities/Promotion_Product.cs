namespace Point_of_Sale.Entities
{
    public class Promotion_Product : IAuditableEntity
    {
        public long promotion_id { get; set; } 
        public long product_id { get; set; } 
        public double quantity { get; set; } //số lượng sản phẩm đc km
        public double quantity_sold { set; get; } = 0;// số lượng đã bán 
        public double quantity_max { get; set; }//số lượng km tối đa
        public double price { get; set; }// giá bán gốc
        public double discount_price { get; set; }//giám giá tiền
        public double discount_rate { get; set; }//giảm theo %
    }
}
