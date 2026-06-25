using HRMS.Core.Postgres.Common;
using HRMS.Shared.Domain.Entity;

namespace OnboardingFeature.Domain
{
    // PSD Section 4.1 — Core Data Entity: OnboardingEmployee
    // Fields derived from ON-01 acceptance criteria:
    //   "Dashboard shows my name, designation, department, manager, buddy, and joining date."
    //   "An overall progress percentage is displayed."
    // ON-06: "Completing onboarding hides the onboarding flow."
    public class OnboardingEmployee : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string BuddyName { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public int OverallProgressPercent { get; set; }
        public bool IsCompleted { get; set; }
        public UserBase? UserContext { get; set; }
    }

    // PSD Section 4.1 — Core Data Entity: OnboardingTask
    // Fields derived from ON-01/02 acceptance criteria:
    //   "Tasks are grouped by phase with clear status badges."
    //   "Each task shows title, description, due date, priority, and assignee."
    //   "I can mark employee-assigned tasks as complete."
    //   "Completed tasks display a completion date."
    public class OnboardingTask : BaseEntity
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // PSD Key Capabilities: "pre-joining, day-1, week-1, week-2, month-1"
        public string Phase { get; set; } = string.Empty;
        // PSD Key Capabilities: task has "priority"
        public string Priority { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Assignee { get; set; } = string.Empty;
        // ON-01: "status badges" — ON-02: "mark as complete"
        public string Status { get; set; } = string.Empty;
        // ON-02: "Completed tasks display a completion date."
        public DateTime? CompletedDate { get; set; }
        public UserBase? UserContext { get; set; }
    }

    // PSD Section 4.1 — Core Data Entity: WelcomeMessage
    // Fields derived from ON-03 acceptance criteria:
    //   "Welcome messages display sender name, role, and message."
    //   "Video welcome messages can be played when available."
    // PSD Key Capabilities: "from CEO, manager, buddy, HR, and team, including optional video"
    public class WelcomeMessage : BaseEntity
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderRole { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public UserBase? UserContext { get; set; }
    }

    // PSD Section 4.1 — Core Data Entity: TrainingModule
    // Minimum reference only — full entity belongs to PSD Section 4.9 (Training & Learning).
    // Fields here are limited to what ON-01/02 requires:
    //   PSD Key Capabilities: "Mandatory and optional training modules with progress and certificates."
    public class OnboardingTrainingModuleRef : BaseEntity
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsMandatory { get; set; }
        public int ProgressPercent { get; set; }
        public bool HasCertificate { get; set; }
        public UserBase? UserContext { get; set; }
    }

    // PSD Section 4.1 — Core Data Entity: RelocationSupport
    // Fields derived from ON-04 acceptance criteria:
    //   "Relocation status, visa status, accommodation, and travel details are visible."
    //   "A local buddy contact is provided."
    //   "I can view open relocation support tickets and their status."
    // PSD Key Capabilities: "visa status, accommodation, travel, allowance, local buddy, and support tickets"
    public class RelocationSupport : BaseEntity
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
        public string RelocationStatus { get; set; } = string.Empty;
        public string VisaStatus { get; set; } = string.Empty;
        public string Accommodation { get; set; } = string.Empty;
        public string TravelDetails { get; set; } = string.Empty;
        public string Allowance { get; set; } = string.Empty;
        public string LocalBuddyContact { get; set; } = string.Empty;
        // ON-04: "open relocation support tickets and their status" — stored as JSON list
        public List<string> SupportTickets { get; set; } = new();
        public UserBase? UserContext { get; set; }
    }

    // PSD Section 4.1 — Core Data Entity: TeamIntroduction
    // Fields derived from ON-05 acceptance criteria:
    //   "Team member cards show bio, expertise, and fun facts."
    //   "Introduction and welcome message status is tracked."
    public class TeamIntroduction : BaseEntity
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
        public string TeamMemberName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Expertise { get; set; } = string.Empty;
        public string FunFact { get; set; } = string.Empty;
        // ON-05: "Introduction and welcome message status is tracked."
        public string IntroductionStatus { get; set; } = string.Empty;
        public UserBase? UserContext { get; set; }
    }

    // PSD Section 4.1 — Core Data Entity: OnboardingMilestone
    // PSD Key Capabilities: "Onboarding milestones (check-ins, reviews, celebrations) with scheduled dates."
    public class OnboardingMilestone : BaseEntity
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        // PSD Key Capabilities: "check-ins, reviews, celebrations"
        public string Type { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public bool IsCompleted { get; set; }
        public UserBase? UserContext { get; set; }
    }
}
