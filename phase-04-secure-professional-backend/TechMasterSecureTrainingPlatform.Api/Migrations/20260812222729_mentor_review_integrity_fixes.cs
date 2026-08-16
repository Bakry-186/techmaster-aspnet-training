using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingCenter.Api.Migrations
{
    /// <inheritdoc />
    public partial class mentor_review_integrity_fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingTracks_Code",
                table: "TrainingTracks");

            migrationBuilder.DropIndex(
                name: "IX_Students_Email",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTracks_Code",
                table: "TrainingTracks",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_TrainingTrackId",
                table: "Enrollments",
                columns: new[] { "StudentId", "TrainingTrackId" },
                unique: true,
                filter: "[Status] IN (0, 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingTracks_Code",
                table: "TrainingTracks");

            migrationBuilder.DropIndex(
                name: "IX_Students_Email",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_StudentId_TrainingTrackId",
                table: "Enrollments");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTracks_Code",
                table: "TrainingTracks",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments",
                column: "StudentId");
        }
    }
}
