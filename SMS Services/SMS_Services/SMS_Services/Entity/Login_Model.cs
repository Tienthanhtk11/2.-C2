using Microsoft.AspNetCore.Mvc;

namespace SMS_Services.Model
{
    public class Login_Model
    {
    }
    public class UserTokenModel
    {
        public long id { get; set; }
        public string user_name { get; set; }
        public string token { get; set; }
        public string name { get; set; }
    }
 
    public class LoginModel
    {
        public string user_name { set; get; }
        public string password { set; get; }
    }
}
