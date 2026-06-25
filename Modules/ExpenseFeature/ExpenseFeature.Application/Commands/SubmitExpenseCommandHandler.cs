using ExpenseFeature.Application.DTO;
using ExpenseFeature.Application.Repository;
using ExpenseFeature.Domain;
using HRMS.Shared.Application.Services;
using HRMS.Shared.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFeature.Application.Commands
{
    public class SubmitExpenseCommand : IRequest<ReimbursementRecordDto>
    {
        public SubmitExpenseDto Payload { get; set; } = new();
    }

    public class SubmitExpenseCommandHandler : IRequestHandler<SubmitExpenseCommand, ReimbursementRecordDto>
    {
        private readonly IExpenseRepository _repository;
        private readonly IAzureStorage _azureStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmitExpenseCommandHandler(IExpenseRepository repository, IAzureStorage azureStorage, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _azureStorage = azureStorage;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ReimbursementRecordDto> Handle(SubmitExpenseCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value 
                ?? string.Empty;
            if (string.IsNullOrEmpty(userId))
            {
                userId = "EMP12345";
            }

            string? receiptUrl = null;
            if (!string.IsNullOrEmpty(request.Payload.ReceiptBase64Content) && !string.IsNullOrEmpty(request.Payload.ReceiptFileName))
            {
                var fileBytes = Convert.FromBase64String(request.Payload.ReceiptBase64Content);
                var folderName = userId;
                var fileName = $"{Guid.NewGuid()}_{request.Payload.ReceiptFileName}";

                var uploadResult = await _azureStorage.UploadAsync(
                    BlobContainerNames.expenses,
                    folderName,
                    fileName,
                    fileBytes,
                    request.Payload.ReceiptFileType ?? "application/octet-stream");

                receiptUrl = uploadResult.Blob.Uri ?? $"https://storage.hrms.local/expenses/{folderName}/{fileName}";
            }

            var record = new ReimbursementRecord
            {
                UserId = userId,
                Category = request.Payload.Category,
                Amount = request.Payload.Amount,
                Currency = request.Payload.Currency,
                Description = request.Payload.Description,
                ExpenseDate = request.Payload.ExpenseDate,
                ReceiptUrl = receiptUrl,
                Mileage = request.Payload.Mileage,
                IsTaxable = request.Payload.IsTaxable,
                Status = "Submitted"
            };

            var saved = await _repository.AddAsync(record, cancellationToken);

            return new ReimbursementRecordDto
            {
                Id = saved.Id,
                UserId = saved.UserId,
                Category = saved.Category,
                Amount = saved.Amount,
                Currency = saved.Currency,
                Description = saved.Description,
                ExpenseDate = saved.ExpenseDate,
                ReceiptUrl = saved.ReceiptUrl,
                Mileage = saved.Mileage,
                IsTaxable = saved.IsTaxable,
                Status = saved.Status,
                ApprovalComments = saved.ApprovalComments,
                PaidDate = saved.PaidDate
            };
        }
    }
}
