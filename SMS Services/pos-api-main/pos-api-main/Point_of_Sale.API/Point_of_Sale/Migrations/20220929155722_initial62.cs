using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial62 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ecom_product_id",
                table: "warehouse_request_product");

            migrationBuilder.DropColumn(
                name: "ecom_product_id",
                table: "warehouse_request");

            migrationBuilder.DropColumn(
                name: "ecom_warehouse_id",
                table: "warehouse_request");

            migrationBuilder.DropColumn(
                name: "id_ecom",
                table: "warehouse_request");

            migrationBuilder.DropColumn(
                name: "shop_id",
                table: "warehouse_request");

            migrationBuilder.RenameColumn(
                name: "batch_number",
                table: "warehouse_request_product",
                newName: "barcode");

            migrationBuilder.AddColumn<long>(
                name: "warehouse_source_id",
                table: "purchase",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "warehouse_source_id",
                table: "purchase");

            migrationBuilder.RenameColumn(
                name: "barcode",
                table: "warehouse_request_product",
                newName: "batch_number");

            migrationBuilder.AddColumn<long>(
                name: "ecom_product_id",
                table: "warehouse_request_product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ecom_product_id",
                table: "warehouse_request",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ecom_warehouse_id",
                table: "warehouse_request",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "id_ecom",
                table: "warehouse_request",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "shop_id",
                table: "warehouse_request",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
