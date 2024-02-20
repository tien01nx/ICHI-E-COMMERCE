using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICHI_API.Migrations
{
    /// <inheritdoc />
    public partial class addSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "product_images");

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplier_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tax_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    banK_account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bank_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    create_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    create_user_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    update_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_user_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "product_images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
