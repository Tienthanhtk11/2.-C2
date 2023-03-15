using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantity_stock",
                table: "product_warehouse_history",
                newName: "quantity_in_stock");

            migrationBuilder.RenameColumn(
                name: "quantity_sold",
                table: "product_warehouse_history",
                newName: "quantity");

            migrationBuilder.AddColumn<long>(
                name: "product_warehouse_id",
                table: "product_warehouse_history",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_warehouse_id",
                table: "product_warehouse_history");

            migrationBuilder.RenameColumn(
                name: "quantity_in_stock",
                table: "product_warehouse_history",
                newName: "quantity_stock");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "product_warehouse_history",
                newName: "quantity_sold");
        }
    }
}
