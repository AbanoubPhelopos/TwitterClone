using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twitter.Application.Migrations
{
    /// <inheritdoc />
    public partial class fixingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Follows_FolloweeId",
                table: "Follows",
                column: "FolloweeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FolloweeId",
                table: "Follows",
                column: "FolloweeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowerId",
                table: "Follows",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FolloweeId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowerId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_FolloweeId",
                table: "Follows");
        }
    }
}
