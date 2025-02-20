using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTable.Migrations
{
    /// <inheritdoc />
    public partial class dropmajor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Majors_MajorId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_MajorId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "Courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MajorId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_MajorId",
                table: "Courses",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Majors_MajorId",
                table: "Courses",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
