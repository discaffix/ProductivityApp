using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductivityApp.DataAccess.Migrations
{
    public partial class AddedProjectName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Projects");
        }
    }
}
