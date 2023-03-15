using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("customer")]
    public class Customer : IAuditableEntity
    {
        public string? name { get; set; }
        public string? code  { get; set; } 
        public string? phone { get; set; }
        public int? province_code { get; set; }  // code tỉnh
        public string? email { get; set; }
        public string? address { set; get; } 
        public int count_purchases { get; set; } = 0;
        public int member_point { get; set; } = 0;
        public double total_price { get; set; } = 0;
    }
}
