using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticosApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePricesToCLP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 8, 20, 49, 2, 850, DateTimeKind.Local).AddTicks(7585), "https://images.unsplash.com/photo-1586495777744-4413f21062fa?w=300&h=300&fit=crop", 35990m });

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 8, 20, 49, 2, 850, DateTimeKind.Local).AddTicks(7646), "https://images.unsplash.com/photo-1512496015851-a90fb38ba796?w=300&h=300&fit=crop", 25990m });

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 8, 20, 49, 2, 850, DateTimeKind.Local).AddTicks(7649), "https://images.unsplash.com/photo-1512496015851-a90fb38ba796?w=300&h=300&fit=crop", 22990m });

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 8, 20, 49, 2, 850, DateTimeKind.Local).AddTicks(7652), "https://images.unsplash.com/photo-1556228453-efd6c1ff04f6?w=300&h=300&fit=crop", 12990m });

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 8, 20, 49, 2, 850, DateTimeKind.Local).AddTicks(7654), "https://images.unsplash.com/photo-1586495777744-4413f21062fa?w=300&h=300&fit=crop&random=2", 17990m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 6, 18, 14, 48, 911, DateTimeKind.Local).AddTicks(1858), null, 45.99m });

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 6, 18, 14, 48, 911, DateTimeKind.Local).AddTicks(1905), null, 32.50m });

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 6, 18, 14, 48, 911, DateTimeKind.Local).AddTicks(1908), null, 28.75m });

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 6, 18, 14, 48, 911, DateTimeKind.Local).AddTicks(1910), null, 15.99m });

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Precio" },
                values: new object[] { new DateTime(2025, 7, 6, 18, 14, 48, 911, DateTimeKind.Local).AddTicks(1913), null, 22.30m });
        }
    }
}
