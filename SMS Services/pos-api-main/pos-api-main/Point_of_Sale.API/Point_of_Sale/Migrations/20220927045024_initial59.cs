using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial59 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "product");
        }
    }
}
