using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_prouct_warehouse_history",
                table: "prouct_warehouse_history");

            migrationBuilder.DropPrimaryKey(
                name: "PK_prouct_warehouse",
                table: "prouct_warehouse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_prouct",
                table: "prouct");

            migrationBuilder.RenameTable(
                name: "prouct_warehouse_history",
                newName: "product_warehouse_history");

            migrationBuilder.RenameTable(
                name: "prouct_warehouse",
                newName: "product_warehouse");

            migrationBuilder.RenameTable(
                name: "prouct",
                newName: "product");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_warehouse_history",
                table: "product_warehouse_history",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_warehouse",
                table: "product_warehouse",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product",
                table: "product",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_product_warehouse_history",
                table: "product_warehouse_history");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_warehouse",
                table: "product_warehouse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product",
                table: "product");

            migrationBuilder.RenameTable(
                name: "product_warehouse_history",
                newName: "prouct_warehouse_history");

            migrationBuilder.RenameTable(
                name: "product_warehouse",
                newName: "prouct_warehouse");

            migrationBuilder.RenameTable(
                name: "product",
                newName: "prouct");

            migrationBuilder.AddPrimaryKey(
                name: "PK_prouct_warehouse_history",
                table: "prouct_warehouse_history",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_prouct_warehouse",
                table: "prouct_warehouse",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_prouct",
                table: "prouct",
                column: "id");
        }
    }
}
