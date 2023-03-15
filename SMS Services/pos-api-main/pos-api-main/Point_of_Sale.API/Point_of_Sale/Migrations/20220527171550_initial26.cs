using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "maxium_reduction",
                table: "voucher",
                newName: "maximum_reduction");

            migrationBuilder.AlterColumn<long>(
                name: "voucher_id",
                table: "order",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "maximum_reduction",
                table: "voucher",
                newName: "maxium_reduction");

            migrationBuilder.AlterColumn<long>(
                name: "voucher_id",
                table: "order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
