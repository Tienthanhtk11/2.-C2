using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("bank_account")]
    public class BankAccount : IAuditableEntity
    {
        public string name { get; set; } = string.Empty;
        public string account_number { get; set; } = string.Empty;
        public string? note { get; set; } = string.Empty;
    }
}
