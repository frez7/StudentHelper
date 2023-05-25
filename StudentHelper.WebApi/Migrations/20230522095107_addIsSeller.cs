using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentHelper.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class addIsSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeller",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeller",
                table: "AspNetUsers");
        }
    }
}
