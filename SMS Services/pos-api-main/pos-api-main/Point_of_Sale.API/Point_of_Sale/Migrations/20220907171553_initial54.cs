using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category_packing_code",
                table: "warehouse_inventory_product");

            migrationBuilder.DropColumn(
                name: "category_unit_code",
                table: "warehouse_inventory_product");

            migrationBuilder.AlterColumn<bool>(
                name: "is_weigh",
                table: "warehouse_inventory_product",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "batch_number",
                table: "warehouse_inventory_product",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "barcode",
                table: "warehouse_inventory_product",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "packing_code",
                table: "warehouse_inventory_product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit_code",
                table: "warehouse_inventory_product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "warehouse_inventory_id",
                table: "warehouse_inventory_product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "packing_code",
                table: "warehouse_inventory_product");

            migrationBuilder.DropColumn(
                name: "unit_code",
                table: "warehouse_inventory_product");

            migrationBuilder.DropColumn(
                name: "warehouse_inventory_id",
                table: "warehouse_inventory_product");

            migrationBuilder.AlterColumn<bool>(
                name: "is_weigh",
                table: "warehouse_inventory_product",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "batch_number",
                table: "warehouse_inventory_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "barcode",
                table: "warehouse_inventory_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "category_packing_code",
                table: "warehouse_inventory_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "category_unit_code",
                table: "warehouse_inventory_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
