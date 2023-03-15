using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("customer_point_history")]

    public class Customer_Point_History : IAuditableEntity
    {
        public long order_id { get; set; }
        public long customer_id { get; set; }
        public int number_of_point { get; set; }
        public int value_of_point { get; set; }
        public int type { get; set; } = 0; // 0 la mua hang tich diem & su dung diem cho don hang, 1 la  hoan hang nen cong diem

    }
}
