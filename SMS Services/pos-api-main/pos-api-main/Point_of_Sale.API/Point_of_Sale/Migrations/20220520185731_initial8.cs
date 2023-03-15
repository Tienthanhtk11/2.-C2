using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receipt");

            migrationBuilder.DropTable(
                name: "receipt_product");

            migrationBuilder.DropTable(
                name: "request");

            migrationBuilder.DropTable(
                name: "request_product");

            migrationBuilder.CreateTable(
                name: "warehouse",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_ecom = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<byte>(type: "tinyint", nullable: false),
                    id_come = table.Column<long>(type: "bigint", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    province_id = table.Column<long>(type: "bigint", nullable: false),
                    district_id = table.Column<long>(type: "bigint", nullable: false),
                    ward_id = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_receipt",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    warehouse_from = table.Column<long>(type: "bigint", nullable: false),
                    warehouse_to = table.Column<long>(type: "bigint", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transfer_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    delivery_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status_id = table.Column<byte>(type: "tinyint", nullable: false),
                    type = table.Column<byte>(type: "tinyint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_receipt", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_receipt_product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receipt_id = table.Column<long>(type: "bigint", nullable: false),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false),
                    category_unit_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_packing_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    warehouse_id = table.Column<long>(type: "bigint", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    exp_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    warning_date = table.Column<int>(type: "int", nullable: false),
                    batch_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_receipt_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_request",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    ecom_product_id = table.Column<long>(type: "bigint", nullable: false),
                    shop_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_ecom = table.Column<long>(type: "bigint", nullable: false),
                    warehouse_id = table.Column<long>(type: "bigint", nullable: false),
                    ecom_warehouse_id = table.Column<long>(type: "bigint", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transfer_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    delivery_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status_id = table.Column<byte>(type: "tinyint", nullable: false),
                    type = table.Column<byte>(type: "tinyint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_request", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_request_product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    request_id = table.Column<long>(type: "bigint", nullable: false),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    ecom_product_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false),
                    category_unit_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_packing_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    warehouse_id = table.Column<long>(type: "bigint", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    exp_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    warning_date = table.Column<int>(type: "int", nullable: false),
                    batch_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_request_product", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "warehouse");

            migrationBuilder.DropTable(
                name: "warehouse_receipt");

            migrationBuilder.DropTable(
                name: "warehouse_receipt_product");

            migrationBuilder.DropTable(
                name: "warehouse_request");

            migrationBuilder.DropTable(
                name: "warehouse_request_product");

            migrationBuilder.CreateTable(
                name: "receipt",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    delivery_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    status_id = table.Column<byte>(type: "tinyint", nullable: false),
                    transfer_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    type = table.Column<byte>(type: "tinyint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    warehouse_from = table.Column<long>(type: "bigint", nullable: false),
                    warehouse_to = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "receipt_product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    batch_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_packing_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_unit_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    exp_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_delete = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    receipt_id = table.Column<long>(type: "bigint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    warehouse_id = table.Column<long>(type: "bigint", nullable: false),
                    warning_date = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    delivery_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ecom_product_id = table.Column<long>(type: "bigint", nullable: false),
                    id_ecom = table.Column<long>(type: "bigint", nullable: false),
                    is_delete = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    shop_id = table.Column<long>(type: "bigint", nullable: false),
                    status_id = table.Column<byte>(type: "tinyint", nullable: false),
                    transfer_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    type = table.Column<byte>(type: "tinyint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    warehouse_from = table.Column<long>(type: "bigint", nullable: false),
                    warehouse_to = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request_product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    batch_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_packing_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_unit_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    exp_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_delete = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    receipt_id = table.Column<long>(type: "bigint", nullable: false),
                    shop_id = table.Column<long>(type: "bigint", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    warehouse_id = table.Column<long>(type: "bigint", nullable: false),
                    warning_date = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_product", x => x.id);
                });
        }
    }
}
