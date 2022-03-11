using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.API.Migrations
{
    public partial class AddEmployeeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("73ac1c67-ee15-4d5b-9ce3-b067d64de5b4"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("a5698328-5494-4b72-b791-5bca082cd472"));

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("bd8960e0-d4e8-421c-bb6f-828aaf2fb6f6"), "Great Company", "Microsoft" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("2d072dab-2c51-4933-a26e-a09fe1b4f218"), "Don't be evil", "Google" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("2fc93c7f-6959-421c-bf0a-a9ed5d5d1625"), new Guid("bd8960e0-d4e8-421c-bb6f-828aaf2fb6f6"), new DateTime(1996, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "G003", "Mary", 2, "King" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("e7a83f38-eab8-4fc0-abf8-f1cf1c3f19ae"), new Guid("bd8960e0-d4e8-421c-bb6f-828aaf2fb6f6"), new DateTime(1987, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "G097", "Kevin", 1, "Richardson" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("2d072dab-2c51-4933-a26e-a09fe1b4f218"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("2fc93c7f-6959-421c-bf0a-a9ed5d5d1625"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("e7a83f38-eab8-4fc0-abf8-f1cf1c3f19ae"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("bd8960e0-d4e8-421c-bb6f-828aaf2fb6f6"));

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("73ac1c67-ee15-4d5b-9ce3-b067d64de5b4"), "Great Compant", "Microsoft" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("a5698328-5494-4b72-b791-5bca082cd472"), "Don't be evil", "Google" });
        }
    }
}
