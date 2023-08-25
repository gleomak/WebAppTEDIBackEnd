using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.DataAccess.Migrations
{
    public partial class NewResidence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06108650-f078-450b-9f64-e7c554380d4c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c4debcc-2280-4e2b-83d2-25a238f29afa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3734a96b-a932-4034-bc76-78e16a6699d3");

            migrationBuilder.AddColumn<bool>(
                name: "Aircondition",
                table: "Residences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CostPerNight",
                table: "Residences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Internet",
                table: "Residences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Kitchen",
                table: "Residences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ParkingSpot",
                table: "Residences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Tv",
                table: "Residences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5d281a53-6d59-4742-aec7-ec028c948570", "201a4898-d10a-4dd3-b593-afd13d8b25a0", "Host", "HOST" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7ad5cfb0-7184-4991-a634-572c00e8d7fb", "d964f93c-b192-4a72-bbd8-68b8f6f0340a", "Member", "MEMBER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8b4c321d-c140-48ca-afb2-71ebdaad1220", "154242bd-426f-471d-9c68-360dd0bc84fa", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d281a53-6d59-4742-aec7-ec028c948570");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ad5cfb0-7184-4991-a634-572c00e8d7fb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b4c321d-c140-48ca-afb2-71ebdaad1220");

            migrationBuilder.DropColumn(
                name: "Aircondition",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "CostPerNight",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "Internet",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "Kitchen",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "ParkingSpot",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "Tv",
                table: "Residences");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "06108650-f078-450b-9f64-e7c554380d4c", "13d03c21-9d82-4f8f-a190-7575ab8cf895", "Host", "HOST" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1c4debcc-2280-4e2b-83d2-25a238f29afa", "de969100-1294-40f0-94ab-368772505539", "Member", "MEMBER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3734a96b-a932-4034-bc76-78e16a6699d3", "a3ed2f4c-9b4a-48d9-b97c-7103fab8b032", "Admin", "ADMIN" });
        }
    }
}
