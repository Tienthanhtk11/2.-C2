using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("category_stalls")]

    public class Category_Stalls : IAuditableEntity // bảng danh mục quầy hàng 
    {
        [StringLength(20)]
        public string code { get; set; } = string.Empty;
        [StringLength(250)]
        public string name { set; get; } = string.Empty;
        [StringLength(1500)]
        public string note { set; get; } = string.Empty;
        public string search_name { set; get; } = string.Empty;
        public long category_id { get; set; }        // id ngành hàng
        public bool is_active { get; set; } = true;

    }
}
