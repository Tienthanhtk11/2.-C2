namespace SMS_Services.Model
{
    public class Order: IAuditableEntity
    {
        public string code { get; set; } = "";
        public string note { get; set; } = "";
        public long customer_id { get; set; } = 0;
        public int count_sms { get; set; } = 0;
        public int status { get; set; } = 0;
        public DateTime timeSend { get; set; }
        public List<OrderDetails>? details { get; set; } = null;
    }
}
