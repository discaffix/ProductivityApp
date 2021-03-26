using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductivityApp.DataAccess.Migrations
{
    public partial class addedDbSetForSessionTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionTag_Sessions_SessionId",
                table: "SessionTag");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionTag_Tags_TagId",
                table: "SessionTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionTag",
                table: "SessionTag");

            migrationBuilder.RenameTable(
                name: "SessionTag",
                newName: "SessionTags");

            migrationBuilder.RenameIndex(
                name: "IX_SessionTag_TagId",
                table: "SessionTags",
                newName: "IX_SessionTags_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionTags",
                table: "SessionTags",
                columns: new[] { "SessionId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTags_Sessions_SessionId",
                table: "SessionTags",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTags_Tags_TagId",
                table: "SessionTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionTags_Sessions_SessionId",
                table: "SessionTags");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionTags_Tags_TagId",
                table: "SessionTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionTags",
                table: "SessionTags");

            migrationBuilder.RenameTable(
                name: "SessionTags",
                newName: "SessionTag");

            migrationBuilder.RenameIndex(
                name: "IX_SessionTags_TagId",
                table: "SessionTag",
                newName: "IX_SessionTag_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionTag",
                table: "SessionTag",
                columns: new[] { "SessionId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTag_Sessions_SessionId",
                table: "SessionTag",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTag_Tags_TagId",
                table: "SessionTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
