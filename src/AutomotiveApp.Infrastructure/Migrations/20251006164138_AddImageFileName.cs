using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomotiveApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "PaymentMethods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "CourseCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "CourseCategories");
        }
    }
}
