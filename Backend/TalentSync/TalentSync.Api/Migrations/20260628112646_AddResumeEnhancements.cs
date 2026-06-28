using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSync.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Users_CandidateId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_CandidateId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_CreatedAt",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_Status",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Resumes");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Resumes",
                newName: "FileUrl");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadedDate",
                table: "Resumes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Resumes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Resumes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Resumes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Resumes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Resumes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_CandidateId",
                table: "Resumes",
                column: "CandidateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_PublicId",
                table: "Resumes",
                column: "PublicId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Users_CandidateId",
                table: "Resumes",
                column: "CandidateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Users_CandidateId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_CandidateId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_PublicId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Resumes");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "Resumes",
                newName: "FilePath");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadedDate",
                table: "Resumes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Resumes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Resumes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_CandidateId",
                table: "Resumes",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_CreatedAt",
                table: "Resumes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_Status",
                table: "Resumes",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Users_CandidateId",
                table: "Resumes",
                column: "CandidateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
