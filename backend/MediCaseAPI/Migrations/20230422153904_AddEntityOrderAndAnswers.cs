using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCaseAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityOrderAndAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Entities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Entities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Entities");
        }
    }
}
