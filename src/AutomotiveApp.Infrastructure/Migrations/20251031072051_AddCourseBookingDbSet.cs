using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomotiveApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseBookingDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseBooking_AspNetUsers_UserId",
                table: "CourseBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseBooking_CourseSessions_SessionId",
                table: "CourseBooking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseBooking",
                table: "CourseBooking");

            migrationBuilder.RenameTable(
                name: "CourseBooking",
                newName: "CourseBookings");

            migrationBuilder.RenameIndex(
                name: "IX_CourseBooking_UserId",
                table: "CourseBookings",
                newName: "IX_CourseBookings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseBooking_SessionId",
                table: "CourseBookings",
                newName: "IX_CourseBookings_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseBookings",
                table: "CourseBookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseBookings_AspNetUsers_UserId",
                table: "CourseBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseBookings_CourseSessions_SessionId",
                table: "CourseBookings",
                column: "SessionId",
                principalTable: "CourseSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseBookings_AspNetUsers_UserId",
                table: "CourseBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseBookings_CourseSessions_SessionId",
                table: "CourseBookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseBookings",
                table: "CourseBookings");

            migrationBuilder.RenameTable(
                name: "CourseBookings",
                newName: "CourseBooking");

            migrationBuilder.RenameIndex(
                name: "IX_CourseBookings_UserId",
                table: "CourseBooking",
                newName: "IX_CourseBooking_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseBookings_SessionId",
                table: "CourseBooking",
                newName: "IX_CourseBooking_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseBooking",
                table: "CourseBooking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseBooking_AspNetUsers_UserId",
                table: "CourseBooking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseBooking_CourseSessions_SessionId",
                table: "CourseBooking",
                column: "SessionId",
                principalTable: "CourseSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
