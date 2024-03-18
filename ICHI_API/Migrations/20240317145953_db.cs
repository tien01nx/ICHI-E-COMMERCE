using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ICHI_API.Migrations
{
    /// <inheritdoc />
    public partial class db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentID = table.Column<int>(type: "int", nullable: false),
                    CategoryLevel = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    log_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    log_status = table.Column<byte>(type: "tinyint", nullable: true),
                    pc_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    pg_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PromotionName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    MinimumPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trademarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrademarkName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trademarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    FailedPassAttemptCount = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "InventoryReceipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    isAvtive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryReceipts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrademarkId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriorityLevel = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Trademarks_TrademarkId",
                        column: x => x.TrademarkId,
                        principalTable: "Trademarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductReturns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReturns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReturns_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrxTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShoppingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentIntentID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrxTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrxTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionDetails_Promotions_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrxTransactionID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_TrxTransactions_TrxTransactionID",
                        column: x => x.TrxTransactionID,
                        principalTable: "TrxTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryReceiptDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryReceiptID = table.Column<int>(type: "int", nullable: false),
                    ProductDetailID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryReceiptDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryReceiptDetails_InventoryReceipts_InventoryReceiptID",
                        column: x => x.InventoryReceiptID,
                        principalTable: "InventoryReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryReceiptDetails_ProductDetails_ProductDetailID",
                        column: x => x.ProductDetailID,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductReturnDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrxTransactionID = table.Column<int>(type: "int", nullable: false),
                    ProductDetailID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReturnDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReturnDetails_ProductDetails_ProductDetailID",
                        column: x => x.ProductDetailID,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductReturnDetails_TrxTransactions_TrxTransactionID",
                        column: x => x.TrxTransactionID,
                        principalTable: "TrxTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryLevel", "CategoryName", "CreateBy", "CreateDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "Notes", "ParentID" },
                values: new object[,]
                {
                    { 1, 1, "Dụng cụ viết", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2937), false, null, null, "Mô tả về dụng cụ viết", 0 },
                    { 2, 1, "Giấy và sổ tay", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2941), false, null, null, "Mô tả về giấy và sổ tay", 0 },
                    { 3, 2, "Bút bi", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2944), false, null, null, "Mô tả về bút bi", 1 },
                    { 4, 2, "Bút mực", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2948), false, null, null, "Mô tả về bút mực", 1 },
                    { 5, 2, "Bút chì", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2951), false, null, null, "Mô tả về bút chì", 1 },
                    { 6, 2, "Giấy in", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2953), false, null, null, "Mô tả về giấy in", 2 },
                    { 7, 2, "Sổ tay", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2956), false, null, null, "Mô tả về sổ tay", 2 },
                    { 8, 2, "Sổ bìa cứng", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2959), false, null, null, "Mô tả về sổ bìa cứng", 2 },
                    { 9, 2, "Sổ bìa mềm", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2963), false, null, null, "Mô tả về sổ bìa mềm", 2 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateBy", "CreateDate", "Description", "ModifiedBy", "ModifiedDate", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2437), "Quản trị viên", null, null, "ADMIN" },
                    { 2, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2444), "Nhân viên", null, null, "EMPLOYEE" },
                    { 3, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2447), "Người dùng", null, null, "USER" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "BankAccount", "BankName", "CreateBy", "CreateDate", "Email", "ModifiedBy", "ModifiedDate", "Notes", "PhoneNumber", "SupplierName", "TaxCode", "isActive", "isDeleted" },
                values: new object[,]
                {
                    { 1, "123 Đường ABC, Quận XYZ, Thành phố HCM", "0123456789", "Ngân hàng ABC, Chi nhánh XYZ", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3004), "info@supplierA.com", null, null, "Thông tin chi tiết về nhà cung cấp A", "0123456789", "Nhà cung cấp A", "TAX001", true, false },
                    { 2, "456 Đường XYZ, Quận ABC, Thành phố HCM", "9876543210", "Ngân hàng XYZ, Chi nhánh ABC", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3009), "info2@supplier.com", null, null, "Thông tin chi tiết về nhà cung cấp B", "0987654321", "Nhà cung cấp B", "TAX002", true, false },
                    { 3, "789 Đường LMN, Quận PQR, Thành phố HCM", "7412589630", "Ngân hàng LMN, Chi nhánh PQR", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3013), "info3@gmail.com", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3010), "Thông tin chi tiết về nhà cung cấp C", "0369852147", "Nhà cung cấp C", "TAX003", true, false },
                    { 4, "789 Đường LMN, Quận PQR, Thành phố HCM", "7412589630", "Ngân hàng LMN, Chi nhánh PQR", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3016), "demo2@gmail.com", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3014), "Thông tin chi tiết về nhà cung cấp D", "0369852147", "Nhà cung cấp D", "TAX004", true, false },
                    { 5, "789 Đường LMN, Quận PQR, Thành phố HCM", "7412589630", "Ngân hàng LMN, Chi nhánh PQR", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3020), "demo2@gmail.com", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3018), "Thông tin chi tiết về nhà cung cấp E", "0369852147", "Nhà cung cấp E", "TAX005", true, false }
                });

            migrationBuilder.InsertData(
                table: "Trademarks",
                columns: new[] { "Id", "CreateBy", "CreateDate", "ModifiedBy", "ModifiedDate", "TrademarkName" },
                values: new object[,]
                {
                    { 1, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3045), null, null, "Thương hiệu A" },
                    { 2, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3048), null, null, "Thương hiệu B" },
                    { 3, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3050), null, null, "Thương hiệu C" },
                    { 4, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3052), null, null, "Thương hiệu D" },
                    { 5, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3055), null, null, "Thương hiệu E" },
                    { 6, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3057), null, null, "Thương hiệu F" },
                    { 7, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3060), null, null, "Thương hiệu G" },
                    { 8, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3062), null, null, "Thương hiệu H" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "Avatar", "CreateBy", "CreateDate", "FailedPassAttemptCount", "IsLocked", "ModifiedBy", "ModifiedDate", "Password" },
                values: new object[,]
                {
                    { "diuthanh88@gmail.com", "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw", "ADMIN", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2808), 0, false, "ADMIN", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2808), "$2a$11$iIcGb07Q8qC2x3bT2kXe4.7815/izsL8vF9tCLyKrtaCD06.HYF7." },
                    { "s2family2001bn@gmail.com", "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw", "ADMIN", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2811), 0, false, "ADMIN", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2812), "$2a$11$iIcGb07Q8qC2x3bT2kXe4.7815/izsL8vF9tCLyKrtaCD06.HYF7." },
                    { "tien01nx@gmail.com", "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw", "ADMIN", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2803), 0, false, "ADMIN", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2804), "$2a$11$iIcGb07Q8qC2x3bT2kXe4.7815/izsL8vF9tCLyKrtaCD06.HYF7." }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Avatar", "Birthday", "CreateBy", "CreateDate", "Email", "FullName", "Gender", "ModifiedBy", "ModifiedDate", "PhoneNumber", "UserId", "isActive", "isDeleted" },
                values: new object[] { 1, "123 Đường ABC, Quận XYZ, Thành phố HCM", "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2845), "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2848), "kh03@gmail.com", "Khách hàng A", "Nam", null, null, "0123456789", "s2family2001bn@gmail.com", true, false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "Avatar", "Birthday", "CreateBy", "CreateDate", "Email", "FullName", "Gender", "ModifiedBy", "ModifiedDate", "PhoneNumber", "UserId", "isActive", "isDeleted" },
                values: new object[] { 1, "123 Đường ABC, Quận XYZ, Thành", "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2875), "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(2876), "nhanvien@gmail.com", "Nhân viên A", "Nam", null, null, "0123456789", "diuthanh88@gmail.com", true, false });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Color", "CreateBy", "CreateDate", "Description", "ModifiedBy", "ModifiedDate", "Notes", "Price", "PriorityLevel", "ProductName", "Quantity", "TrademarkId", "isActive", "isDeleted" },
                values: new object[,]
                {
                    { 2, 9, "Trắng", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3099), "<p><img src=\"https://salt.tikicdn.com/ts/tmp/42/36/fa/73fa41041422e798678760b2150701c4.jpg\" alt=\"Bản Đồ\"></p>\r\n<p>H&atilde;y kh&aacute;m ph&aacute; thế giới c&ugrave;ng cuốn bản đồ khổng lồ đầu ti&ecirc;n ở Việt Nam! S&aacute;ch gồm 52 tấm bản đồ minh họa sinh động c&aacute;c đặc điểm địa l&yacute; v&agrave; bi&ecirc;n giới ch&iacute;nh trị, giới thiệu những địa điểm nổi tiếng, những n&eacute;t đặc trưng, về động vật v&agrave; thực vật bản địa, về con người địa phương, c&aacute;c sự kiện văn h&oacute;a c&ugrave;ng nhiều th&ocirc;ng tin hấp dẫn kh&aacute;c.<br><br>Đến với cuốn Bản đồ khổng lồ (27x37cm) gồm 52 tấm bản đồ đầy m&agrave;u sắc sống động n&agrave;y, c&aacute;c bạn nhỏ sẽ được thỏa sức kh&aacute;m ph&aacute; thế giới. C&oacute; tất cả 6 tấm bản đồ lục địa v&agrave; 42 bản đồ quốc gia. Ch&acirc;u u c&oacute; g&igrave;, ch&acirc;u &Aacute; nổi tiếng v&igrave; điều chi, kh&iacute; hậu ở ch&acirc;u Phi như thế n&agrave;o? Tất cả những chi tiết nổi bật của từng v&ugrave;ng miền, từng đất nước, như địa danh, trang phục, ẩm thực, lễ hội tập tục truyền thống, v&hellip;v&hellip; đều được liệt k&ecirc; bằng những h&igrave;nh vẽ ngộ nghĩnh đ&aacute;ng y&ecirc;u. Mỗi bản đồ c&oacute; thống k&ecirc; sơ bộ về diện t&iacute;ch, d&acirc;n số, ng&ocirc;n ngữ&hellip; để c&aacute;c bạn nhỏ nắm được th&ocirc;ng tin tổng qu&aacute;t của từng đất nước, ch&acirc;u lục. Mỗi nước đều được ph&acirc;n chia th&agrave;nh c&aacute;c v&ugrave;ng địa l&yacute; cụ thể với t&ecirc;n v&ugrave;ng được viết mờ, c&aacute;c th&agrave;nh phố lớn trong từng nước được viết bằng m&agrave;u đỏ nổi bật với chấm đỏ b&ecirc;n cạnh.<br><br>Cuốn s&aacute;ch n&agrave;y hứa hẹn sẽ l&agrave; tấm v&eacute; đưa độc giả nhỏ du lịch khắp mọi miền tr&ecirc;n thế giới. C&aacute;c bậc phụ huynh cũng c&oacute; thể đồng h&agrave;nh c&ugrave;ng con em m&igrave;nh, c&ugrave;ng ng&acirc;m cứu từng chi tiết tr&ecirc;n mỗi tấm bản đồ, t&igrave;m hiểu v&agrave; b&agrave;n luận về c&aacute;c địa phương. Th&ocirc;ng qua việc chỉ dẫn, diễn giải cho c&aacute;c con về những th&ocirc;ng tin tr&ecirc;n bản đồ, đ&acirc;y sẽ l&agrave; cuốn s&aacute;ch tương t&aacute;c tốt để bố mẹ kết nối v&agrave; gần gũi với con m&igrave;nh hơn.<br><br><strong>CUỐN S&Aacute;CH N&Agrave;Y C&Oacute; G&Igrave; ĐẶC BIỆT?</strong><br><br>Cuốn s&aacute;ch Bản đồ đ&atilde; được xuất bản tại hơn 30 quốc gia, b&aacute;n được hơn 3 triệu bản in, l&agrave; một trong những cuốn bản đồ ăn kh&aacute;ch nhất thế giới. Bản đồ của hai t&aacute;c giả Aleksandra Mizielińska v&agrave; Daniel Mizieliński đ&atilde; gi&agrave;nh được nhiều giải thưởng lớn, nổi bật nhất l&agrave; giải Prix Sorci&egrave;res của Ph&aacute;p v&agrave; giải Premio Andersen của &Yacute; &ndash; hai giải thưởng danh gi&aacute; cho d&ograve;ng s&aacute;ch thiếu nhi.<br><br>C&aacute;c quốc gia đ&atilde; xuất bản &ldquo;Bản đồ&rdquo;: &Uacute;c, &Aacute;o, Bỉ, Brazil, Canada, Chile, Trung Quốc, Croatia, S&eacute;c, Ecuador, Ai Cập, Fiji, Phần Land, Ph&aacute;p, Đức, Ghana, Hy Lạp, Iceland, Ấn Độ, &Yacute;, Nhật Bản, Jordan, Madagascar, Ma Rốc, Mexico, M&ocirc;ng Cổ, Namibia, Nepal, H&agrave; Lan, New Zealand, Peru, Ba Lan, Nam Phi, Romania, Nga, T&acirc;y Ban Nha, Thụy Điển, Thụy Sĩ, Tanzania, Th&aacute;i Lan, Anh, Mỹ.<br><br>ĐẶC BIỆT: Phi&ecirc;n bản \"Bản đồ\" Việt Nam đặc biệt được t&aacute;c giả vẽ ri&ecirc;ng đất nước Việt Nam.<br><br>Để thực hiện cuốn s&aacute;ch đồ sộ n&agrave;y, hai t&aacute;c giả trẻ đ&atilde; phải mất hơn 3 năm trời. Sau khi nghi&ecirc;n cứu v&agrave; t&igrave;m hiểu kỹ lưỡng, họ lập một danh s&aacute;ch c&aacute;c th&ocirc;ng tin hấp dẫn v&agrave; th&uacute; vị với trẻ em, chọn lọc ra những chi tiết đặc sắc nhất của mỗi nước để vẽ v&agrave;o bản đồ. C&aacute;c tấm bản đồ đều được vẽ theo tỉ lệ chuẩn x&aacute;c dựa tr&ecirc;n c&aacute;c bản đồ địa l&yacute; đ&atilde; được ph&aacute;t h&agrave;nh. Hai t&aacute;c giả kh&ocirc;ng chỉ vẽ tay tất cả c&aacute;c chi tiết h&igrave;nh ảnh m&agrave; c&ograve;n d&agrave;y c&ocirc;ng thiết kế tất cả c&aacute;c ph&ocirc;ng chữ được d&ugrave;ng trong s&aacute;ch.</p>\r\n<p>Gi&aacute; sản phẩm tr&ecirc;n Tiki đ&atilde; bao gồm thuế theo luật hiện h&agrave;nh. B&ecirc;n cạnh đ&oacute;, tuỳ v&agrave;o loại sản phẩm, h&igrave;nh thức v&agrave; địa chỉ giao h&agrave;ng m&agrave; c&oacute; thể ph&aacute;t sinh th&ecirc;m chi ph&iacute; kh&aacute;c như ph&iacute; vận chuyển, phụ ph&iacute; h&agrave;ng cồng kềnh, thuế nhập khẩu (đối với đơn h&agrave;ng giao từ nước ngo&agrave;i c&oacute; gi&aacute; trị tr&ecirc;n 1 triệu đồng).....</p>", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3088), "", 50000m, 0, "Bản Đồ", 0.0, 4, true, false },
                    { 3, 5, "Trắng", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3103), "<p>Kh&ocirc;ng c&oacute; g&igrave; l&agrave; ngẫu nhi&ecirc;n.<br>Mọi chuyện đều l&agrave; tất nhi&ecirc;n.<br>Một cuốn s&aacute;ch t&acirc;m linh gi&uacute;p bạn giải quyết những vấn đề trong cuộc sống, c&ocirc;ng việc, t&igrave;nh cảm&hellip; Nếu bạn đang ph&acirc;n v&acirc;n trước những lựa chọn, nếu bạn đang thiếu quyết định, nếu bạn kh&ocirc;ng biết tiếp theo n&ecirc;n l&agrave;m g&igrave;: h&atilde;y đặt một c&acirc;u hỏi.<br>V&agrave; h&atilde;y để những vị thần quyết định thay bạn.</p>", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3100), "", 45000m, 0, "Vị Thần Của Những Quyết Định", 0.0, 4, true, false },
                    { 4, 7, "Trắng", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3107), "<p class=\"MsoNormal\"><strong><img src=\"https://vcdn.tikicdn.com/ts/tmp/a1/dd/30/0d1aa4020c3f5ece81362f1849e56a5e.jpg\" alt=\"\" width=\"750\" height=\"972\"></strong></p>\r\n<p class=\"MsoNormal\"><strong>Hiểu Về Tr&aacute;i Tim &ndash; Cuốn S&aacute;ch Mở Cửa Thề Giới Cảm X&uacute;c Của Mỗi Người</strong></p>\r\n<p class=\"MsoNormal\">Xuất bản lần đầu ti&ecirc;n v&agrave;o năm 2011, Hiểu Về Tr&aacute;i Tim tr&igrave;nh l&agrave;ng cũng l&uacute;c với kỷ lục: cuốn s&aacute;ch c&oacute; số lượng in lần đầu lớn nhất: 100.000 bản. Trung t&acirc;m s&aacute;ch kỷ lục Việt Nam c&ocirc;ng nhận kỳ t&iacute;ch ấy nhưng đến nay, con số ph&aacute;t h&agrave;nh của Hiểu về tr&aacute;i tim vẫn chưa c&oacute; dấu hiệu chậm lại.</p>\r\n<p class=\"MsoNormal\">L&agrave; t&aacute;c phẩm đầu tay của nh&agrave; sư Minh Niệm, người s&aacute;ng lập d&ograve;ng thiền hiểu biết (Understanding Meditation), kết hợp giữa tư tưởng Phật gi&aacute;o Đại thừa v&agrave; Thiền nguy&ecirc;n thủy Vipassana, nhưng Hiểu Về Tr&aacute;i Tim kh&ocirc;ng phải t&aacute;c phẩm thuyết gi&aacute;o về Phật ph&aacute;p. Cuốn s&aacute;ch rất &ldquo;đời&rdquo; với những ưu tư của một người tu nh&igrave;n về c&otilde;i thế. Ở đ&oacute;, c&oacute; hạnh ph&uacute;c, c&oacute; đau khổ, c&oacute; t&igrave;nh y&ecirc;u, c&oacute; c&ocirc; đơn, c&oacute; tuyệt vọng, c&oacute; lười biếng, c&oacute; yếu đuối, c&oacute; bu&ocirc;ng xả&hellip; Nhưng, tất cả những hỉ nộ &aacute;i ố ấy đều được kho&aacute;c l&ecirc;n tấm &aacute;o mới, một tấm &aacute;o tinh khiết v&agrave; xuy&ecirc;n suốt, khiến người đọc khi nh&igrave;n v&agrave;o, đều thấy mọi sự như nhẹ nh&agrave;ng hơn&hellip;</p>\r\n<p class=\"MsoNormal\"><img src=\"https://vcdn.tikicdn.com/ts/product/0c/e1/06/06a3b9bfbbd775345370ff6629eadb4e.jpg\" alt=\"\" width=\"750\" height=\"971\"></p>\r\n<p class=\"MsoNormal\">Sinh tại Ch&acirc;u Th&agrave;nh, Tiền Giang, xuất gia tại Phật Học Viện Huệ Nghi&ecirc;m &ndash; S&agrave;i G&ograve;n, Minh Niệm từng thọ gi&aacute;o thiền sư Th&iacute;ch Nhất Hạnh tại Ph&aacute;p v&agrave; thiền sư Tejaniya tại Mỹ. Kết quả sau qu&aacute; tr&igrave;nh tu tập, lĩnh hội kiến thức&hellip; &Ocirc;ng quyết định chọn con đường hướng dẫn thiền v&agrave; khai triển t&acirc;m l&yacute; trị liệu cho giới trẻ l&agrave;m Phật sự của m&igrave;nh. Tiếp cận với nhiều người trẻ, lắng nghe thế giới quan của họ v&agrave; quan s&aacute;t những đổi thay trong đời sống hiện đại, &ocirc;ng ph&aacute;t hiện c&oacute; rất nhiều vấn đề của cuộc sống. Nhưng, tất cả, chỉ xuất ph&aacute;t từ một nguy&ecirc;n nh&acirc;n: Ch&uacute;ng ta chưa hiểu, v&agrave; chưa hiểu đ&uacute;ng về tr&aacute;i tim m&igrave;nh l&agrave; chưa cơ chế vận động của những hỉ, nộ, &aacute;i, ố trong mỗi con người. &ldquo;T&ocirc;i đ&atilde; từng quyết l&ograve;ng ra đi t&igrave;m hạnh ph&uacute;c ch&acirc;n thật. D&ugrave; thời điểm ấy, &yacute; niệm về hạnh ph&uacute;c ch&acirc;n thật trong t&ocirc;i rất mơ hồ nhưng t&ocirc;i vẫn tin rằng n&oacute; c&oacute; thật v&agrave; lu&ocirc;n hiện hữu trong thực tại. Hơn mười năm sau, t&ocirc;i mới thấy con đường. V&agrave; cũng chừng ấy năm nữa, t&ocirc;i mới tự tin đặt b&uacute;t viết về những điều m&igrave;nh đ&atilde; kh&aacute;m ph&aacute; v&agrave; trải nghiệm&hellip;&rdquo;, t&aacute;c giả chia sẻ.</p>\r\n<p class=\"MsoNormal\"><img src=\"https://vcdn.tikicdn.com/ts/tmp/5a/92/d0/fbf3268e4f18030feeff2f22f2583d90.jpg\" alt=\"\" width=\"750\" height=\"976\"></p>\r\n<p class=\"MsoNormal\">Gần 500 trang s&aacute;ch, Hiểu Về Tr&aacute;i Tim l&agrave; những ph&aacute;c thảo r&otilde; n&eacute;t về bức tranh đời sống cảm x&uacute;c của tất cả mọi người. Người đọc sẽ t&igrave;m thấy căn nguy&ecirc;n th&agrave;nh h&igrave;nh của những x&uacute;c cảm, thấy cả việc ch&uacute;ng chi phối thế n&agrave;o đến h&agrave;nh xử thường ng&agrave;y v&agrave; quan trọng hơn cả l&agrave; c&aacute;ch thức để điều khiển ch&uacute;ng thế n&agrave;o. Kh&ocirc;ng c&oacute; c&acirc;u trả lời cuối c&ugrave;ng của việc đ&uacute;ng &ndash; sai trong từng t&igrave;nh huống nhưng Hiểu Về Tr&aacute;i Tim c&oacute; chứa trong n&oacute; ch&igrave;a kh&oacute;a để mở ra một c&aacute;nh cửa đến với thế giới mới, thế giới an lạc từ trong t&acirc;m mỗi người. Bởi, suy cho c&ugrave;ng, mỗi tr&aacute;i tim - cơ quan ch&uacute;ng ta thường gắn cho nhiệm vụ điều khiển tr&iacute; tuệ cảm x&uacute;c của con người, đều c&oacute; những nỗi niềm ri&ecirc;ng. Chỉ cần hiểu c&acirc;u chuyện của tr&aacute;i tim, tự khắc, mỗi người sẽ quyết định được c&acirc;u chuyện của ch&iacute;nh m&igrave;nh. B&iacute; quyết của sự chuyển h&oacute;a l&agrave; kh&ocirc;ng n&ecirc;n d&ugrave;ng &yacute; ch&iacute; để &aacute;p đặt hay nhồi nặn t&acirc;m m&igrave;nh trở th&agrave;nh một kiểu mẫu tốt đẹp n&agrave;o đ&oacute;. Chỉ cẩn quan s&aacute;t v&agrave; thấu hiểu ch&uacute;ng l&agrave; đủ. T&aacute;c giả nhận định: &ldquo;Việc đưa t&acirc;m thức vượt l&ecirc;n những cung bậc cao hơn để nh&igrave;n đ&uacute;ng đắn hơn về th&acirc;n phận của m&igrave;nh v&agrave; bản chất cuộc sống l&agrave; điều ho&agrave;n to&agrave;n c&oacute; thể l&agrave;m được&rdquo;.</p>\r\n<p class=\"MsoNormal\"><img src=\"https://vcdn.tikicdn.com/ts/product/12/79/74/175d52e69c01d68030aac2eb7e3d33eb.jpg\" alt=\"\" width=\"750\" height=\"974\"></p>\r\n<p class=\"MsoNormal\">L&uacute;c sinh thời cố Gi&aacute;o sư, Tiến sĩ Trần Văn Khu&ecirc;, c&oacute; dịp tiếp cận với Hiểu Về Tr&aacute;i Tim. &Ocirc;ng nhận x&eacute;t, như một cuốn s&aacute;ch đầu ti&ecirc;n thuộc chủ đề Hạt Giống T&acirc;m Hồn do một t&aacute;c giả Việt Nam viết, cuốn s&aacute;ch sẽ gi&uacute;p người đọc hiểu được cảm x&uacute;c của t&acirc;m hồn, tr&aacute;i tim của ch&iacute;nh m&igrave;nh v&agrave; của người kh&aacute;c. Để, tận c&ugrave;ng l&agrave; loại bỏ nỗi buồn, tổn thương v&agrave; t&igrave;m được hạnh ph&uacute;c trong cuộc sống. C&oacute; lẽ, v&igrave; điều n&agrave;y m&agrave; gần 10 năm qua, Hiểu về tr&aacute;i tim vẫn l&agrave; cuốn s&aacute;ch li&ecirc;n tục được t&aacute;i bản v&agrave; chưa c&oacute; dấu hiệu &ldquo;hạ nhiệt&rdquo;. Đ&aacute;ng qu&yacute; hơn, t&ograve;an bộ lợi nhuận thu được từ việc ph&aacute;t h&agrave;nh cuốn s&aacute;ch n&agrave;y đều được chuyển về quỹ từ thiện c&ugrave;ng t&ecirc;n để gi&uacute;p đỡ trẻ em c&oacute; ho&agrave;n cảnh kh&oacute; khăn, bất hạnh tại Việt Nam.</p>\r\n<p>Gi&aacute; sản phẩm tr&ecirc;n Tiki đ&atilde; bao gồm thuế theo luật hiện h&agrave;nh. B&ecirc;n cạnh đ&oacute;, tuỳ v&agrave;o loại sản phẩm, h&igrave;nh thức v&agrave; địa chỉ giao h&agrave;ng m&agrave; c&oacute; thể ph&aacute;t sinh th&ecirc;m chi ph&iacute; kh&aacute;c như ph&iacute; vận chuyển, phụ ph&iacute; h&agrave;ng cồng kềnh, thuế nhập khẩu (đối với đơn h&agrave;ng giao từ nước ngo&agrave;i c&oacute; gi&aacute; trị tr&ecirc;n 1 triệu đồng).....</p>", "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3105), "", 60000m, 0, "Sách Hiểu Về Trái Tim(Tái Bản 2019) -Minh Niệm", 0.0, 5, true, false }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "tien01nx@gmail.com" },
                    { 2, 2, "diuthanh88@gmail.com" },
                    { 3, 3, "s2family2001bn@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "CreateBy", "CreateDate", "ImageName", "ImagePath", "IsActive", "IsDefault", "IsDeleted", "ModifiedBy", "ModifiedDate", "ProductId" },
                values: new object[,]
                {
                    { 2, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3203), "574854f032ae36fc0d0a57b61f588965.jpg", "\\images\\products\\product-2\\66df2941-a432-4b5f-a1ef-4f07eec2e608.jpg", true, false, false, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3204), 2 },
                    { 3, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3208), "5cb2991cc6a258b7c1cc07105bccaa29.jpg", "\\images\\products\\product-3\\04be0986-90ea-41b4-ac17-153d52f3fe74.jpg", true, false, false, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3209), 3 },
                    { 4, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3212), "3f23c30055381c7e58af80a62ce28fa5.jpg", "\\images\\products\\product-4\\0016eca6-2e6e-44d2-874f-b2deefb97893.jpg", true, false, false, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3212), 4 },
                    { 5, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3216), "Screenshot_3.png", "\\images\\products\\product-5\\ff69973a-bad5-4757-be92-6792b6d4ff9e.png", true, false, false, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3217), 4 },
                    { 6, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3220), "Screenshot_7.png", "\\images\\products\\product-6\\0c3182bd-e13f-4303-b7d7-c69c0616b6b8.png", true, false, false, "Admin", new DateTime(2024, 3, 17, 21, 59, 53, 415, DateTimeKind.Local).AddTicks(3221), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceiptDetails_InventoryReceiptID",
                table: "InventoryReceiptDetails",
                column: "InventoryReceiptID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceiptDetails_ProductDetailID",
                table: "InventoryReceiptDetails",
                column: "ProductDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceipts_SupplierID",
                table: "InventoryReceipts",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReturnDetails_ProductDetailID",
                table: "ProductReturnDetails",
                column: "ProductDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReturnDetails_TrxTransactionID",
                table: "ProductReturnDetails",
                column: "TrxTransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReturns_UserId",
                table: "ProductReturns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TrademarkId",
                table: "Products",
                column: "TrademarkId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionDetails_ProductID",
                table: "PromotionDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionDetails_PromotionID",
                table: "PromotionDetails",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_TrxTransactionID",
                table: "TransactionDetails",
                column: "TrxTransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_TrxTransactions_UserId",
                table: "TrxTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "InventoryReceiptDetails");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductReturnDetails");

            migrationBuilder.DropTable(
                name: "ProductReturns");

            migrationBuilder.DropTable(
                name: "PromotionDetails");

            migrationBuilder.DropTable(
                name: "TransactionDetails");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "InventoryReceipts");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "TrxTransactions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Trademarks");
        }
    }
}
