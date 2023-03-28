using App.Common;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace App
{
    public partial class FromReadSMS : Form
    {
        public FromReadSMS()
        {
            InitializeComponent();
        }
        public static ExtractSMS extractSMS = new ExtractSMS();
        static void ReadSMS(object str)
        {
            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("chay vao doc sms tren port {0} roi, lan {1}", str, i);
                    Thread.Sleep(1000);
                }
                //SerialPort serialPort = new SerialPort();
                //bool connect_gsm = loadPort(str, serialPort);
                //if (connect_gsm)
                //{
                //    Console.WriteLine("chay vao doc sms tren port {0} roi", str);
                //    var list_sms = extractSMS.ReadUnReadMesss(serialPort);
                //    if (list_sms != null)
                //    {
                //        foreach (var sms in list_sms)
                //        { //bố sung thêm sdt nhận vào record trước khi lưu db
                //            Console.WriteLine(sms.message);
                //            sms.phone_receive = str.ToString();
                //        }
                //    }
                //    string ServiceUrl = "https://localhost:7067/api/";
                //    try
                //    {
                //        string resourcePath = "api/SMS/create-list-sms-receive";
                //        var body = JsonConvert.SerializeObject(list_sms);
                //        var client = new RestClient(ServiceUrl);
                //        var request = new RestRequest(resourcePath, Method.Post);
                //        request.AddHeader("Content-Type", "application/json");
                //        request.AddHeader("Accept", "application/json");
                //        request.AddJsonBody(body);
                //        var response = client.Execute(request);
                //        //var response_token = JsonConvert.DeserializeObject<ResponseSingleContentModel<CustomerTokenModel>>(response.Content ?? "");
                //        //if (DateTime.Now >= response_token.Data.license_exp)
                //        //{
                //        //    MessageBox.Show("License của bạn đã hết hạn, vui lòng liên hệ admin để được gia hạn!");
                //        //}
                //        //else
                //        //{
                //        //    MessageBox.Show("Đăng nhập thành công, license của bạn còn: " + (int)((response_token.Data.license_exp - DateTime.Now).TotalDays) + " ngày!");
                //        //    FormMain form = new FormMain();
                //        //    this.Hide();
                //        //    form.ShowDialog();
                //        //    this.Close();
                //        //}
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show("Không gửi được data lên server /n" + ex.Message);
                //    }
                //}
            }
        }
        public static bool loadPort(object com_port, SerialPort serialPort)
        {
            try
            {
                string port_name = com_port.ToString();
                serialPort = extractSMS.OpenPort(port_name, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private void FromReadSMS_Load(object sender, EventArgs e)
        {
            String com1 = "COM1";
            String com2 = "COM3";
            Thread thread1 = new Thread(ReadSMS);
            Thread thread2 = new Thread(ReadSMS);
            thread1.Start(com1);
            thread2.Start(com2);

           
        }
    }
}

