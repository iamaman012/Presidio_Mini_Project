using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Portal_API.Migrations
{
    public partial class changeJobListinng : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyDescription",
                table: "JobListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLocation",
                table: "JobListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "JobListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyDescription",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "CompanyLocation",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "JobListings");
        }
    }
}
