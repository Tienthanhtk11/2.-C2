using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS_Services.Migrations
{
    public partial class initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "computer_name",
                table: "Message_Receive",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "port_name",
                table: "Message_Receive",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "computer_name",
                table: "Message_Receive");

            migrationBuilder.DropColumn(
                name: "port_name",
                table: "Message_Receive");
        }
    }
}
