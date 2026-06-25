using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceFeature.Domain.Repositories
{
    public interface IAttendanceRepository
    {
        Task<AttendanceRecord?> GetTodayRecordAsync(string userId, DateTime today);
        Task<List<AttendanceRecord>> GetMonthlyRecordsAsync(string userId, int month, int year);
        Task<List<AttendanceRecord>> GetTeamRecordsAsync(DateTime date);
        Task AddAsync(AttendanceRecord record);
        Task UpdateAsync(AttendanceRecord record);
    }
}
