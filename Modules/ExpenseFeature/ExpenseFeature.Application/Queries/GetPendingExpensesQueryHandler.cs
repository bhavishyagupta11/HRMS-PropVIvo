using ExpenseFeature.Application.DTO;
using ExpenseFeature.Application.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFeature.Application.Queries
{
    public class GetPendingExpensesQuery : IRequest<IEnumerable<ReimbursementRecordDto>> { }

    public class GetPendingExpensesQueryHandler : IRequestHandler<GetPendingExpensesQuery, IEnumerable<ReimbursementRecordDto>>
    {
        private readonly IExpenseRepository _repository;

        public GetPendingExpensesQueryHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ReimbursementRecordDto>> Handle(GetPendingExpensesQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetPendingExpensesAsync(cancellationToken);

            return records.Select(r => new ReimbursementRecordDto
            {
                Id = r.Id,
                UserId = r.UserId,
                EmployeeName = "Team Member (" + r.UserId.Substring(0, Math.Min(4, r.UserId.Length)) + ")",
                Category = r.Category,
                Amount = r.Amount,
                Currency = r.Currency,
                Description = r.Description,
                ExpenseDate = r.ExpenseDate,
                ReceiptUrl = r.ReceiptUrl,
                Mileage = r.Mileage,
                IsTaxable = r.IsTaxable,
                Status = r.Status,
                ApprovalComments = r.ApprovalComments,
                PaidDate = r.PaidDate
            });
        }
    }
}
