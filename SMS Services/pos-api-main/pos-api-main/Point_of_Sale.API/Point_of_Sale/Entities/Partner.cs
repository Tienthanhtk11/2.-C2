using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("partner")]
    public class Partner : IAuditableEntity
    {
        public long id_ecom { get; set; }
        public byte type { get; set; }// 0 doi tac thuong, 1: doi tac la smartgap, 2 doi tac ao kiem kho
        public string? name { get; set; }
        public string? code { get; set; }
        public int? province_code { get; set; }  // code tỉnh
        public string? phone {set; get; }
        public string? website { set; get; }
        public string? email { get; set; } = string.Empty;
        public string? taxcode { get; set; } = string.Empty;
        public string? introduce { get; set; } = string.Empty;
        public string search_name { set; get; } = "";

    }
}
