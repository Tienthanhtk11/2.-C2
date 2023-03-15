using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial82 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "order_id",
                table: "warehouse_export",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "member_point",
                table: "refund",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "product_sale_cost",
                table: "refund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "voucher_cost",
                table: "refund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "member_point",
                table: "order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "member_point_value",
                table: "order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "member_point",
                table: "customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "customer_member_config",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    min_apply_value = table.Column<double>(type: "float", nullable: false),
                    ratio_point = table.Column<double>(type: "float", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    value_of_point = table.Column<int>(type: "int", nullable: false),
                    is_total_amount = table.Column<bool>(type: "bit", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_member_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer_point_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    number_of_point = table.Column<int>(type: "int", nullable: false),
                    value_of_point = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    userAdded = table.Column<long>(type: "bigint", nullable: false),
                    userUpdated = table.Column<long>(type: "bigint", nullable: true),
                    dateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_point_history", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_member_config");

            migrationBuilder.DropTable(
                name: "customer_point_history");

            migrationBuilder.DropColumn(
                name: "order_id",
                table: "warehouse_export");

            migrationBuilder.DropColumn(
                name: "member_point",
                table: "refund");

            migrationBuilder.DropColumn(
                name: "product_sale_cost",
                table: "refund");

            migrationBuilder.DropColumn(
                name: "voucher_cost",
                table: "refund");

            migrationBuilder.DropColumn(
                name: "member_point",
                table: "order");

            migrationBuilder.DropColumn(
                name: "member_point_value",
                table: "order");

            migrationBuilder.DropColumn(
                name: "member_point",
                table: "customer");
        }
    }
}
