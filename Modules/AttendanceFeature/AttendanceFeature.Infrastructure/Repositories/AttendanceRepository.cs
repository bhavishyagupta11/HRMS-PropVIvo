using AttendanceFeature.Domain;
using AttendanceFeature.Domain.Repositories;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceFeature.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly PostgresDbContext _context;

        public AttendanceRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<AttendanceRecord?> GetTodayRecordAsync(string userId, DateTime today)
        {
            return await _context.Set<AttendanceRecord>()
                .Where(r => r.UserId == userId && r.Date.Date == today.Date)
                .OrderByDescending(r => r.ClockInTime)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AttendanceRecord>> GetMonthlyRecordsAsync(string userId, int month, int year)
        {
            return await _context.Set<AttendanceRecord>()
                .Where(r => r.UserId == userId && r.Date.Month == month && r.Date.Year == year)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<List<AttendanceRecord>> GetTeamRecordsAsync(DateTime date)
        {
            return await _context.Set<AttendanceRecord>()
                .Where(r => r.Date.Date == date.Date)
                .OrderByDescending(r => r.ClockInTime)
                .ToListAsync();
        }

        public async Task AddAsync(AttendanceRecord record)
        {
            _context.Set<AttendanceRecord>().Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AttendanceRecord record)
        {
            _context.Set<AttendanceRecord>().Update(record);
            await _context.SaveChangesAsync();
        }
    }
}
