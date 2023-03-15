using GsmComm.GsmCommunication;
using GsmComm.PduConverter;

namespace SMS_Services.Common
{
    class SmsReceive
    {
        public string Sender { get; set; }
        public string Command { get; set; }
        public string Store { get; set; }
        public static List<SmsReceive> GetCommandAllStorage(string Server, string Port, string Database, string Username, string Password)
        {
            List<SmsReceive> sr = new();
            //Lấy toàn bộ tin nhắn nhận được trong bộ nhớ modem
            foreach (SmsReceive sms in SmsReceive.GetCommand(PhoneStorageType.Phone, Server, Port, Database, Username, Password))
            {
                sr.Add(sms);
            }
            //lấy toàn bộ tin nhắn nhận được trong bộ nhớ sim
            foreach (SmsReceive sms in SmsReceive.GetCommand(PhoneStorageType.Sim, Server, Port, Database, Username, Password))
            {
                sr.Add(sms);
            }
            return sr;
        }
        public static List<SmsReceive> GetCommand(string Storage, string Server, string Port, string Database, string Username, string Password)
        {
            List<SmsReceive> sr = new();
            DecodedShortMessage[] messages = GsmSetting.comm.ReadMessages(PhoneMessageStatus.All, Storage);//PhoneStorageType.Phone);
            //string path = Application.StartupPath + "\\queryconfig.sms";
            //string path2 = Application.StartupPath + "\\query.sms";
            if (messages.Count() > 0)
            {
                foreach (DecodedShortMessage message in messages)
                {
                    SmsPdu pdu = message.Data;
                    bool numOK = false;
                    if (pdu is SmsDeliverPdu)
                    {
                        SmsDeliverPdu data = (SmsDeliverPdu)pdu;
                        SmsReceive srs = new SmsReceive();
                        //bóc tách số điện thoại và nội dung yêu cầu từ tin nhắn nhận được
                        string numPhone = data.OriginatingAddress.Trim();
                        string command = data.UserDataText.Trim().ToUpper();

                        //kiểm tra danh sách số điện thoại được phép yêu cầu
                        //if (File.Exists(path2))
                        //{
                        //    FileManager fm = new FileManager();
                        //    string[] ds;
                        //    ds = fm.readFile(path2);
                        //    if (ds != null)
                        //    {
                        //        foreach (string line in ds)
                        //        {
                        //            //if (message.Status == PhoneMessageStatus.ReceivedUnread){
                        //            if (GsmSetting.InsertCode(GsmSetting.ConvertToPhoneNumber(line.Trim())) ==
                        //                GsmSetting.InsertCode(GsmSetting.ConvertToPhoneNumber(numPhone)))
                        //            {
                        //                numOK = true;
                        //                //kiểm tra yêu cầu có trong danh sách lệnh không
                        //                if (File.Exists(path))
                        //                {
                        //                    FileManager fm2 = new FileManager();
                        //                    string[] ds2;
                        //                    ds2 = fm2.readFile(path);
                        //                    if (ds2 != null)
                        //                    {
                        //                        foreach (string line2 in ds2)
                        //                        {
                        //                            SMSCommand sc2 = fm2.parseCommand(line2);
                        //                            if (sc2.Command.Trim().ToUpper() == command)
                        //                            {
                        //                                SqlServer sq = new SqlServer();
                        //                                sq.Server = Server;
                        //                                sq.Port = Port;
                        //                                sq.Database = Database;
                        //                                sq.Username = Username;
                        //                                sq.Password = Password;
                        //                                if (sq.Connect())
                        //                                {
                        //                                    string Content = sq.executeStoreProcedureReturnOneRow(sc2.Store);
                        //                                    if (!string.IsNullOrEmpty(Content))
                        //                                    {
                        //                                        srs.Store = Content;
                        //                                        srs.Sender = numPhone;
                        //                                        srs.Command = command;
                        //                                        sr.Add(srs);
                        //                                    }
                        //                                }
                        //                                sq.Disconnect();
                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        //nếu số điện thoại yêu cầu thông tin không nằm trong danh sách cho phép thì từ chối trả lời
                        if (numOK == false)
                        {
                            string log = "(Tin nhắn ưu tiên) Từ chối trả lời số: " + numPhone + " vì không nằm trong danh sách được cấp phép tra cứu số liệu.";
                            //GsmSetting.WriteLog(log);
                        }
                    }
                }
            }
            //xóa toàn bộ tin nhắn nhận được để giải phóng bộ nhớ
            GsmSetting.comm.DeleteMessages(DeleteScope.All, Storage);

            return sr;
        }
    }
}
