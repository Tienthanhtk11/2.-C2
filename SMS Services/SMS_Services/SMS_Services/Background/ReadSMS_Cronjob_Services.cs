using SMS_Services.Common;
using SMS_Services.Model;
using System.IO.Ports;

namespace SMS_Services.Background
{
    public class ReadSMS_Cronjob_Services : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private readonly ApplicationDbContext _context;
        SerialPort serialPort = new();
        ExtractSMS extractSMS = new();
        bool connect_gsm = false;

        public ReadSMS_Cronjob_Services( IServiceScopeFactory factory)
        {
            _context = factory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
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
            {  //đọc ở tất cả các cổng
                List<Port> ports = _context.Port.Where(x => !x.is_delete).ToList();
                foreach (var item in ports)
                {
                    connect_gsm = loadPort(item.name);
                    if (connect_gsm)
                    {
                        Console.WriteLine("chay vao doc sms tren port {0} roi", item.name);
                        // lấy danh sách sms chưa đọc & lưu vào db
                        var list_sms = extractSMS.ReadUnReadMesss(this.serialPort);
                        if (list_sms != null)
                        {
                            foreach (var sms in list_sms)
                            { //bố sung thêm sdt nhận vào record trước khi lưu db
                                Console.WriteLine(  sms.message);
                                sms.phone_receive = item.phone_number;
                            }       
                            MessageCreate(list_sms);
                        }
                        //break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
        public void MessageCreate(List<Message_Receive> messages)
        {
            _context.Message_Receive.AddRange(messages);
            _context.SaveChanges();
        }
    }
}
