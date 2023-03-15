using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial58 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "barcode_int",
                table: "product_warehouse");

            migrationBuilder.DropColumn(
                name: "code",
                table: "product_warehouse");

            migrationBuilder.DropColumn(
                name: "ecom_prartner_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "id_ecom",
                table: "product");

            migrationBuilder.DropColumn(
                name: "product_batch_number",
                table: "order_detail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "barcode_int",
                table: "product_warehouse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "product_warehouse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ecom_prartner_id",
                table: "product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "id_ecom",
                table: "product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "product_batch_number",
                table: "order_detail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
