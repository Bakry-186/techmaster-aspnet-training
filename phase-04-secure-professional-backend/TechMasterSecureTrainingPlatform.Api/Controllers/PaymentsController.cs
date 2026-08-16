using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController(PaymentService service, AuditService auditService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPayments(DateTime? from, DateTime? to, string? status)
    {
        if (from.HasValue && to.HasValue && from > to)
            return BadRequest(ApiResponse<object>.Fail("from must be less than or equal to to."));
        var (data, error) = await service.GetAllAsync(from, to, status);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<IReadOnlyList<PaymentResponse>>.Ok(data!));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(CreatePaymentRequest request)
    {
        var (data, error) = await service.CreateAsync(request);
        if (data is not null)
            await auditService.LogAsync("CreatePayment", "Payment", data.PaymentId,
                $"Payment created for enrollment {data.EnrollmentId}, amount {data.Amount}.");
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Created($"/api/payments/{data!.PaymentId}",
                ApiResponse<PaymentResponse>.Ok(data!, "Payment created successfully."));
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdatePaymentStatus(int id, UpdatePaymentStatusRequest request)
    {
        var (data, error) = await service.UpdateStatusAsync(id, request);
        if (error == "Payment not found.") return NotFound(ApiResponse<object>.Fail(error));
        if (data is not null)
            await auditService.LogAsync("UpdatePaymentStatus", "Payment", data.PaymentId,
                $"Payment status changed to {data.PaymentStatus}.");
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<PaymentResponse>.Ok(data!, "Payment status updated."));
    }
}

[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("api/enrollments/{enrollmentId:int}/payments")]
public class EnrollmentPaymentsController(PaymentService service, EnrollmentService enrollmentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEnrollmentPayments(int enrollmentId)
    {
        if (await enrollmentService.GetByIdAsync(enrollmentId) is null)
            return NotFound(ApiResponse<object>.Fail("Enrollment not found."));
        var data = await service.GetByEnrollmentAsync(enrollmentId);
        return Ok(ApiResponse<IReadOnlyList<PaymentResponse>>.Ok(data));
    }
}
