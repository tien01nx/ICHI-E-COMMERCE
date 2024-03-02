using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICHI_API.Migrations
{
    /// <inheritdoc />
    public partial class db1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_user_UserId",
                table: "customer");

            migrationBuilder.DropIndex(
                name: "IX_customer_UserId",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "customer",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_customer_UserId",
                table: "customer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_user_UserId",
                table: "customer",
                column: "UserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
