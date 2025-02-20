using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTable.Migrations
{
    /// <inheritdoc />
    public partial class addMajorInAssignCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MajorId",
                table: "AssignCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssignCourses_MajorId",
                table: "AssignCourses",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignCourses_Majors_MajorId",
                table: "AssignCourses",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignCourses_Majors_MajorId",
                table: "AssignCourses");

            migrationBuilder.DropIndex(
                name: "IX_AssignCourses_MajorId",
                table: "AssignCourses");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "AssignCourses");
        }
    }
}
