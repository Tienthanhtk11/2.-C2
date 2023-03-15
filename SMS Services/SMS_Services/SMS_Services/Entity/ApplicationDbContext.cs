using Microsoft.EntityFrameworkCore;
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

    }
}
