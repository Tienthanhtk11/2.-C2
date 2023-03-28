using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using System;
using System.Diagnostics;
using System.IO.Ports;

namespace App.Common
{
    public class GsmSetting
    {
        public static SerialPort serialPort;

        public static string commPort = "";
        public static int commBaudRate = 9600;
        public static int commTimeOut = 300;
        public static int dataBits = 8;
        public static int commDelay = 5;
        public static GsmCommMain comm;
        public static int smsToOK = 0;
        public static int smsToFail = 0;
        public static int smsOK = 0;
        public static int smsFail = 0;

       
        //lấy tên cổng COM từ 1 chuỗi
        public static string getPortName(string strPortName)
        {
            string portName = "";
            if (strPortName.IndexOf("-") > 0)
            {
                portName = strPortName.Substring(0, strPortName.IndexOf("-")).Trim();
            }
            return portName;
        }
        //Kiểm tra 1 chuỗi có phải là chuỗi Unicode
        public static bool IsUnicode(string message)
        {
            string text = "áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ";
            bool result;
            for (int i = 0; i < text.Length; i++)
            {
                bool flag = message.IndexOf(text[i]) >= 0;
                if (flag)
                {
                    result = true;
                    return result;
                }
            }
            result = false;
            return result;
        }
        //Chuyển chuỗi Unicode về chuỗi không dấu
        public static string RemoveUnicode(string strUnicode)
        {
            string result = strUnicode;
            string[] array = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };
            for (int i = 1; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    result = result.Replace(array[i][j], array[0][i - 1]);
                }
            }
            return result;
        }
        //Gửi nhiều tin nhắn cùng 1 lúc
        public static bool SendMultiple(OutgoingSmsPdu[] pdus)
        {
            int num = pdus.Length;
            try
            {
                // Gửi các tin nhắn được tạo
                int i = 0;
                foreach (OutgoingSmsPdu pdu in pdus)
                {
                    i++;
                    comm.SendMessage(pdu, true);
                    //Chờ 15 giây để modem phản hồi lại thông tin
                    Wait(15);

                }
                smsOK = smsOK + num;
                return true;
            }
            catch (Exception ex)
            {
              
                ExtractSMS extract = new ExtractSMS();
                extract.ClosePort(GsmSetting.serialPort);
                //Disconnect(GsmSetting.serialPort);
                //Application.DoEvents();
                //OpenPort();
                extract.OpenPort(GsmSetting.commPort, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                //Application.DoEvents();
                return false;
            }
        }

        public static OutgoingSmsPdu[] CreateConcatMessage(string message, string number, bool unicode)
        {
            OutgoingSmsPdu[] pdus = null;
            try
            {
                //Nếu không nhắn tin dạng unicode
                if (!unicode)
                {
                    //Nếu tin nhắn đầu vào là 1 chuỗi unicode thì thực hiện đưa về không dấu
                    if (IsUnicode(message))
                    {
                        message = RemoveUnicode(message);
                    }
                    //tạo các tin nhắn nối đối với nhưng tin trên 160 ký tự
                    pdus = SmartMessageFactory.CreateConcatTextMessage(message, number);

                }
                else
                {
                    //tạo các tin nhắn nối đối với nhưng tin trên 160 ký tự
                    pdus = SmartMessageFactory.CreateConcatTextMessage(message, true, number);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("(Tạo tin nhắn nối) Lỗi: " + ex.Message); 
                return null;
            }

            return pdus;
        }

        public static bool SendConcatSMS(string strMessage, string strNumber, bool Unicode)
        {
            //strNumber = InsertCode(strNumber);
            OutgoingSmsPdu[] pdus = CreateConcatMessage(strMessage, strNumber, Unicode);
            if (pdus != null)
            {
                return SendMultiple(pdus);
            }
            else
            {
                return false;
            }
        }

        //Chèn mã quốc gia cho số điện thoại cần gửi
        public static string InsertCode(string num)
        {
            //bien luu gia tri so dien thoai tam thoi
            string tempValue = num;// ConvertToPhoneNumber(num);
            tempValue = ReplaceFirstOccurrence(tempValue, "0", "+84");

            return tempValue;
        }
        //thay thế ký tự đầu tiên trong chuỗi
        public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {

            string result = Source;
            if (string.IsNullOrEmpty(result.Trim())) { return ""; }
            if (result.Substring(0, Find.Length) == Find && result.Length > 0)
            {
                result = result.Remove(0, Find.Length).Insert(0, Replace);
            }

            return result;
        }

        //Chuyển đổi 1 chuỗi sang chuẩn
        public static string ConvertToPhoneNumber(string num)
        {
            //bien luu gia tri so dien thoai tam thoi
            string tempValue = num.Trim();
            //thay the chuoi +84 thanh 0
            tempValue = ReplaceFirstOccurrence(tempValue, "+84", "0");
            //thay the chuoi 084 thanh 0
            tempValue = ReplaceFirstOccurrence(tempValue, "084", "0");
            //thay the ky tu + thanh rong
            tempValue = ReplaceFirstOccurrence(tempValue, "+", "");
            //thay the chuoi 84 thanh 0
            tempValue = ReplaceFirstOccurrence(tempValue, "84", "0");
            //thay the khoang trang thanh rong
            tempValue = tempValue.Replace(" ", "");
            //Chuyen doi so dien thoai ve chuan 10 so hoac 11 so de kiem tra tinh hop le cua so dien thoai
            return tempValue;
        }
        //kiểm tra số điện thoại có phải có phải là số hay không
        public static bool IsNumeric(string strNumber)
        {
            double Num;
            bool isNum = double.TryParse(strNumber, out Num);

            if (isNum)
                return true;
            else
                return false;
        }

        public static bool CheckPhoneNumber(string phoneNumber)
        {
            bool flag = false;
            //bien luu gia tri so dien thoai tam thoi
            string tempValue = "";
            //bien luu 2 gia tri dau tien cua so dien thoai
            string firstNumber = "";
            //Thuc hien chuyen doi ve chuan 10 hoac 11 so
            tempValue = ConvertToPhoneNumber(phoneNumber);

            //neu gia tri bien tam khong rong
            if (!string.IsNullOrEmpty(tempValue))
            {
                //neu gia tri bien tam la so
                if (IsNumeric(tempValue) == true)
                {
                    firstNumber = tempValue.Substring(0, 2);
                    if (((firstNumber == "09" || firstNumber == "08" || firstNumber == "07" || firstNumber == "05" || firstNumber == "03") && tempValue.Length == 10))
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }
        public static void Wait(int second)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds <= (second * 1000))
            {
                //Application.DoEvents();
            }
            sw.Stop();
        }

    }
}
