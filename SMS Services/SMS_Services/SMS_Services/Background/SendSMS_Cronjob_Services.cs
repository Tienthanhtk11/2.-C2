using Microsoft.EntityFrameworkCore;
using SMS_Services.Common;
using SMS_Services.Model;
using System.IO.Ports;

namespace SMS_Services.Background
{
    public class SendSMS_Cronjob_Services : IHostedService, IDisposable
    {
        private readonly ILogger<SendSMS_Cronjob_Services> _logger;
        private Timer? _timer = null;
        private readonly ApplicationDbContext _context;
        SerialPort serialPort = new();
        ExtractSMS extractSMS = new();
        bool connect_gsm = false;

        public SendSMS_Cronjob_Services(ILogger<SendSMS_Cronjob_Services> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _context = factory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            return Task.CompletedTask;
        }
        public bool loadPort(string com_port)
        {
            try
            {
                serialPort = extractSMS.OpenPort(com_port, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private async void DoWorkAsync(object? state)
        {
            try
            {
                Port current_port = new();
                List<Port> ports = _context.Port.Where(x => !x.is_delete).ToList();
                foreach (var item in ports)
                {
                    if (item.cash > 300)
                    {
                        connect_gsm = loadPort(item.name);
                        if (connect_gsm)
                        {
                            current_port = item;
                            break;
                        }
                    }
                }
                var sms = await _context.OrderDetails.Where(x => x.status == 0).FirstOrDefaultAsync();
                if (sms != null)
                {
                    SendSMSHistory sms_history = new()
                    {
                        order_code = sms.order_code,
                        order_id = sms.order_id,
                        phone_receive = sms.phone_receive,
                        message = sms.message,
                        telco = sms.telco,
                        sum_sms = sms.sum_sms,
                        status = sms.status,
                        phone_send = current_port.phone_number
                    };

                    if (GsmSetting.CheckPhoneNumber(GsmSetting.ConvertToPhoneNumber(sms.phone_receive.Trim())) == true)
                    {
                        bool smsSent = extractSMS.sendMsg(this.serialPort, sms.phone_receive.Trim(), sms.message.Trim(), 500);
                        if (smsSent == false)
                        {
                            sms_history.system_response = "Gửi thành công";
                        }
                        else
                        {
                            sms_history.system_response = "Gửi thất bại";
                        }
                    }
                    else
                    {
                        sms_history.system_response = "SDT không đúng định dạng";
                    }
                    sms.status = 1;
                    current_port.cash -= 300;
                    _context.Port.Update(current_port);
                    _context.OrderDetails.Update(sms);
                    _context.SendSMSHistory.Add(sms_history);
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
}
