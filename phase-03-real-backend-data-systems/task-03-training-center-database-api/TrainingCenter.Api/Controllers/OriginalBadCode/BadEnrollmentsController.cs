using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Controllers.OriginalBadCode;

/// <summary>
/// ORIGINAL BAD CODE — preserved for Task 07 refactor review.
/// See EnrollmentsController for refactored version.
/// </summary>
[ApiController]
[Route("api/bad-enrollments")]
public class BadEnrollmentsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var data = db.Enrollments
            .Include(e => e.Student)
            .Include(e => e.TrainingTrack)
            .Include(e => e.Payments)
            .ToList();
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(Enrollment enrollment)
    {
        enrollment.EnrollmentDate = DateTime.Now;
        enrollment.Status = EnrollmentStatus.Active;
        db.Enrollments.Add(enrollment);
        db.SaveChanges();
        return Ok(enrollment);
    }

    [HttpPost("pay")]
    public IActionResult Pay(int enrollmentId, decimal amount)
    {
        var enrollment = db.Enrollments
            .Include(x => x.Payments)
            .FirstOrDefault(x => x.EnrollmentId == enrollmentId);

        if (enrollment == null)
            return Ok("not found");

        var payment = new Payment
        {
            EnrollmentId = enrollmentId,
            Amount = amount,
            PaymentDate = DateTime.Now,
            PaymentStatus = PaymentStatus.Paid,
            ReferenceNumber = Guid.NewGuid().ToString(),
            PaymentMethod = PaymentMethod.Cash
        };
        db.Payments.Add(payment);
        db.SaveChanges();
        return Ok(payment);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = db.Enrollments.Find(id);
        if (item == null)
            return Ok("missing");

        db.Enrollments.Remove(item);
        db.SaveChanges();
        return Ok("deleted");
    }
}
