using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category_packing",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    note = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<byte>(type: "tinyint", nullable: false),
                    ecom_id = table.Column<long>(type: "bigint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_packing", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category_product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    note = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<byte>(type: "tinyint", nullable: false),
                    ecom_id = table.Column<long>(type: "bigint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category_unit",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    note = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<byte>(type: "tinyint", nullable: false),
                    ecom_id = table.Column<long>(type: "bigint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_unit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prouct",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    import_price = table.Column<double>(type: "float", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    id_ecom = table.Column<long>(type: "bigint", nullable: false),
                    category_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    batch_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prouct", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prouct_warehouse",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_prouct_warehouse", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_packing");

            migrationBuilder.DropTable(
                name: "category_product");

            migrationBuilder.DropTable(
                name: "category_unit");

            migrationBuilder.DropTable(
                name: "prouct");

            migrationBuilder.DropTable(
                name: "prouct_warehouse");
        }
    }
}
