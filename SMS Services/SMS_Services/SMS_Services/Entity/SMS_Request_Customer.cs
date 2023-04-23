using SMS_Services.Model;

namespace SMS_Services.Entity
{
    public class SMS_Request_Customer : IAuditableEntity
    {
        public long customer_id { get; set; }
        public long template_id { get; set; } = 0;
        public string phone_receive { get; set; }
        public string message { get; set; }
        public string telco { get; set; } = "";
        public int sum_sms { get; set; } = 1;
        public int status { get; set; } = 0;
    }
}
