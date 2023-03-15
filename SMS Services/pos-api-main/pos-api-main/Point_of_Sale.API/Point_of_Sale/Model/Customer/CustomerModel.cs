using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Customer
{
    public class CustomerModel : IAuditableEntity
    {
        public string? code  { get; set; } 
        public string? name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public int? province_code { get; set; }  // code tỉnh
        public int member_point { get; set; } = 0;
        public string? address { set; get; }
        public int count_purchases { get; set; }
        public double total_price { get; set; }

    }
}
