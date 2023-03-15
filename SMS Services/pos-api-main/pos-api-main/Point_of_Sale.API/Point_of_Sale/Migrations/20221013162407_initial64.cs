using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_promotion",
                table: "warehouse_receipt_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_promotion",
                table: "warehouse_inventory_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_promotion",
                table: "warehouse_export_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_promotion",
                table: "purchase_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_promotion",
                table: "product_warehouse_history",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_promotion",
                table: "product_warehouse",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "partner_id",
                table: "product",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_promotion",
                table: "warehouse_receipt_product");

            migrationBuilder.DropColumn(
                name: "is_promotion",
                table: "warehouse_inventory_product");

            migrationBuilder.DropColumn(
                name: "is_promotion",
                table: "warehouse_export_product");

            migrationBuilder.DropColumn(
                name: "is_promotion",
                table: "purchase_product");

            migrationBuilder.DropColumn(
                name: "is_promotion",
                table: "product_warehouse_history");

            migrationBuilder.DropColumn(
                name: "is_promotion",
                table: "product_warehouse");

            migrationBuilder.AlterColumn<long>(
                name: "partner_id",
                table: "product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
