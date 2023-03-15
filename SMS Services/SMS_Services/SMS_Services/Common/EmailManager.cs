using System.Net.Mail;
using System.Net.Mime;

namespace SMS_Services
{
    public class EmailManager
    {
        public EmailManager()
        {

        }
        private string m_HostName; // your email SMTP server  

        public EmailManager(string hostName)
        {
            m_HostName = hostName;
        }

        public void SendMail(EmailSendConfigure emailConfig, EmailContent content)
        {
            MailMessage msg = ConstructEmailMessage(emailConfig, content);
            Send(msg, emailConfig);
        }

        // Put the properties of the email including "to", "cc", "from", "subject" and "email body"  
        private MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)
        {
            MailMessage msg = new System.Net.Mail.MailMessage();
            foreach (string to in emailConfig.TOs)
            {
                if (!string.IsNullOrEmpty(to))
                {
                    msg.To.Add(to);
                }
            }
            if(emailConfig.CCs !=null)
            {
                foreach (string cc in emailConfig.CCs)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        msg.CC.Add(cc);
                    }
                }
            }


            msg.From = new System.Net.Mail.MailAddress(emailConfig.From,
                                       emailConfig.FromDisplayName,
                                       System.Text.Encoding.UTF8);
            msg.IsBodyHtml = content.IsHtml;
            msg.Body = content.Content;
            msg.Priority = emailConfig.Priority;
            msg.Subject = emailConfig.Subject;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            if (content.AttachFileName != null)
            {
                Attachment data = new Attachment(content.AttachFileName,
                                                 MediaTypeNames.Application.Zip);
                msg.Attachments.Add(data);
            }

            return msg;
        }

        //Send the email using the SMTP server  
        private void Send(MailMessage message, EmailSendConfigure emailConfig)
        {
            SmtpClient client = new()
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(
                                  emailConfig.ClientCredentialUserName,
                                  emailConfig.ClientCredentialPassword),
                Host = m_HostName,
                /* client.Port = 25; */ // this is critical
                Port = 587,  // this is critical
                EnableSsl = true,  // this is critical
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Log.Logging("Error in Send funciation: " + ex.ToString(), "ERROR");
            }
            message.Dispose();
        }


        public int SendEmail(string smtpServer, string message)
        {
            int result = -1;
            try
            {
                //Send teh High priority Email  
                EmailManager mailMan = new(smtpServer);

                EmailSendConfigure myConfig = new()
                {
                    // replace with your email userName  
                    ClientCredentialUserName = "trantin184@gmail.com",
                    // replace with your email account password
                    ClientCredentialPassword = "adtqwwtqhfbolztv",
                    TOs = new string[] { "truongtb1984@gmail.com" },

                    //myConfig.CCs = new string[] { "huong.pn1@cmctelecom.vn", "ttkd-online.cskh@cmctelecom.vn" };
                    From = "trantin184@gmail.com",
                    FromDisplayName = "HOTLINE",
                    Priority = System.Net.Mail.MailPriority.Normal,
                    Subject = "SMS EMAIL"
                };

                EmailContent myContent = new EmailContent();
                myContent.Content = message;
                myContent.IsHtml = true;
                mailMan.SendMail(myConfig, myContent);
                result = 1;
            }
            catch (Exception ex)
            {
                Log.Logging("Error in SendEmail funciation: " + ex.ToString(), "ERROR");
            }
            return result;

        }

    }

    public class EmailSendConfigure
    {
        public string[] TOs { get; set; }
        public string[] CCs { get; set; }
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string Subject { get; set; }
        public MailPriority Priority { get; set; }
        public string ClientCredentialUserName { get; set; }
        public string ClientCredentialPassword { get; set; }
        public EmailSendConfigure()
        {
        }
    }

    public class EmailContent
    {
        public bool IsHtml { get; set; }
        public string Content { get; set; }
        public string AttachFileName { get; set; }
    }

}
