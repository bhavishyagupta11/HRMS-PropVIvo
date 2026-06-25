using HRMS.Core.Postgres.Data;
using IdentityFeature.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityFeature.Infrastructure.Services
{
    public class IdentitySeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public IdentitySeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            // Wait for DB creation/migration if necessary, handled before this by Startup.

            // Seed Roles
            var roles = new[] { "Admin", "HR", "ReportingManager", "Manager", "Employee" };
            foreach (var roleName in roles)
            {
                if (!await context.Set<Role>().AnyAsync(r => r.Name == roleName, cancellationToken))
                {
                    context.Set<Role>().Add(new Role
                    {
                        Id = roleName,
                        Name = roleName,
                        CreatedOn = DateTime.UtcNow
                    });
                }
            }
            await context.SaveChangesAsync(cancellationToken);

            // Helper to seed users
            var usersToSeed = new[]
            {
                new { Email = "admin@hrms.com", FirstName = "System", LastName = "Administrator", Role = "Admin", Password = "Admin@123" },
                new { Email = "employee@hrms.com", FirstName = "Sarah", LastName = "Mitchell", Role = "Employee", Password = "Role@123" },
                new { Email = "manager@hrms.com", FirstName = "Michael", LastName = "Chen", Role = "Manager", Password = "Role@123" },
                new { Email = "repmanager@hrms.com", FirstName = "Arjun", LastName = "Mehta", Role = "ReportingManager", Password = "Role@123" },
                new { Email = "hr@hrms.com", FirstName = "Elena", LastName = "Rostova", Role = "HR", Password = "Role@123" }
            };

            foreach (var u in usersToSeed)
            {
                var roleObj = await context.Set<Role>().FirstOrDefaultAsync(r => r.Name == u.Role, cancellationToken);
                if (roleObj != null)
                {
                    if (!await context.Set<User>().AnyAsync(usr => usr.Email == u.Email, cancellationToken))
                    {
                        var newUser = new User
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = u.Email,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            RoleId = roleObj.Id,
                            CreatedOn = DateTime.UtcNow,
                            UserContext = new HRMS.Shared.Domain.Entity.UserBase
                            {
                                CreatedByUserId = Guid.Empty.ToString()
                            }
                        };
                        newUser.PasswordHash = passwordHasher.HashPassword(newUser, u.Password);
                        context.Set<User>().Add(newUser);
                    }
                }
            }
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
