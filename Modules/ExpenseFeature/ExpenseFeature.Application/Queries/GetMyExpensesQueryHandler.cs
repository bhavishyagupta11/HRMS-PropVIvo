using ExpenseFeature.Application.DTO;
using ExpenseFeature.Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFeature.Application.Queries
{
    public class GetMyExpensesQuery : IRequest<IEnumerable<ReimbursementRecordDto>> { }

    public class GetMyExpensesQueryHandler : IRequestHandler<GetMyExpensesQuery, IEnumerable<ReimbursementRecordDto>>
    {
        private readonly IExpenseRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyExpensesQueryHandler(IExpenseRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ReimbursementRecordDto>> Handle(GetMyExpensesQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value 
                ?? string.Empty;
            if (string.IsNullOrEmpty(userId))
            {
                userId = "EMP12345";
            }

            var records = await _repository.GetByUserIdAsync(userId, cancellationToken);

            return records.Select(r => new ReimbursementRecordDto
            {
                Id = r.Id,
                UserId = r.UserId,
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
