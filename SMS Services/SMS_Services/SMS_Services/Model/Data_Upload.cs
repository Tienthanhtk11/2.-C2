using SMS_Services.Entity;

namespace SMS_Services.Model
{
    public class SMS_Template_Model
    {
        public long customer_id { get; set; } = 0;
        public string message { get; set; }
        public long id { set; get; } = 0;
        public long userAdded { set; get; } = 0;
    }
    public class Phone_Number
    {
        public string telco { get; set; } = "";
        public string phone_number { get; set; } = "";
        public long customer_id { get; set; } = 0;
        public long id { set; get; } = 0;
        public long userAdded { set; get; } = 0;
    }
    public class Data_Upload
    {
        public List<Phone_Number> list_phone_number { get; set; } = new List<Phone_Number>();
        public List<SMS_Template> list_sms_template { get; set; } = new List<SMS_Template>();

    }
}
