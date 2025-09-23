using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakechangeontablesubjectmakeingclassimmplemntIHasCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectCode",
                table: "Subjects",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_SubjectCode",
                table: "Subjects",
                newName: "IX_Subjects_Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Subjects",
                newName: "SubjectCode");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_Code",
                table: "Subjects",
                newName: "IX_Subjects_SubjectCode");
        }
    }
}
