using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("sale_session")]

    public class Sales_Session : IAuditableEntity
    {
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public long staff_id { get; set; }
        public long warehouse_id { get; set; } = 0;
        public double opening_cash { get; set; }    
        public double closing_cash { get; set; } = 0;
        public double closing_card { get; set; } = 0;
        public double closing_online_transfer { get; set; } = 0;
        public string? note { get; set; }
        public string code { get; set; } = "";
        public byte status { get; set; } = 0;// 0 la dang hoat dong, 1 la da ket thuc

    }
}
