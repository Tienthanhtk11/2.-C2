using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weight",
                table: "warehouse_export_product");

            migrationBuilder.DropColumn(
                name: "quatity",
                table: "order_detail");

            migrationBuilder.RenameColumn(
                name: "receipt_id",
                table: "warehouse_export_product",
                newName: "products_warehouse_id");

            migrationBuilder.RenameColumn(
                name: "category_unit_code",
                table: "warehouse_export_product",
                newName: "unit_code");

            migrationBuilder.RenameColumn(
                name: "category_packing_code",
                table: "warehouse_export_product",
                newName: "packing_code");

            migrationBuilder.AlterColumn<double>(
                name: "quantity",
                table: "warehouse_export_product",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "exp_date",
                table: "warehouse_export_product",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "quantity_in_stock",
                table: "product_warehouse_history",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "quantity",
                table: "product_warehouse_history",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "quantity_stock",
                table: "product_warehouse",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "quantity_sold",
                table: "product_warehouse",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "quantity",
                table: "order_detail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantity",
                table: "order_detail");

            migrationBuilder.DropColumn(
                name: "note",
                table: "order");

            migrationBuilder.RenameColumn(
                name: "unit_code",
                table: "warehouse_export_product",
                newName: "category_unit_code");

            migrationBuilder.RenameColumn(
                name: "products_warehouse_id",
                table: "warehouse_export_product",
                newName: "receipt_id");

            migrationBuilder.RenameColumn(
                name: "packing_code",
                table: "warehouse_export_product",
                newName: "category_packing_code");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "warehouse_export_product",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "exp_date",
                table: "warehouse_export_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "weight",
                table: "warehouse_export_product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "quantity_in_stock",
                table: "product_warehouse_history",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "product_warehouse_history",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "quantity_stock",
                table: "product_warehouse",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "quantity_sold",
                table: "product_warehouse",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "quatity",
                table: "order_detail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
