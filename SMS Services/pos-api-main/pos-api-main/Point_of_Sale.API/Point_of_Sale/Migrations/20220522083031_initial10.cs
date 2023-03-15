using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_packing_id",
                table: "order_detail");

            migrationBuilder.DropColumn(
                name: "product_unit_id",
                table: "order_detail");

            migrationBuilder.AddColumn<string>(
                name: "packing_code",
                table: "order_detail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit_code",
                table: "order_detail",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "packing_code",
                table: "order_detail");

            migrationBuilder.DropColumn(
                name: "unit_code",
                table: "order_detail");

            migrationBuilder.AddColumn<long>(
                name: "product_packing_id",
                table: "order_detail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "product_unit_id",
                table: "order_detail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
