using Microsoft.EntityFrameworkCore;
using SMS_Services.Entity;

namespace SMS_Services.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Order> Order { set; get; }
        public virtual DbSet<Port> Port { set; get; }
        public virtual DbSet<OrderDetails> OrderDetails { set; get; }
        public virtual DbSet<SendSMSHistory> SendSMSHistory { set; get; }
        public virtual DbSet<Message_Receive> Message_Receive { set; get; }
        public virtual DbSet<Admin_User> Admin_User { set; get; }
        public virtual DbSet<Customer> Customer { set; get; }
        public virtual DbSet<Config_Port> Config_Port { set; get; }
        public virtual DbSet<SMS_Template> SMS_Template { set; get; }
        public virtual DbSet<SMS_Request_Customer> SMS_Request_Customer { set; get; }

    }
}
