namespace SMS_Services.Model
{
    public class Port : IAuditableEntity
    {
        public string name { get; set; }
        public string phone_number { get; set; }
        public string telco { get; set; }
        public int cash { get; set; } = 0;
    }
}
