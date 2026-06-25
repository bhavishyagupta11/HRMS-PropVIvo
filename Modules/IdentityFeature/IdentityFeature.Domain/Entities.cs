using System;
using System.Collections.Generic;
using HRMS.Core.Postgres.Common;
using HRMS.Shared.Domain.Entity;

namespace IdentityFeature.Domain
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>();
        public UserBase? UserContext { get; set; }
    }

    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public Role? Role { get; set; }
        public UserBase? UserContext { get; set; }
    }
}
