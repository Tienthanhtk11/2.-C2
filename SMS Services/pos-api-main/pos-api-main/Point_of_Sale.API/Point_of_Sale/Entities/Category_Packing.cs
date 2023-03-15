using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("category_packing")]
    public class Category_Packing : IAuditableEntity
    {
        [StringLength(20)]
        public string code { get; set; } = string.Empty;
        [StringLength(250)]
        public string name { set; get; } = string.Empty;
        [StringLength(1500)]
        public string note { set; get; } = string.Empty;
        public int order { set; get; }
        public byte status_id { set; get; }
        public string search_name { get; set; } = string.Empty;

    }
}
