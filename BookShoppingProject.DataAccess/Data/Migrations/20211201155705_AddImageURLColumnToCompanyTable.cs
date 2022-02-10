using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShoppingProject.DataAccess.Migrations
{
    public partial class AddImageURLColumnToCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Companies");
        }
    }
}
