using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class Update_Order_Relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

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
                    { new Guid("1646c51a-2c3a-4f96-af62-0d2e9c8d4942"), new DateTime(2025, 10, 28, 22, 1, 45, 398, DateTimeKind.Utc).AddTicks(3030), "branch A" },
                    { new Guid("913904d7-4ea8-4695-b4da-e9d331693b56"), new DateTime(2025, 10, 28, 22, 1, 45, 398, DateTimeKind.Utc).AddTicks(3032), "Branch B" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("1646c51a-2c3a-4f96-af62-0d2e9c8d4942"));

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("913904d7-4ea8-4695-b4da-e9d331693b56"));

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("05b993a6-7c0d-4b2c-aaba-4d73878a88af"), new DateTime(2025, 10, 28, 18, 10, 50, 285, DateTimeKind.Utc).AddTicks(8252), "branch A" },
                    { new Guid("cd425c15-2954-4a04-be1c-82424d90c0cd"), new DateTime(2025, 10, 28, 18, 10, 50, 285, DateTimeKind.Utc).AddTicks(8257), "Branch B" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
