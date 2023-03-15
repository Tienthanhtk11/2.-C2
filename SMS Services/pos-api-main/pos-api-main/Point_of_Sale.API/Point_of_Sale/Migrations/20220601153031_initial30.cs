using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_weigh",
                table: "warehouse_receipt_product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_weigh",
                table: "warehouse_receipt_product");
        }
    }
}
