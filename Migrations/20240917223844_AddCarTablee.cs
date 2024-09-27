using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserOperations.Migrations
{
    /// <inheritdoc />
    public partial class AddCarTablee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarModel = table.Column<string>(type: "TEXT", nullable: true),
                    CarPlate = table.Column<string>(type: "TEXT", nullable: true),
                    Gearbox = table.Column<string>(type: "TEXT", nullable: true),
                    EKM = table.Column<string>(type: "TEXT", nullable: true),
                    YKM = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
