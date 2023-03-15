using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Customer
{
    public class Customer_Point_History_Model : IAuditableEntity
    {
        public string order_code { get; set; }
        public long order_id { get; set; }
        public long customer_id { get; set; }
        public int number_of_point { get; set; }
        public int value_of_point { get; set; }
        public int type { get; set; } = 0; // 0 la mua hang tich diem & su dung diem cho don hang, 1 la  hoan hang nen cong diem

    }
}
