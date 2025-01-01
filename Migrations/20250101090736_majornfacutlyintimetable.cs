using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTable.Migrations
{
    /// <inheritdoc />
    public partial class majornfacutlyintimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "Timetables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MajorId",
                table: "Timetables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_FacultyId",
                table: "Timetables",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_MajorId",
                table: "Timetables",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_Faculties_FacultyId",
                table: "Timetables",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_Majors_MajorId",
                table: "Timetables",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_Faculties_FacultyId",
                table: "Timetables");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_Majors_MajorId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_FacultyId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_MajorId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "Timetables");
        }
    }
}
