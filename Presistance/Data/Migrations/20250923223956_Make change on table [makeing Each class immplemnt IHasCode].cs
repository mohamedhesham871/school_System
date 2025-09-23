using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakechangeontablemakeingEachclassimmplemntIHasCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuizCode",
                table: "Quizzes",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_QuizCode",
                table: "Quizzes",
                newName: "IX_Quizzes_Code");

            migrationBuilder.RenameColumn(
                name: "QuestionCode",
                table: "Questions",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuestionCode",
                table: "Questions",
                newName: "IX_Questions_Code");

            migrationBuilder.RenameColumn(
                name: "LessonCode",
                table: "Lessons",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_LessonCode",
                table: "Lessons",
                newName: "IX_Lessons_Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Quizzes",
                newName: "QuizCode");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_Code",
                table: "Quizzes",
                newName: "IX_Quizzes_QuizCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Questions",
                newName: "QuestionCode");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_Code",
                table: "Questions",
                newName: "IX_Questions_QuestionCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Lessons",
                newName: "LessonCode");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_Code",
                table: "Lessons",
                newName: "IX_Lessons_LessonCode");
        }
    }
}
