using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "barcode",
                table: "purchase_product");

            migrationBuilder.DropColumn(
                name: "batch_number",
                table: "purchase_product");

            migrationBuilder.DropColumn(
                name: "exp_date",
                table: "purchase_product");

            migrationBuilder.DropColumn(
                name: "warning_date",
                table: "purchase_product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "barcode",
                table: "purchase_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "batch_number",
                table: "purchase_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "exp_date",
                table: "purchase_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "warning_date",
                table: "purchase_product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
