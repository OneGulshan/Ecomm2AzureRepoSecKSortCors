using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Books",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<bool>(
                name: "Trending",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trending",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Books",
                newName: "CreateDate");
        }
    }
}
