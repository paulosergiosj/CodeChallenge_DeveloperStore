using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class User_Create_UserNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("05b4be53-13c6-4fd0-b623-c5513d947bfc"));

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("ea073788-95d3-4a66-983a-65cf357e5de3"));

            migrationBuilder.AddColumn<int>(
                name: "UserNumber",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("55d7c30d-ab4a-4fdf-9ff2-b35e59286e40"), new DateTime(2025, 10, 27, 21, 41, 24, 638, DateTimeKind.Utc).AddTicks(6042), "Branch B" },
                    { new Guid("ae04ad68-0b7a-42f9-8d7d-af5dd0c25702"), new DateTime(2025, 10, 27, 21, 41, 24, 638, DateTimeKind.Utc).AddTicks(6037), "branch A" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("55d7c30d-ab4a-4fdf-9ff2-b35e59286e40"));

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("ae04ad68-0b7a-42f9-8d7d-af5dd0c25702"));

            migrationBuilder.DropColumn(
                name: "UserNumber",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("05b4be53-13c6-4fd0-b623-c5513d947bfc"), new DateTime(2025, 10, 26, 3, 15, 2, 126, DateTimeKind.Utc).AddTicks(7609), "Branch B" },
                    { new Guid("ea073788-95d3-4a66-983a-65cf357e5de3"), new DateTime(2025, 10, 26, 3, 15, 2, 126, DateTimeKind.Utc).AddTicks(7601), "branch A" }
                });
        }
    }
}
