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
        public static string log = "";
        static void ReadSMS(object str)
        {
            while (true)
            {
                SerialPort serialPort = new SerialPort();
                try
                {

                    serialPort = extractSMS.OpenPort(str.ToString(), Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                    if (serialPort!= null)
                    {
                        var list_sms = extractSMS.ReadUnReadMesss(serialPort);
                        if (list_sms.Count > 0)
                        {
                            foreach (var sms in list_sms)
                            {
                                sms.phone_receive = str.ToString();
                                log = "new SMS from: " + str.ToString() + ", content: " + sms.message + " ,phone send: " + sms.phone_send + " , time: " + sms.date_receive + "\r\n" + log;
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
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
                catch (Exception ex)
                {
                    extractSMS.ClosePort(serialPort);
                }
            }
        }
        private void FromReadSMS_Load(object sender, EventArgs e)
        {
            String com1 = "COM37";
            String com2 = "COM40";
            Thread thread1 = new Thread(ReadSMS);
            Thread thread2 = new Thread(ReadSMS);
            timer1.Interval = 1000;
            timer1.Start();
            thread1.Start(com1);
            //thread2.Start(com2);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string input = log;
            Console.WriteLine(input);
            textBox1.Text = input;
            timer1.Interval = 5000;
            timer1.Start();
        }

    }
}