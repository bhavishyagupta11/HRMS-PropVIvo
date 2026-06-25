using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AttendanceFeature.Application.DTO;
using AttendanceFeature.Application.Queries;
using AttendanceFeature.Domain;
using AttendanceFeature.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace HRMS.UnitTests
{
    public class QueryTests
    {
        [Fact]
        public async Task GetMyAttendanceQueryHandler_ReturnsCorrectMonthlyAttendanceRecords()
        {
            // Arrange
            var repositoryMock = new Mock<IAttendanceRepository>();
            var sampleRecords = new List<AttendanceRecord>
            {
                new AttendanceRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = "USR-101",
                    Date = new DateTime(2026, 6, 24),
                    ClockInTime = new DateTime(2026, 6, 24, 9, 0, 0),
                    ClockOutTime = new DateTime(2026, 6, 24, 17, 30, 0),
                    Status = "Present",
                    TotalHours = 8.5
                }
            };

            repositoryMock.Setup(repo => repo.GetMonthlyRecordsAsync("USR-101", 6, 2026))
                .ReturnsAsync(sampleRecords);

            var handler = new GetMyAttendanceQueryHandler(repositoryMock.Object);
            var query = new GetMyAttendanceQuery
            {
                UserId = "USR-101",
                Input = new GetMyAttendanceInput { Month = 6, Year = 2026 }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].UserId.Should().Be("USR-101");
            result[0].Status.Should().Be("Present");
            result[0].TotalHours.Should().Be(8.5);
        }
    }
}
