using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial61 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "packing_code",
                table: "product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit_code",
                table: "product",
                type: "nvarchar(max)",
                nullable: true);
        }
      
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "packing_code",
                table: "product");

            migrationBuilder.DropColumn(
                name: "unit_code",
                table: "product");
        }
    }
}
