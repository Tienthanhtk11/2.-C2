using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial78 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "category_id",
                table: "category_group",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category_id",
                table: "category_group");
        }
    }
}
