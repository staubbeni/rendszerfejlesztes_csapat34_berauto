using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerAuto.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class JWT2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "LicensePlate",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "DailyRate",
                table: "Cars",
                newName: "Odometer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Odometer",
                table: "Cars",
                newName: "DailyRate");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LicensePlate",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
