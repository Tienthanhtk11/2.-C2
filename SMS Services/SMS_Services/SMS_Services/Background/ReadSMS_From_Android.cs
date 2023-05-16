using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SMS_Services.Common;
using SMS_Services.Entity;
using SMS_Services.Model;
using System.IO.Ports;
using System.Net.WebSockets;
using static SMS_Services.Controllers.BaseController;

namespace SMS_Services.Background
{
    public class ReadSMS_From_Android : IHostedService, IDisposable
    {
        private readonly ILogger<SendSMS_Cronjob_Services> _logger;
        private Timer? _timer = null;
        private readonly ApplicationDbContext _context;
        private readonly AndroidDBContext _Androidcontext;

        public ReadSMS_From_Android(ILogger<SendSMS_Cronjob_Services> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _context = factory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _Androidcontext = factory.CreateScope().ServiceProvider.GetRequiredService<AndroidDBContext>();
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        private async void DoWorkAsync(object? state)
        {
            try
            {
                List<sms> list_sm = new();
                List<Message_Receive> new_sms_receive = new();
                var receive_sms_android = _context.Message_Receive.Where(x => x.android_sms_id != null).OrderByDescending(x => x.android_sms_id).FirstOrDefault();
                if (receive_sms_android != null)
                {
                    list_sm = _Androidcontext.sms.Where(x => x.id > receive_sms_android.android_sms_id).ToList();
                }
                else
                    list_sm = _Androidcontext.sms.ToList();
                if (list_sm.Count>0)
                {
                    foreach (var item in list_sm)
                    {
                        if (item.text!="")
                        {
                            sms1 sms_content = JsonConvert.DeserializeObject<sms1>(item.text ?? "");
                            Message_Receive receive_sms = new()
                            {
                                port_name = "Android",
                                phone_send = sms_content.title,
                                android_sms_id = item.id,
                                phone_receive = "Android",
                                status = "Success",
                                computer_name = "Android",
                                date_receive = item.created_date.ToString() ?? DateTime.Now.ToString(),
                                message = sms_content.text
                            };
                            new_sms_receive.Add(receive_sms);
                        }
                       
                    }
                    _context.Message_Receive.AddRange(new_sms_receive);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }


    }
    public class sms1
    {
        public string icon { get; set; }
        public string iconLarge { get; set; }
        public string image { get; set; }
        public string imageBackgroundURI { get; set; }
        public string subText { get; set; }
        public string summaryText { get; set; }
        public string text { get; set; }
        public string time { get; set; }
        public string title { get; set; }
        public string titleBig { get; set; }
    }
}
