using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.DataAccess.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Residences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumOfBeds = table.Column<int>(type: "int", nullable: false),
                    NumOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    ResidenceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOfBedrooms = table.Column<int>(type: "int", nullable: false),
                    LivingRoom = table.Column<bool>(type: "bit", nullable: false),
                    SquareMeters = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Smoking = table.Column<bool>(type: "bit", nullable: false),
                    Pets = table.Column<bool>(type: "bit", nullable: false),
                    Events = table.Column<bool>(type: "bit", nullable: false),
                    MinDaysForReservation = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residences", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Residences");
        }
    }
}
