using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTable.Migrations
{
    /// <inheritdoc />
    public partial class RemovePasswordFromFaculty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Faculties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Faculties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
