using System.ComponentModel.DataAnnotations.Schema;

namespace SMS_Services.Model
{
    public class Customer : IAuditableEntity
    {
        public string user_name { get; set; }
        public string password { get; set; }
        public string passcode { get; set; } = "";
        public string name { get; set; } = "";
        public string email { get; set; } = "";
        public int cash { get; set; } = 0;
        public bool active { get; set; } = true;
        public DateTime license_exp { get; set; } = DateTime.Now.AddMonths(3);
        public DateTime last_active { get; set; } = DateTime.Now;
        public string license_key { get; set; } = string.Empty;
        [NotMapped]
        public string status { get; set; } = string.Empty;

    }
}
