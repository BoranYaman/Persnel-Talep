using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserOperations.Migrations
{
    /// <inheritdoc />
    public partial class AddCarStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarStatus",
                table: "Cars",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarStatus",
                table: "Cars");
        }
    }
}
