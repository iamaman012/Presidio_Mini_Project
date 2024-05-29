using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Portal_API.Migrations
{
    public partial class AddGpa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GPA",
                table: "Educations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GPA",
                table: "Educations");
        }
    }
}
