using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Core.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClockInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClockOutTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClockInMethod = table.Column<string>(type: "text", nullable: false),
                    LocationVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IpValidated = table.Column<bool>(type: "boolean", nullable: false),
                    SelfieUrl = table.Column<string>(type: "text", nullable: true),
                    TotalHours = table.Column<double>(type: "double precision", nullable: false),
                    ProductiveHours = table.Column<double>(type: "double precision", nullable: false),
                    BreakHours = table.Column<double>(type: "double precision", nullable: false),
                    OvertimeHours = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ShiftName = table.Column<string>(type: "text", nullable: false),
                    ShiftStartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ShiftEndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    UserContext_CreatedByUserId = table.Column<string>(type: "text", nullable: true),
                    UserContext_CreatedByUserName = table.Column<string>(type: "text", nullable: true),
                    UserContext_CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserContext_ModifiedByUserId = table.Column<string>(type: "text", nullable: true),
                    UserContext_ModifiedByUserName = table.Column<string>(type: "text", nullable: true),
                    UserContext_ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserContext_ProfilePicture = table.Column<string>(type: "text", nullable: true),
                    DocumentType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CreatedByUserId = table.Column<string>(type: "text", nullable: true),
                    CreatedByUserName = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedByUserId = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserName = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProfilePicture = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_UserId",
                table: "AttendanceRecords",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceRecords");
        }
    }
}
