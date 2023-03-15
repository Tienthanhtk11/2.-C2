using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "batch_number",
                table: "prouct");

            migrationBuilder.DropColumn(
                name: "import_price",
                table: "prouct");

            migrationBuilder.DropColumn(
                name: "price",
                table: "prouct");

            migrationBuilder.AddColumn<string>(
                name: "barcode",
                table: "prouct_warehouse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ecom_prartner_id",
                table: "prouct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "prouct",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "partner",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_ecom = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    taxcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    introduce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partner", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partner");

            migrationBuilder.DropColumn(
                name: "barcode",
                table: "prouct_warehouse");

            migrationBuilder.DropColumn(
                name: "ecom_prartner_id",
                table: "prouct");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "prouct");

            migrationBuilder.AddColumn<string>(
                name: "batch_number",
                table: "prouct",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "import_price",
                table: "prouct",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "prouct",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
