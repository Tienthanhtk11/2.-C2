using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "customer_id",
                table: "warehouse_export",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "warehouse_destination_id",
                table: "warehouse_export",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "warehouse_export");

            migrationBuilder.DropColumn(
                name: "warehouse_destination_id",
                table: "warehouse_export");
        }
    }
}
