/**
 * HR Copilot API Route — PSD Section 4.15
 * "Backed by an API route for handling assistant requests"
 * Context-aware: currentView, userRole, isOnboarding
 */

export async function POST(request) {
  try {
    const body = await request.json();
    const { message, currentView, userRole, isOnboarding } = body;

    if (!message || typeof message !== 'string') {
      return Response.json({ error: 'Message is required' }, { status: 400 });
    }

    // Build context-aware system prompt per PSD Section 4.15
    const contextPrompt = [
      `You are WorkFlow HR Copilot, an embedded AI assistant for the Global HRMS platform.`,
      `Current user role: ${userRole || 'Employee'}`,
      `Current view/module: ${currentView || 'Dashboard'}`,
      isOnboarding ? `User is currently in the onboarding flow. Provide extra guidance for new joiners.` : '',
      ``,
      `Provide helpful, concise, accurate answers about HR policies, workflows, and the HRMS platform.`,
      `Always be professional and empathetic.`,
    ].filter(Boolean).join('\n');

    // Mock intelligent responses per PSD context (production would call an LLM)
    const response = generateContextualResponse(message.toLowerCase(), currentView, userRole, isOnboarding);

    return Response.json({
      reply: response,
      context: { currentView, userRole, isOnboarding },
    });

  } catch (error) {
    return Response.json(
      { error: 'Failed to process request. Please try again.' },
      { status: 500 }
    );
  }
}

/**
 * Context-aware response generation
 * PSD: "Responses reflect the user's current context and role"
 */
function generateContextualResponse(message, currentView, userRole, isOnboarding) {
  // Onboarding context — PSD CP-02
  if (isOnboarding) {
    if (message.includes('task') || message.includes('complete') || message.includes('onboard')) {
      return "As a new joiner, you can see all your onboarding tasks grouped by phase (Pre-joining, Day 1, Week 1, etc.). Click 'Mark Complete' on any task that you've finished. Your progress is tracked as a percentage at the top of the dashboard. If you're unsure about a task, I'm here to help!";
    }
    if (message.includes('document') || message.includes('upload')) {
      return "For onboarding, you'll need to upload several documents including your ID proof, offer letter acceptance, and tax forms. Go to the Documents section from the onboarding task list. Make sure files are clear and legible — HR will verify them within 2 business days.";
    }
    return "Welcome aboard! I'm your HR Copilot. I can help you navigate your onboarding journey, answer questions about documents, policies, or next steps. What would you like to know?";
  }

  // Leave queries
  if (message.includes('leave') || message.includes('vacation') || message.includes('time off')) {
    if (message.includes('balance') || message.includes('how many') || message.includes('available')) {
      return `You currently have the following leave balances:\n• Casual Leave: 7 days available\n• Sick Leave: 8 days available\n• Personal Leave: 4 days available\n• Comp-off: 2 days available\n\nTo request leave, go to the Leave module and click 'Apply for Leave'. Your request will be routed to your manager for approval.`;
    }
    if (message.includes('apply') || message.includes('submit') || message.includes('request')) {
      return "To submit a leave request:\n1. Navigate to the Leave module\n2. Click 'Apply for Leave' tab\n3. Select the leave type and verify your available balance\n4. Choose your start and end dates\n5. Add a reason for the leave\n6. Submit — your manager will be notified for approval\n\nYou'll receive a notification once your request is reviewed.";
    }
    return "WorkFlow supports the following leave types: Casual, Sick, Personal, Maternity, Paternity, Leave Without Pay, and Comp-off. You can view balances, apply for leave, and track approval status in the Leave module.";
  }

  // Expense queries
  if (message.includes('expense') || message.includes('reimburse') || message.includes('claim')) {
    return "To submit an expense claim:\n1. Go to Expenses module\n2. Click 'Submit New Claim'\n3. Select the category (Travel, Food, Accommodation, etc.)\n4. Enter amount, currency, date, and description\n5. Attach receipts (required for amounts over ₹5,000)\n6. For travel claims, you can enter mileage details\n\nClaims go through manager approval before reimbursement is processed. Status: Draft → Submitted → Approved → Paid.";
  }

  // Payroll queries
  if (message.includes('payslip') || message.includes('salary') || message.includes('payroll') || message.includes('pay')) {
    return "Your payslip is available in the Payroll module. It shows:\n• Earnings: Basic, HRA, Special Allowance, Bonus, Overtime\n• Deductions: PF, Income Tax, Professional Tax, ESI, Health Insurance\n• Employer Contributions: PF, ESI, Gratuity\n• Net Pay after all deductions\n\nYou can download your payslip as PDF. Tax documents like Form 16 are also available there.";
  }

  // Performance queries
  if (message.includes('performance') || message.includes('review') || message.includes('goal') || message.includes('okr')) {
    return "Your performance is tracked through Goals & OKRs in the Performance module. Each goal has:\n• Key Results with targets and current progress\n• Status tracking (Not Started, In Progress, On Track, At Risk, Completed)\n• Weightage contributing to overall performance\n\nQuarterly and annual reviews include category ratings, strengths, and improvement areas. You can also view feedback from peers and your manager.";
  }

  // Attendance queries
  if (message.includes('attendance') || message.includes('clock') || message.includes('check-in')) {
    return "To clock in, go to the Attendance module and click 'Clock In Now'. You can verify using:\n• Selfie capture\n• Geolocation\n• IP-based verification\n• Biometric (if configured)\n• Manual entry\n\nYour total, productive, break, and overtime hours are tracked automatically. Your shift schedule is also visible in the Attendance module.";
  }

  // Training queries
  if (message.includes('training') || message.includes('course') || message.includes('certificate') || message.includes('learn')) {
    return "Your training modules are in the Training section. Mandatory modules have a due date and must be completed on time. Module types include:\n• Videos\n• Documents to read\n• Interactive exercises\n• Quizzes\n\nCompleting certificate-eligible modules earns you a digital certificate. Your progress is saved automatically.";
  }

  // Recognition queries
  if (message.includes('recognition') || message.includes('appreciate') || message.includes('thank')) {
    return "You can recognize a colleague's great work in the Recognition module:\n1. Click 'Send Recognition'\n2. Select the recipient from your colleagues\n3. Choose a category: Excellence, Team Player, Innovation, Leadership, or Customer Focus\n4. Write a personal message\n5. Choose public or private visibility\n\nRecognitions appear in the company-wide recognition feed where everyone can like and comment.";
  }

  // Role-specific responses
  if (userRole === 'Manager') {
    if (message.includes('team') || message.includes('report')) {
      return "As a Manager, you can view your team in the Team module. It shows all your direct reports with their designation, department, status, and reporting relationships. You also have access to team-level analytics and can approve leave requests and expense claims from your team.";
    }
    if (message.includes('approve') || message.includes('approval')) {
      return "Approvals are pending in your queue:\n• Leave Requests: Check the Leave module → Approval Queue tab\n• Expense Claims: Check the Expenses module → Approval Queue tab\n• Contributions: Check the Contributions module → Approval Queue tab\n\nYou can approve or reject with optional comments.";
    }
  }

  if (userRole === 'HR') {
    if (message.includes('recruit') || message.includes('hire') || message.includes('candidate')) {
      return "The Recruitment module gives you full access to job postings and the candidate pipeline. You can:\n• View all active job postings with applicant counts\n• Track candidates through stages: New → Screening → Shortlisted → Interview → Offer → Hired\n• View detailed candidate profiles with skills and experience\n• Schedule interviews and update pipeline status";
    }
  }

  // Module navigation help
  if (message.includes('navigate') || message.includes('where') || message.includes('find') || message.includes('go to')) {
    return `In WorkFlow, you can navigate using:\n• **Desktop**: Sidebar on the left shows all modules available for your role (${userRole})\n• **Mobile**: Bottom navigation bar with your role-specific quick access tabs\n\nAll modules: Dashboard, Onboarding, Attendance, Leave, Payroll, Documents, Expenses, Performance, Contributions, Training, Recognition, Announcements, Analytics, and HR Copilot.`;
  }

  // Default helpful response
  return `Hi! I'm your WorkFlow HR Copilot. I can help you with:\n• Leave balances and requests\n• Expense submissions\n• Payslip and payroll questions\n• Performance goals and reviews\n• Training modules and certificates\n• Attendance and clock-in procedures\n• Recognition and contributions\n• Company announcements\n\nYou're currently viewing: **${currentView || 'Dashboard'}** as **${userRole || 'Employee'}**. What can I help you with today?`;
}
