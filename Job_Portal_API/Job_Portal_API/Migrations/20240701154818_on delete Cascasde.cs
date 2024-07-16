using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Portal_API.Migrations
{
    public partial class ondeleteCascasde : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerID",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerID",
                table: "Applications",
                column: "JobSeekerID",
                principalTable: "JobSeekers",
                principalColumn: "JobSeekerID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerID",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerID",
                table: "Applications",
                column: "JobSeekerID",
                principalTable: "JobSeekers",
                principalColumn: "JobSeekerID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
