using App.Entity;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace App
{
    public class Database : DbContext
    {
        // Your context has been configured to use a 'Database' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'App.Database' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Database' 
        // connection string in the application configuration file.
        public Database()
            : base("name=Database")
        {
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.


        public virtual DbSet<Message_Receive> Message_Receive { set; get; }
        public virtual DbSet<Message_Request> Message_Request { set; get; }
        public virtual DbSet<Send_SMS_History> Send_SMS_History { set; get; }
        public virtual DbSet<Port> Port { set; get; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}