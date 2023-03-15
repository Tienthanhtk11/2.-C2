using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{

    [Table("warehouse")]
    public class Warehouse : IAuditableEntity
    {
        public string name { get; set; }
        public long id_ecom { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public int? province_code { get; set; } // code vi tri kho
        public byte type { get; set; }// 0 kho huy, 1 kho that, 2 kho ao,
        public long id_come { get; set; }
        public string? address { get; set; }
        public long province_id { set; get; }
        public long district_id { set; get; }
        public long ward_id { set; get; }
        public bool is_active { get; set; } = true;
        public long parent_id { set; get; }
        public string search_name { set; get; } = "";

    }
}
