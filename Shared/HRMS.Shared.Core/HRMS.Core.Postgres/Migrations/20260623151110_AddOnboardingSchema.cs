using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Core.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddOnboardingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnboardingEmployees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Designation = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Department = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ManagerName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    BuddyName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OverallProgressPercent = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_OnboardingEmployees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingMilestones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OnboardingEmployeeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_OnboardingMilestones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingTasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OnboardingEmployeeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Phase = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Priority = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Assignee = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_OnboardingTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingTrainingModuleRefs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OnboardingEmployeeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    ProgressPercent = table.Column<int>(type: "integer", nullable: false),
                    HasCertificate = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_OnboardingTrainingModuleRefs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelocationSupports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OnboardingEmployeeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RelocationStatus = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    VisaStatus = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Accommodation = table.Column<string>(type: "text", nullable: false),
                    TravelDetails = table.Column<string>(type: "text", nullable: false),
                    Allowance = table.Column<string>(type: "text", nullable: false),
                    LocalBuddyContact = table.Column<string>(type: "text", nullable: false),
                    SupportTickets = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_RelocationSupports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamIntroductions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OnboardingEmployeeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TeamMemberName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: false),
                    Expertise = table.Column<string>(type: "text", nullable: false),
                    FunFact = table.Column<string>(type: "text", nullable: false),
                    IntroductionStatus = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_TeamIntroductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WelcomeMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OnboardingEmployeeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SenderName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SenderRole = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_WelcomeMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingEmployees_UserId",
                table: "OnboardingEmployees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingMilestones_OnboardingEmployeeId",
                table: "OnboardingMilestones",
                column: "OnboardingEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingTasks_OnboardingEmployeeId",
                table: "OnboardingTasks",
                column: "OnboardingEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingTrainingModuleRefs_OnboardingEmployeeId",
                table: "OnboardingTrainingModuleRefs",
                column: "OnboardingEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RelocationSupports_OnboardingEmployeeId",
                table: "RelocationSupports",
                column: "OnboardingEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamIntroductions_OnboardingEmployeeId",
                table: "TeamIntroductions",
                column: "OnboardingEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WelcomeMessages_OnboardingEmployeeId",
                table: "WelcomeMessages",
                column: "OnboardingEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnboardingEmployees");

            migrationBuilder.DropTable(
                name: "OnboardingMilestones");

            migrationBuilder.DropTable(
                name: "OnboardingTasks");

            migrationBuilder.DropTable(
                name: "OnboardingTrainingModuleRefs");

            migrationBuilder.DropTable(
                name: "RelocationSupports");

            migrationBuilder.DropTable(
                name: "TeamIntroductions");

            migrationBuilder.DropTable(
                name: "WelcomeMessages");
        }
    }
}
