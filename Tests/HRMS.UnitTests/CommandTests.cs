using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AttendanceFeature.Application.Commands;
using AttendanceFeature.Application.DTO;
using AttendanceFeature.Domain;
using AttendanceFeature.Domain.Repositories;
using CopilotFeature.Application.Commands;
using CopilotFeature.Application.Repository;
using CopilotFeature.Domain;
using FluentAssertions;
using LeaveFeature.Application.Commands;
using LeaveFeature.Application.DTO;
using LeaveFeature.Application.Repository;
using LeaveFeature.Domain;
using Microsoft.AspNetCore.Http;
using Moq;
using TrainingFeature.Application.Commands;
using TrainingFeature.Application.Repository;
using TrainingFeature.Domain;
using Xunit;

namespace HRMS.UnitTests
{
    public class CommandTests
    {
        [Fact]
        public async Task ClockInCommandHandler_WhenNotAlreadyClockedIn_SuccessfullyAddsAttendanceRecord()
        {
            // Arrange
            var repositoryMock = new Mock<IAttendanceRepository>();
            repositoryMock.Setup(repo => repo.GetTodayRecordAsync("USR-101", It.IsAny<DateTime>()))
                .ReturnsAsync((AttendanceRecord?)null);

            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<AttendanceRecord>()))
                .Returns(Task.CompletedTask);

            var handler = new ClockInCommandHandler(repositoryMock.Object);
            var command = new ClockInCommand
            {
                UserId = "USR-101",
                Input = new ClockInInput
                {
                    ClockInMethod = "Web",
                    LocationVerified = true,
                    IpValidated = true,
                    ShiftName = "General Shift",
                    ShiftStartTime = TimeSpan.FromHours(9),
                    ShiftEndTime = TimeSpan.FromHours(18)
                }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            repositoryMock.Verify(repo => repo.AddAsync(It.Is<AttendanceRecord>(r => r.UserId == "USR-101" && r.Status == "Present")), Times.Once);
        }

        [Fact]
        public async Task ClockInCommandHandler_WhenAlreadyClockedIn_ThrowsInvalidOperationException()
        {
            // Arrange
            var repositoryMock = new Mock<IAttendanceRepository>();
            repositoryMock.Setup(repo => repo.GetTodayRecordAsync("USR-101", It.IsAny<DateTime>()))
                .ReturnsAsync(new AttendanceRecord { UserId = "USR-101" });

            var handler = new ClockInCommandHandler(repositoryMock.Object);
            var command = new ClockInCommand { UserId = "USR-101", Input = new ClockInInput() };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task SubmitLeaveRequestCommandHandler_WithSufficientBalance_CreatesLeaveRequest()
        {
            // Arrange
            var leaveRepoMock = new Mock<ILeaveRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var claims = new List<Claim> { new Claim("sub", "USR-101") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var defaultHttpContext = new DefaultHttpContext { User = claimsPrincipal };
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(defaultHttpContext);

            var leaveBalance = new LeaveBalance
            {
                UserId = "USR-101",
                LeaveType = "Annual",
                Available = 15,
                Pending = 0
            };

            leaveRepoMock.Setup(repo => repo.GetLeaveBalanceAsync("USR-101", "Annual"))
                .ReturnsAsync(leaveBalance);

            leaveRepoMock.Setup(repo => repo.GetLeaveBalancesAsync("USR-101"))
                .ReturnsAsync(new List<LeaveBalance> { leaveBalance });

            leaveRepoMock.Setup(repo => repo.UpdateLeaveBalanceAsync(It.IsAny<LeaveBalance>()))
                .Returns(Task.CompletedTask);

            leaveRepoMock.Setup(repo => repo.CreateLeaveRequestAsync(It.IsAny<LeaveRequest>()))
                .Returns(Task.CompletedTask);

            var handler = new SubmitLeaveRequestCommandHandler(leaveRepoMock.Object, httpContextAccessorMock.Object);
            var command = new SubmitLeaveRequestCommand
            {
                Payload = new SubmitLeaveRequestDto
                {
                    LeaveType = "Annual",
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(3),
                    TotalDays = 3,
                    Reason = "Family Vacation"
                }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be("USR-101");
            result.LeaveType.Should().Be("Annual");
            result.TotalDays.Should().Be(3);
            result.Status.Should().Be("Pending");
            leaveBalance.Available.Should().Be(12);
            leaveBalance.Pending.Should().Be(3);
        }

        [Fact]
        public async Task SubmitCopilotPromptCommandHandler_WithLeaveInquiry_ReturnsLeavePolicyAnswer()
        {
            // Arrange
            var copilotRepoMock = new Mock<ICopilotRepository>();
            copilotRepoMock.Setup(repo => repo.SaveInteractionAsync(It.IsAny<CopilotInteractionRecord>()))
                .Returns(Task.CompletedTask);

            var handler = new SubmitCopilotPromptCommandHandler(copilotRepoMock.Object);
            var command = new SubmitCopilotPromptCommand { PromptText = "What is the policy for annual leave?" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Category.Should().Be("Leave & Time Off");
            result.ResponseText.Should().Contain("20 days of paid annual leave");
        }

        [Fact]
        public async Task EnrollInCourseCommandHandler_WithValidCourseId_SuccessfullyEnrollsUser()
        {
            // Arrange
            var trainingRepoMock = new Mock<ITrainingRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var claims = new List<Claim> { new Claim("sub", "USR-101") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var defaultHttpContext = new DefaultHttpContext { User = claimsPrincipal };
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(defaultHttpContext);

            trainingRepoMock.Setup(repo => repo.EnrollAsync(It.IsAny<CourseEnrollmentRecord>()))
                .Returns(Task.CompletedTask);

            var handler = new EnrollInCourseCommandHandler(trainingRepoMock.Object, httpContextAccessorMock.Object);
            var command = new EnrollInCourseCommand { CourseId = "CRS-2026-AI" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CourseId.Should().Be("CRS-2026-AI");
            result.UserId.Should().Be("USR-101");
            result.Status.Should().Be("Enrolled");
        }
    }
}
