using App.Common;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace App
{
    public partial class FormReadSMS : Form
    {
        public FormReadSMS()
        {
            InitializeComponent();
        }
        public static ExtractSMS extractSMS = new ExtractSMS();
        static void ReadSMS(object str)
        {
            while (true)
            {
                SerialPort serialPort = new SerialPort();
                try
                {
                    serialPort = extractSMS.OpenPort(str.ToString(), Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                    Console.WriteLine("chay vao doc sms tren port {0} roi", str);
                    var list_sms = extractSMS.ReadUnReadMesss(serialPort);
                    if (list_sms.Count > 0)
                    {
                        foreach (var sms in list_sms)
                        {
                            sms.phone_receive = str.ToString();
                        }
                        if (true)
                        {
                            string ServiceUrl = "https://localhost:7067/api/";
                            string resourcePath = "SMS/create-list-sms-receive";
                            var body = JsonConvert.SerializeObject(list_sms);
                            var client = new RestClient(ServiceUrl);
                            var request = new RestRequest(resourcePath, Method.Post);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("Accept", "application/json");
                            request.AddJsonBody(body);
                            var response = client.Execute(request);
                            Console.WriteLine(response);
                        }

                    }
                    extractSMS.ClosePort(serialPort);
                }
                catch (Exception ex)
                {
                    extractSMS.ClosePort(serialPort);
                }
            }
        }
        private void FromReadSMS_Load(object sender, EventArgs e)
        {
            String com1 = "COM85";
            String com2 = "COM86";
            Thread thread1 = new Thread(ReadSMS);
            Thread thread2 = new Thread(ReadSMS);
            thread1.Start(com1);
            thread2.Start(com2);


        }
    }
}