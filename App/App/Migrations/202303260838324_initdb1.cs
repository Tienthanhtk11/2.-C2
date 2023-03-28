namespace App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdb1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message_Receive",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        phone_receive = c.String(),
                        phone_send = c.String(),
                        message = c.String(),
                        date_receive = c.String(),
                        status = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                        is_delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Message_Request",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        phone_receive = c.String(),
                        message = c.String(),
                        telco = c.String(),
                        sum_sms = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        dateAdded = c.DateTime(nullable: false),
                        is_delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Ports",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(),
                        phone_number = c.String(),
                        telco = c.String(),
                        cash = c.Int(nullable: false),
                        dateAdded = c.DateTime(nullable: false),
                        is_delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Send_SMS_History",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        message_request_id = c.Long(nullable: false),
                        phone_receive = c.String(),
                        phone_send = c.String(),
                        message = c.String(),
                        telco = c.String(),
                        sum_sms = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        system_response = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                        is_delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Send_SMS_History");
            DropTable("dbo.Ports");
            DropTable("dbo.Message_Request");
            DropTable("dbo.Message_Receive");
        }
    }
}
