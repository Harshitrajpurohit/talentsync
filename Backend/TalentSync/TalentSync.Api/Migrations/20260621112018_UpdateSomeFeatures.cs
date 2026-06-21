using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSync.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSomeFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screenings_Users_RecruiterId",
                table: "Screenings");

            migrationBuilder.RenameColumn(
                name: "RecruiterId",
                table: "Screenings",
                newName: "ScreenedById");

            migrationBuilder.RenameIndex(
                name: "IX_Screenings_RecruiterId",
                table: "Screenings",
                newName: "IX_Screenings_ScreenedById");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "Submitted",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "Pending");

            migrationBuilder.AddForeignKey(
                name: "FK_Screenings_Users_ScreenedById",
                table: "Screenings",
                column: "ScreenedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screenings_Users_ScreenedById",
                table: "Screenings");

            migrationBuilder.RenameColumn(
                name: "ScreenedById",
                table: "Screenings",
                newName: "RecruiterId");

            migrationBuilder.RenameIndex(
                name: "IX_Screenings_ScreenedById",
                table: "Screenings",
                newName: "IX_Screenings_RecruiterId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "Submitted");

            migrationBuilder.AddForeignKey(
                name: "FK_Screenings_Users_RecruiterId",
                table: "Screenings",
                column: "RecruiterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
