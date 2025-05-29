using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Assessment_Backend.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "DiscountEndDate", "DiscountPercentage", "DiscountStartDate", "ImagePath", "Name", "Price" },
                values: new object[,]
                {
                    { 12, "A lightweight running shoe with a breathable Flyknit upper and flexible Free sole for natural movement.", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.00m, new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "/ProductImages/domino-studio-164_6wVEHfI-unsplash.jpg", "Nike Free Flyknit", 120.00m },
                    { 13, "Classic black sunglasses with polarized lenses, offering UV protection and timeless style.", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.00m, new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "/ProductImages/giorgio-trovato-K62u25Jk6vo-unsplash.jpg", "Ray-Ban Classic", 150.00m },
                    { 14, "A sleek chronograph watch with a stainless steel bracelet, dark green dial, and Roman numeral markers.", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 20.00m, new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "/ProductImages/pexels-javon-swaby-197616-2783873.jpg", "Michael Kors Chronograph Watch", 250.00m },
                    { 15, "A vibrant pink lipstick paired with a matching blush compact, ideal for a coordinated makeup look.", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.00m, new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "/ProductImages/pexels-suzyhazelwood-2533266.jpg", "Pink Lipstick and Blush Set", 35.00m },
                    { 16, "A compact mirrorless camera with a 16.3MP sensor, retro design, and a versatile 18-55mm f/2.8-4 lens for high-quality photography.", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.00m, new DateTime(2025, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "/ProductImages/pexels-madebymath-90946.jpg", "Fujifilm X-T10 with XF 18-55mm Lens", 800.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
