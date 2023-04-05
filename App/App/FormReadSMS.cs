using App.Common;
using App.Entity;
using App.Model;
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
        public static long customer_id = 0;
        public static string ServiceUrl = "http://103.120.242.146:8088/api/";
        public static ExtractSMS extractSMS = new ExtractSMS();
        public static string log = "";
        static void ReadSMS(object str)
        {
            while (true)
            {
                Console.WriteLine("thread {0} str start", str);
                SerialPort serialPort = new SerialPort();
                try
                {
                    serialPort = extractSMS.OpenPort(str.ToString(), Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                    if (serialPort != null)
                    {
                        var list_sms = extractSMS.ReadUnReadMesss(serialPort);
                        if (list_sms.Count > 0)
                        {
                            foreach (var sms in list_sms)
                            {
                                sms.phone_receive = str.ToString();
                                sms.userAdded = customer_id;
                                log = "new SMS from: " + str.ToString() + ", content: " + sms.message + " ,phone send: " + sms.phone_send + " , time: " + sms.date_receive + "\r\n" + log;
                            }
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
                        extractSMS.ClosePort(serialPort);
                        Console.WriteLine("Scan xong port {0}", str);
                    }
                    else
                    {
                        Console.WriteLine("Tam dung thread {0} 60s", str);
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
            customer_id = long.Parse(label2.Text);
            for (int i = 0; i < 100; i++)
            {
                string port = "COM" + i;
                Thread thread = new Thread(ReadSMS);
                thread.IsBackground = true;
                thread.Start(port);
            }
            timer1.Interval = 2000;
            timer1.Start();
            timer2.Interval = 60000;
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string input = log;
            Console.WriteLine(input);
            textBox1.Text = input ;
            timer1.Interval = 5000;
            timer1.Start();
        }
        private void FormReadSMS_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        private void timer2_Tick_1(object sender, EventArgs e) 
        {
            string resourcePath = "customer/ping?customer_id=" + customer_id;
            var client = new RestClient(ServiceUrl);
            var request = new RestRequest(resourcePath, Method.Get);
            var x =client.Execute(request);
            timer2.Interval = 60000;
            timer2.Start();
        }
    }
}