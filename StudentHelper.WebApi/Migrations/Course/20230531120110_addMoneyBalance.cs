using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentHelper.WebApi.Migrations.Course
{
    /// <inheritdoc />
    public partial class addMoneyBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Courses");

            migrationBuilder.AddColumn<decimal>(
                name: "MoneyBalance",
                table: "Students",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MoneyBalance",
                table: "Sellers",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoneyBalance",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MoneyBalance",
                table: "Sellers");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Courses",
                type: "bit",
                nullable: true);
        }
    }
}
