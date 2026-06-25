namespace OnboardingFeature.Application.DTO
{
    // ON-01: "Dashboard shows my name, designation, department, manager, buddy, and joining date."
    // ON-01: "An overall progress percentage is displayed."
    // ON-06: IsCompleted used to hide the onboarding flow.
    public class OnboardingEmployeeDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string BuddyName { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public int OverallProgressPercent { get; set; }
        public bool IsCompleted { get; set; }
    }

    // ON-01/02: "Tasks are grouped by phase with clear status badges."
    // ON-02: "Each task shows title, description, due date, priority, and assignee."
    // ON-02: "Completed tasks display a completion date."
    public class OnboardingTaskDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Phase { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Assignee { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? CompletedDate { get; set; }
    }

    // ON-03: "Welcome messages display sender name, role, and message."
    // ON-03: "Video welcome messages can be played when available."
    public class WelcomeMessageDto
    {
        public string Id { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderRole { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
    }

    // ON-04: "Relocation status, visa status, accommodation, and travel details are visible."
    // ON-04: "A local buddy contact is provided."
    // ON-04: "I can view open relocation support tickets and their status."
    // PSD Key Capabilities: "visa status, accommodation, travel, allowance, local buddy, and support tickets"
    public class RelocationSupportDto
    {
        public string Id { get; set; } = string.Empty;
        public string RelocationStatus { get; set; } = string.Empty;
        public string VisaStatus { get; set; } = string.Empty;
        public string Accommodation { get; set; } = string.Empty;
        public string TravelDetails { get; set; } = string.Empty;
        public string Allowance { get; set; } = string.Empty;
        public string LocalBuddyContact { get; set; } = string.Empty;
        public List<string> SupportTickets { get; set; } = new();
    }

    // ON-05: "Team member cards show bio, expertise, and fun facts."
    // ON-05: "Introduction and welcome message status is tracked."
    public class TeamIntroductionDto
    {
        public string Id { get; set; } = string.Empty;
        public string TeamMemberName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Expertise { get; set; } = string.Empty;
        public string FunFact { get; set; } = string.Empty;
        public string IntroductionStatus { get; set; } = string.Empty;
    }

    // PSD Key Capabilities: "Onboarding milestones (check-ins, reviews, celebrations) with scheduled dates."
    public class OnboardingMilestoneDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    // PSD Key Capabilities: "Mandatory and optional training modules with progress and certificates."
    // Minimum reference — full entity owned by PSD Section 4.9.
    public class OnboardingTrainingModuleRefDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsMandatory { get; set; }
        public int ProgressPercent { get; set; }
        public bool HasCertificate { get; set; }
    }
}
