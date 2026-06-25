using ExpenseFeature.Application.DTO;
using ExpenseFeature.Application.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFeature.Application.Commands
{
    public class ProcessExpenseCommand : IRequest<ReimbursementRecordDto>
    {
        public ProcessExpenseDto Payload { get; set; } = new();
    }

    public class ProcessExpenseCommandHandler : IRequestHandler<ProcessExpenseCommand, ReimbursementRecordDto>
    {
        private readonly IExpenseRepository _repository;

        public ProcessExpenseCommandHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReimbursementRecordDto> Handle(ProcessExpenseCommand request, CancellationToken cancellationToken)
        {
            var record = await _repository.GetByIdAsync(request.Payload.ExpenseId, cancellationToken);
            if (record == null)
            {
                throw new Exception("Expense claim not found.");
            }

            record.Status = request.Payload.Status;
            record.ApprovalComments = request.Payload.ApprovalComments;

            if (request.Payload.Status.Equals("paid", StringComparison.OrdinalIgnoreCase))
            {
                record.PaidDate = DateTime.UtcNow;
            }

            var updated = await _repository.UpdateAsync(record, cancellationToken);

            return new ReimbursementRecordDto
            {
                Id = updated.Id,
                UserId = updated.UserId,
                Category = updated.Category,
                Amount = updated.Amount,
                Currency = updated.Currency,
                Description = updated.Description,
                ExpenseDate = updated.ExpenseDate,
                ReceiptUrl = updated.ReceiptUrl,
                Mileage = updated.Mileage,
                IsTaxable = updated.IsTaxable,
                Status = updated.Status,
                ApprovalComments = updated.ApprovalComments,
                PaidDate = updated.PaidDate
            };
        }
    }
}
