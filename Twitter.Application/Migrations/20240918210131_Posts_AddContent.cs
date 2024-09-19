using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twitter.Application.Migrations
{
    /// <inheritdoc />
    public partial class Posts_AddContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AutherId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "AutherId",
                table: "Posts",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AutherId",
                table: "Posts",
                newName: "IX_Posts_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Posts",
                newName: "AutherId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                newName: "IX_Posts_AutherId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AutherId",
                table: "Posts",
                column: "AutherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
