using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerAuto.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class JWT4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Rentals");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Rentals",
                newName: "RequestDate");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Rentals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Rentals",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Rentals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "Rentals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestAddress",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestEmail",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestName",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestPhone",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupDate",
                table: "Rentals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "Rentals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "GuestAddress",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "GuestEmail",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "GuestName",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "GuestPhone",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "PickupDate",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Rentals");

            migrationBuilder.RenameColumn(
                name: "RequestDate",
                table: "Rentals",
                newName: "StartDate");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Rentals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TotalCost",
                table: "Rentals",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Rentals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
