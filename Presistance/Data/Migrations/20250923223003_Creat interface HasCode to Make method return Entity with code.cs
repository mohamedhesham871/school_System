using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatinterfaceHasCodetoMakemethodreturnEntitywithcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnswerCode",
                table: "Answers",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_AnswerCode",
                table: "Answers",
                newName: "IX_Answers_Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Answers",
                newName: "AnswerCode");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_Code",
                table: "Answers",
                newName: "IX_Answers_AnswerCode");
        }
    }
}
