using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "product_warehouse_id",
                table: "refund_order_product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "order_id",
                table: "refund_order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "customer_id",
                table: "refund",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_warehouse_id",
                table: "refund_order_product");

            migrationBuilder.DropColumn(
                name: "order_id",
                table: "refund_order");

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "refund");
        }
    }
}
