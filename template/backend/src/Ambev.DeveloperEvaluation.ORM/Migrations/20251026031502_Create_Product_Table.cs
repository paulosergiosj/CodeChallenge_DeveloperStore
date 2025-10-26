using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class Create_Product_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("d59254ad-b614-4f51-8739-6ebd63c200aa"));

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("e3f51b7d-6277-415a-9873-c3f1da764c4f"));

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ProductNumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Rating_Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    Rating_Count = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("05b4be53-13c6-4fd0-b623-c5513d947bfc"), new DateTime(2025, 10, 26, 3, 15, 2, 126, DateTimeKind.Utc).AddTicks(7609), "Branch B" },
                    { new Guid("ea073788-95d3-4a66-983a-65cf357e5de3"), new DateTime(2025, 10, 26, 3, 15, 2, 126, DateTimeKind.Utc).AddTicks(7601), "branch A" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("05b4be53-13c6-4fd0-b623-c5513d947bfc"));

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: new Guid("ea073788-95d3-4a66-983a-65cf357e5de3"));

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("d59254ad-b614-4f51-8739-6ebd63c200aa"), new DateTime(2025, 10, 25, 13, 55, 23, 754, DateTimeKind.Utc).AddTicks(4412), "Branch B" },
                    { new Guid("e3f51b7d-6277-415a-9873-c3f1da764c4f"), new DateTime(2025, 10, 25, 13, 55, 23, 754, DateTimeKind.Utc).AddTicks(4410), "branch A" }
                });
        }
    }
}
