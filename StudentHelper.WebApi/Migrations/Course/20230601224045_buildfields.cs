using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentHelper.WebApi.Migrations.Course
{
    /// <inheritdoc />
    public partial class buildfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Values",
                table: "ErrorLogs",
                newName: "LogLevel");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "ErrorLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "ErrorLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExceptionMessage",
                table: "ErrorLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExceptionSource",
                table: "ErrorLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExceptionStackTrace",
                table: "ErrorLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ThreadId",
                table: "ErrorLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "EventName",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "ExceptionMessage",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "ExceptionSource",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "ExceptionStackTrace",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "ErrorLogs");

            migrationBuilder.RenameColumn(
                name: "LogLevel",
                table: "ErrorLogs",
                newName: "Values");
        }
    }
}
