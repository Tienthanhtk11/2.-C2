using App.Entity;
using App.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace App.Common
{
    public class ExtractSMS
    {
        public static AutoResetEvent receiveNow;
        private Telco telco = new Telco();
        #region Open and Close Ports
        //Open Port
        public SerialPort OpenPort(string p_strPortName, int p_uBaudRate, int p_uDataBits, int p_uReadTimeout, int p_uWriteTimeout)
        {
            receiveNow = new AutoResetEvent(false);
            SerialPort port = new SerialPort();
            try
            {
                port.PortName = p_strPortName;                 //COM1
                port.BaudRate = p_uBaudRate;                   //9600
                port.DataBits = p_uDataBits;                   //8
                port.StopBits = StopBits.One;                  //1
                port.Parity = Parity.None;                     //None
                port.ReadTimeout = p_uReadTimeout;             //300
                port.WriteTimeout = p_uWriteTimeout;           //300
                port.Encoding = Encoding.GetEncoding("iso-8859-1");
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                port.Open();
                port.DtrEnable = true;
                port.RtsEnable = true;
            }

            catch (Exception ex)
            {
                return null;
            }
            return port;
        }

        //Close Port
        public void ClosePort(SerialPort port)
        {
            try
            {
                port.Close();
                port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                port = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //Execute AT Command
        public string ExecCommand(SerialPort port, string command, int responseTimeout, string errorMessage)
        {
            try
            {
                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                receiveNow.Reset();
                port.Write(command + "\r");
                string input = ReadResponse(port, responseTimeout);
                if ((input.Length == 0) || ((!input.EndsWith("\r\n> ")) && (!input.EndsWith("\r\nOK\r\n"))))
                    throw new ApplicationException("No success message was received.");
                return input;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Receive data from port
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                {
                    receiveNow.Set();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ReadResponse(SerialPort port, int timeout)
        {
            string buffer = string.Empty;
            try
            {
                do
                {
                    //bool check = GsmSetting.receiveNow.WaitOne(timeout, false);

                    var check = receiveNow.WaitOne(timeout, false);
                    if (check)
                    {
                        Thread.Sleep(500);
                        string t = port.ReadExisting();
                        buffer += t;
                        //receiveNow.Reset();
                    }
                    else
                    {
                        if (buffer.Length > 0)
                            throw new ApplicationException("Response received is incomplete.");
                        else
                            throw new ApplicationException("No data received from phone.");
                    }
                }
                while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\n> ") && !buffer.EndsWith("\r\nERROR\r\n"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;
        }

        #region Count SMS
        public int CountSMSmessages(SerialPort port)
        {
            int CountTotalMessages = 0;
            try
            {

                #region Execute Command

                string recievedData = ExecCommand(port, "AT", 300, "No phone connected at ");
                recievedData = ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                String command = "AT+CPMS?";
                recievedData = ExecCommand(port, command, 1000, "Failed to count SMS message");
                int uReceivedDataLength = recievedData.Length;

                #endregion

                #region If command is executed successfully
                if ((recievedData.Length >= 45) && (recievedData.StartsWith("AT+CPMS?")))
                {

                    #region Parsing SMS
                    string[] strSplit = recievedData.Split(',');
                    string strMessageStorageArea1 = strSplit[0];     //SM
                    string strMessageExist1 = strSplit[1];           //Msgs exist in SM
                    #endregion

                    #region Count Total Number of SMS In SIM
                    CountTotalMessages = Convert.ToInt32(strMessageExist1);
                    #endregion

                }
                #endregion

                #region If command is not executed successfully
                else if (recievedData.Contains("ERROR"))
                {

                    #region Error in Counting total number of SMS
                    string recievedError = recievedData;
                    recievedError = recievedError.Trim();
                    recievedData = "Following error occured while counting the message" + recievedError;
                    #endregion

                }
                #endregion

                return CountTotalMessages;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Read SMS

        public ShortMessageCollection ReadSMS(SerialPort port, string p_strCommand)
        {

            // Set up the phone and read the messages
            ShortMessageCollection messages = null;
            try
            {

                #region Execute Command
                // Check connection
                ExecCommand(port, "AT", 300, "No phone connected");
                // Use message format "Text mode"
                ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                // Use character set "PCCP437"
                ExecCommand(port, "AT+CSCS=\"PCCP437\"", 300, "Failed to set character set.");
                // Select SIM storage
                ExecCommand(port, "AT+CPMS=\"SM\"", 300, "Failed to select message storage.");
                // Read the messages
                string input = ExecCommand(port, p_strCommand, 5000, "Failed to read the messages.");
                #endregion

                #region Parse messages
                messages = ParseMessages(input);
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (messages != null)
                return messages;
            else
                return null;

        }
        public ShortMessageCollection ParseMessages(string input)
        {
            ShortMessageCollection messages = new ShortMessageCollection();
            try
            {
                Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)\r\n");
                Match m = r.Match(input);
                while (m.Success)
                {
                    ShortMessage msg = new ShortMessage();
                    //msg.Index = int.Parse(m.Groups[1].Value);
                    msg.Index = m.Groups[1].Value;
                    msg.Status = m.Groups[2].Value;
                    msg.Sender = m.Groups[3].Value;
                    msg.Alphabet = m.Groups[4].Value;
                    msg.Sent = m.Groups[5].Value;
                    msg.Message = m.Groups[6].Value;
                    messages.Add(msg);

                    m = m.NextMatch();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return messages;
        }

        #endregion

        public bool CheckPortSendSMS(SerialPort port)
        {
            bool isSend = false;
            try
            {

                //this.port = OpenPort(strPortName,strBaudRate);
                string recievedData = ExecCommand(port, "AT", 300, "No phone connected");

                if (recievedData.EndsWith("\r\nOK\r\n"))
                {
                    isSend = true;
                }
                else if (recievedData.Contains("ERROR"))
                {
                    isSend = false;
                }
                return isSend;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            finally
            {
                if (port != null)
                {
                    //port.Close();
                    //port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                    //port = null;
                }
            }
            return false;
        }


        #region Send SMS

        static AutoResetEvent readNow = new AutoResetEvent(false);

        public string NapTienVaoSim(SerialPort port, string mathe)
        {
            try
            {
                // string recievedData = ExecCommand(port, "AT", 300, "No phone connected");
                //String command = "AT+CMGS=\"" + PhoneNo + "\"";
                string command = "AT+CUSD=1,\"*100*" + mathe + "#\",15";
                string recievedData = ExecCommand(port, command, 5000, "Failed to send message"); //3 seconds
                return recievedData;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return null;
        }

        public string CheckMoney(SerialPort port)
        {
            //dùng command để lấy số dư tài khoản
            //string command2 = "AT + COPS =? ";
            //string recievedData2 = ExecCommand(port, command2, 5000, "Failed to send message"); //3 seconds
            Thread.Sleep(1000);
            string command = "AT+CUSD=1,\"*101#\",15";
            string recievedData = ExecCommand(port, command, 5000, "Failed to send message"); //3 seconds
            return recievedData;
        }
        public string GetSimNetWork(SerialPort port)
        {
            string command2 = "AT+COPS?";
            string recievedData = ExecCommand(port, command2, 5000, "Failed to send message"); //3 seconds
            if (recievedData.Contains("VINAPHONE"))
            {
                return "Vinaphone";
            }
            if (recievedData.Contains("Viettel"))
            {
                return "Viettel";
            }
            if (recievedData.Contains("Mobifone"))
            {
                return "Mobifone";
            }
            else
                return "UNKNOW";
        }
        //AT+CSCA="+84900000022",145

        public bool sendMsg2(SerialPort serialPort, string PhoneNo, string Message, int timeout)
        {
            try
            {
                serialPort.WriteLine(@"AT" + (char)(13));
                Thread.Sleep(200);
                serialPort.WriteLine("AT+CMGF=1" + (char)(13));
                Thread.Sleep(200);
                serialPort.WriteLine(@"AT+CMGS=""" + PhoneNo + @"""" + (char)(13));
                Thread.Sleep(200);
                serialPort.WriteLine(Message + (char)(26));

                return true;
            }
            catch (Exception ex)
            {
                //throw ex;
                Debug.Write(ex.Message);
                return false;
            }
        }
        public bool sendMsg(SerialPort port, string PhoneNo, string Message, int timeout)
        {
            bool isSend = false;
            try
            {
                string recievedData = ExecCommand(port, "AT", 300, "No phone connected");
                Console.WriteLine(recievedData);

                recievedData = ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                Console.WriteLine(recievedData);

                Thread.Sleep(1000);

                string command = "AT+CMGS=\"" + PhoneNo + "\"";
                Console.WriteLine(command);

                recievedData = ExecCommand(port, command, 300, "Failed to accept phoneNo");
                Console.WriteLine(recievedData);

                Thread.Sleep(1000);

                command = Message + char.ConvertFromUtf32(26) + "\r";
                Console.WriteLine(command);

                recievedData = ExecCommand(port, command, timeout, "Failed to send message"); //3 seconds
                Console.WriteLine(recievedData);
                if (recievedData.EndsWith("\r\nOK\r\n"))
                {
                    isSend = true;
                }
                else if (recievedData.Contains("ERROR"))
                {
                    isSend = false;
                }
                return isSend;
            }
            catch (Exception ex)
            {
                //throw ex;
                Debug.Write(ex.Message);
                return isSend;
            }
        }

        public bool sendMsgAll(List<GsmDevice> listGsmDevice, string PhoneNo, string Message, int timeout)
        {
            //
            bool isSend = false;
            //Get telco
            string telcoPhone = telco.GetTelco(telco.Format_Phone(PhoneNo));
            //Get random port
            Random rnd = new Random();
            var listPortFilter = listGsmDevice.Where(o => o.Telco.ToLower() == telcoPhone.ToLower());
            if (listPortFilter != null && listPortFilter.Count() > 0)
            {
                int r = rnd.Next(listPortFilter.Count());
                var port = listPortFilter.ToArray()[r];
                SerialPort serialPort = OpenPort(listPortFilter.ToArray()[r].Name, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                //bool isConnect = CheckPortSendSMS(serialPort);
                if (serialPort.IsOpen)
                {
                    try
                    {
                        string recievedData = ExecCommand(serialPort, "AT", 300, "No phone connected");
                        recievedData = ExecCommand(serialPort, "AT+CMGF=1", 300, "Failed to set message format.");
                        String command = "AT+CMGS=\"" + PhoneNo + "\"";
                        recievedData = ExecCommand(serialPort, command, 300, "Failed to accept phoneNo");
                        command = Message + char.ConvertFromUtf32(26) + "\r";
                        recievedData = ExecCommand(serialPort, command, timeout, "Failed to send message"); //3 seconds
                        if (recievedData.EndsWith("\r\nOK\r\n"))
                        {
                            isSend = true;
                        }
                        else if (recievedData.Contains("ERROR"))
                        {
                            isSend = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                        Debug.Write(ex.Message);
                        isSend = false;

                    }
                }
                else
                {
                    isSend = false;
                }
                serialPort.Close();
            }
            else
            { //Get random port

                int r = rnd.Next(listGsmDevice.Count());
                var port = listGsmDevice.ToArray()[r];
                SerialPort serialPort = OpenPort(listGsmDevice.ToArray()[r].Name, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                //bool isConnect = CheckPortSendSMS(serialPort);
                if (serialPort.IsOpen)
                {
                    try
                    {
                        string recievedData = ExecCommand(serialPort, "AT", 300, "No phone connected");
                        recievedData = ExecCommand(serialPort, "AT+CMGF=1", 300, "Failed to set message format.");
                        String command = "AT+CMGS=\"" + PhoneNo + "\"";
                        recievedData = ExecCommand(serialPort, command, 300, "Failed to accept phoneNo");
                        command = Message + char.ConvertFromUtf32(26) + "\r";
                        recievedData = ExecCommand(serialPort, command, timeout, "Failed to send message"); //3 seconds
                        if (recievedData.EndsWith("\r\nOK\r\n"))
                        {
                            isSend = true;
                        }
                        else if (recievedData.Contains("ERROR"))
                        {
                            isSend = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                        Debug.Write(ex.Message);
                        isSend = false;
                    }
                }
                else
                {
                    isSend = false;
                }
                serialPort.Close();

            }

            return isSend;
        }
        static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                    readNow.Set();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Message_Receive> ReadSMS(SerialPort port)
        {
            Console.WriteLine("Reading..");
            port.WriteLine("AT+CMGF=1"); // Set mode to Text(1) or PDU(0)
            Thread.Sleep(1000); // Give a second to write
            port.WriteLine("AT+CPMS=\"SM\""); // Set storage to SIM(SM)
            Thread.Sleep(1000);
            port.WriteLine("AT+CMGL=\"ALL\""); // What category to read ALL, REC READ, or REC UNREAD
            Thread.Sleep(1000);
            port.Write("\r");
            Thread.Sleep(1000);
            string response = port.ReadExisting();
            string[] mess_trim = response.Replace('\r', ' ').Replace("OK", "").TrimEnd().Split('\n');
            List<Message_Receive> messages = new List<Message_Receive>();
            Message_Receive mess = new Message_Receive();
            foreach (var item in mess_trim)
            {
                if (item != " " && item != "" && !item.Contains("+CPMS"))
                {

                    if (item.Contains("+CMGL") /*&& !item.Contains("REC READ")*/)
                    {
                        if (mess.message != null && mess.message != "")
                        {
                            messages.Add(mess);
                            mess = new Message_Receive();
                        }
                        string[] mess_raw = item.Split(',');
                        mess.status = mess_raw[1].Replace('"', ' ');
                        mess.phone_send = mess_raw[2].Replace('"', ' ');
                        mess.date_receive = mess_raw[4].Replace('"', ' ');
                    }
                    else
                        mess.message = mess.message + '\n' + item;
                }
            }
            messages.Add(mess);
            return messages;

        }
        public List<Message_Receive> ReadUnReadMesss(SerialPort port)
        {
            Console.WriteLine("Reading..");
            port.WriteLine("AT+CMGF=1"); // Set mode to Text(1) or PDU(0)
            Thread.Sleep(1000);
            port.WriteLine("AT+CPMS=\"SM\""); // Set storage to SIM(SM)
            Thread.Sleep(1000);
            port.WriteLine("AT+CMGL=\"ALL\""); // What category to read ALL, REC READ, or REC UNREAD
            port.Write("\r");
            Thread.Sleep(1000);
            string response = port.ReadExisting();
            Console.WriteLine(response);

            string xxx = port.ReadExisting();
            Console.WriteLine(xxx);
            string[] mess_trim = response.Replace('\r', ' ').Replace("OK", "").TrimEnd().Split('\n');
            List<Message_Receive> messages = new List<Message_Receive>();
            Message_Receive mess = new Message_Receive();
            foreach (var item in mess_trim)
            {
                if (item != " " && item != "" && !item.Contains("+CPMS"))
                {

                    if (item.StartsWith("+CMGL"))
                    {
                        if (mess.message != null && mess.message != "")
                        {
                            messages.Add(mess);
                            mess = new Message_Receive();
                        }
                        string[] mess_raw = item.Split(',');
                        mess.status = mess_raw[1].Replace('"', ' ');
                        mess.phone_send = mess_raw[2].Replace('"', ' ');
                        mess.date_receive = mess_raw[4].Replace('"', ' ');
                    }
                    else
                        mess.message = mess.message + '\n' + item;
                }
            }
            Thread.Sleep(2000);
            messages.Add(mess);
            messages = messages.Where(x => x.phone_send != "" && x.message != "" && !x.status.Contains("REC READ")).ToList();
            DeleteMsg(port);
            return messages;
        }
        #endregion

        public void DeleteMsg(SerialPort port)
        {
            #region Execute Command
            string recievedData = ExecCommand(port, "AT", 300, "No phone connected");
            recievedData = ExecCommand(port, "AT+CMGF=1", 300, "No phone connected");
            recievedData = ExecCommand(port, "AT+CMGD=1,1", 300, "No phone connected");
            Console.WriteLine(recievedData);
        }

        #endregion
    }
}