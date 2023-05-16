using SMS_Services.Model;

namespace SMS_Services.Entity
{
    public class Phone: IAuditableEntity
    {
        public string phone_number { get; set; }
        public string telco { get; set; }
        public string cash { get; set; }
        public DateTime expired { get; set; } = DateTime.Now;
    }
}
