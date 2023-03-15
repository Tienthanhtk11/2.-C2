using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "warning_date",
                table: "product_warehouse");

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idtable = table.Column<long>(type: "bigint", nullable: false),
                    tablename = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    name_guid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ipserver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<byte>(type: "tinyint", nullable: false),
                    path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    file_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.AddColumn<int>(
                name: "warning_date",
                table: "product_warehouse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
