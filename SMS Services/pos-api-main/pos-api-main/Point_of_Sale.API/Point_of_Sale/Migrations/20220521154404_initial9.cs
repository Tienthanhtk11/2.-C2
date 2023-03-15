using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "warehouse_to",
                table: "warehouse_receipt",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "warehouse_from",
                table: "warehouse_receipt",
                newName: "request_id");

            migrationBuilder.CreateTable(
                name: "prouct_warehouse_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_table = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<byte>(type: "tinyint", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity_sold = table.Column<int>(type: "int", nullable: false),
                    quantity_stock = table.Column<int>(type: "int", nullable: false),
                    import_price = table.Column<double>(type: "float", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    unit_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    packing_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    exp_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    warning_date = table.Column<int>(type: "int", nullable: false),
                    warehouse_id = table.Column<long>(type: "bigint", nullable: false),
                    batch_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prouct_warehouse_history", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prouct_warehouse_history");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                table: "warehouse_receipt",
                newName: "warehouse_to");

            migrationBuilder.RenameColumn(
                name: "request_id",
                table: "warehouse_receipt",
                newName: "warehouse_from");
        }
    }
}
