using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReationOfSudentWithOtherClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnrollmentDate",
                table: "AspNetUsers",
                newName: "AssignToSchool");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "StudentClasses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StudentAssignInSubject",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAssignInSubject", x => new { x.StudentId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_StudentAssignInSubject_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAssignInSubject_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClassID",
                table: "AspNetUsers",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GradeID",
                table: "AspNetUsers",
                column: "GradeID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignInSubject_SubjectId",
                table: "StudentAssignInSubject",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Grades_GradeID",
                table: "AspNetUsers",
                column: "GradeID",
                principalTable: "Grades",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_StudentClasses_ClassID",
                table: "AspNetUsers",
                column: "ClassID",
                principalTable: "StudentClasses",
                principalColumn: "ClassID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Grades_GradeID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_StudentClasses_ClassID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "StudentAssignInSubject");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClassID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GradeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "StudentClasses");

            migrationBuilder.RenameColumn(
                name: "AssignToSchool",
                table: "AspNetUsers",
                newName: "EnrollmentDate");
        }
    }
}
