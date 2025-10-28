using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class Create_Order_And_OrderItem_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("55d7c30d-ab4a-4fdf-9ff2-b35e59286e40"));

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("ae04ad68-0b7a-42f9-8d7d-af5dd0c25702"));

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    OrderNumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerRefId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchRefId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CartRefId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductRefId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductRefNumber = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("05b993a6-7c0d-4b2c-aaba-4d73878a88af"), new DateTime(2025, 10, 28, 18, 10, 50, 285, DateTimeKind.Utc).AddTicks(8252), "branch A" },
                    { new Guid("cd425c15-2954-4a04-be1c-82424d90c0cd"), new DateTime(2025, 10, 28, 18, 10, 50, 285, DateTimeKind.Utc).AddTicks(8257), "Branch B" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId_ProductRefNumber",
                table: "OrderItems",
                columns: new[] { "OrderId", "ProductRefNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductRefId",
                table: "OrderItems",
                column: "ProductRefId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductRefNumber",
                table: "OrderItems",
                column: "ProductRefNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchRefId",
                table: "Orders",
                column: "BranchRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartRefId",
                table: "Orders",
                column: "CartRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerRefId",
                table: "Orders",
                column: "CustomerRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("05b993a6-7c0d-4b2c-aaba-4d73878a88af"));

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("cd425c15-2954-4a04-be1c-82424d90c0cd"));

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("55d7c30d-ab4a-4fdf-9ff2-b35e59286e40"), new DateTime(2025, 10, 27, 21, 41, 24, 638, DateTimeKind.Utc).AddTicks(6042), "Branch B" },
                    { new Guid("ae04ad68-0b7a-42f9-8d7d-af5dd0c25702"), new DateTime(2025, 10, 27, 21, 41, 24, 638, DateTimeKind.Utc).AddTicks(6037), "branch A" }
                });
        }
    }
}
