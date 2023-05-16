using Microsoft.EntityFrameworkCore;
using SMS_Services.Entity;

namespace SMS_Services.Model
{
    public class AndroidDBContext : DbContext
    {
        public AndroidDBContext(DbContextOptions<AndroidDBContext> options) : base(options)
        {
        }
        public virtual DbSet<sms> sms { set; get; }

    }
}