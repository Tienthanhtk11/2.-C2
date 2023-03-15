using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("purchase")]
    public class Purchase : IAuditableEntity
    {
        public string code { set; get; }
        public string content { set; get; }
        public string note { set; get; }
        public long warehouse_id { set; get; }
        public byte status_id { set; get; }
        public long partner_id { set; get; }
        public DateTime transfer_date { set; get; }
        public long? warehouse_source_id { get; set; }
    }
}
