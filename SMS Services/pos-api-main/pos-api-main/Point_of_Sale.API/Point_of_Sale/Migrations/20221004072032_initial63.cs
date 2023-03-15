using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial63 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "partner_id",
               table: "product",
               type: "bigint",
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
              name: "partner_id",
              table: "product");
        }
    }
}
