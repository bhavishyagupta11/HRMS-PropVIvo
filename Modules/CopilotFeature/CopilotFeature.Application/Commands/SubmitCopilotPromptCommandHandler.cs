using CopilotFeature.Application.DTO;
using CopilotFeature.Application.Repository;
using CopilotFeature.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CopilotFeature.Application.Commands
{
    public class SubmitCopilotPromptCommand : IRequest<CopilotInteractionRecordDto>
    {
        public string PromptText { get; set; } = string.Empty;
    }

    public class SubmitCopilotPromptCommandHandler : IRequestHandler<SubmitCopilotPromptCommand, CopilotInteractionRecordDto>
    {
        private readonly ICopilotRepository _repository;

        public SubmitCopilotPromptCommandHandler(ICopilotRepository repository)
        {
            _repository = repository;
        }

        public async Task<CopilotInteractionRecordDto> Handle(SubmitCopilotPromptCommand request, CancellationToken cancellationToken)
        {
            string prompt = request.PromptText.ToLowerInvariant();
            string response = "I can assist you with HR policies, leave application procedures, expense reimbursements, and benefits guidance. For complex personal inquiries, I can also connect you with our HR specialist team.";
            string category = "General";
            double score = 0.89;

            if (prompt.Contains("leave") || prompt.Contains("vacation") || prompt.Contains("holiday") || prompt.Contains("time off") || prompt.Contains("sick"))
            {
                response = "According to our 2026 Employee Handbook (Section 4.2), full-time employees are entitled to 20 days of paid annual leave, 10 days of sick leave, and standard federal holidays. You can verify your live balance or submit a request directly in the Leave Management tab.";
                category = "Leave & Time Off";
                score = 0.98;
            }
            else if (prompt.Contains("expense") || prompt.Contains("reimburse") || prompt.Contains("receipt") || prompt.Contains("per diem") || prompt.Contains("travel"))
            {
                response = "Our Global Travel & Expense Policy permits full reimbursement for client dining, domestic flights, and accommodations. Receipts are required for all transactions exceeding $25. Standard approval turnaround is 3-5 business days upon submission in the Expenses tab.";
                category = "Expenses & Policy";
                score = 0.96;
            }
            else if (prompt.Contains("payroll") || prompt.Contains("salary") || prompt.Contains("tax") || prompt.Contains("paycheck") || prompt.Contains("w2"))
            {
                response = "Payroll is disbursed bi-weekly on alternating Fridays. Your tax withholding allowances (W-4) and direct deposit accounts can be managed via the Payroll & Compliance module. End-of-year tax statements are automatically published by January 31st.";
                category = "Payroll & Compliance";
                score = 0.95;
            }
            else if (prompt.Contains("remote") || prompt.Contains("wfh") || prompt.Contains("hybrid") || prompt.Contains("work from home"))
            {
                response = "We operate under a flexible hybrid framework. Employees are expected to be present in their designated regional office 2-3 days per week, subject to team agreements. Home office equipment stipends are available for remote setups upon manager approval.";
                category = "Workplace Policy";
                score = 0.94;
            }

            var record = new CopilotInteractionRecord
            {
                Id = Guid.NewGuid().ToString(),
                PromptText = request.PromptText,
                ResponseText = response,
                Category = category,
                ConfidenceScore = score,
                InteractionDate = DateTime.UtcNow
            };

            await _repository.SaveInteractionAsync(record);

            return new CopilotInteractionRecordDto
            {
                Id = record.Id,
                PromptText = record.PromptText,
                ResponseText = record.ResponseText,
                Category = record.Category,
                ConfidenceScore = record.ConfidenceScore,
                InteractionDate = record.InteractionDate
            };
        }
    }
}
