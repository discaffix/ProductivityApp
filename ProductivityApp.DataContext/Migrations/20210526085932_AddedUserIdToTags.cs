using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductivityApp.DataAccess.Migrations
{
    public partial class AddedUserIdToTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tags",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tags");
        }
    }
}
