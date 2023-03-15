using EASendMail;

namespace SMS_Services
{
    public static class SendEmail
    {
        public static void SendMail(string message)
        {
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                // Your email address
                oMail.From = "ttkd-online.cskh@cmctelecom.vn";

                // Set recipient email address
                oMail.To = "kt.sms@cmctelecom.vn";
                //oMail.To = "tin.tv@cmctelecom.vn";

                // Set email subject
                oMail.Subject = "SMS-QC";

                // Set email body
                oMail.TextBody = message ;

                // Hotmail/Outlook SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.live.com");

                // If your account is office 365, please change to Office 365 SMTP server
                // SmtpServer oServer = new SmtpServer("smtp.office365.com");

                // User authentication should use your
                // email address as the user name.
                oServer.User = "ttkd-online.cskh@cmctelecom.vn";
                oServer.Password = "vas@cmc2017";

                // use 587 TLS port
                oServer.Port = 587;

                // detect SSL/TLS connection automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();
                oSmtp.SendMail(oServer, oMail);
                //Log.Logging("email was sent successfully!", "SendMail",_path);
            }
            catch (Exception ex)
            {
                //Code.Log("Exception Send mail : " + ex.Message + " - StackStrace: " + ex.StackTrace, "Exception");
            }

        }

    }
}
