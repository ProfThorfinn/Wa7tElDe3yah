using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MonthlyReportsController : ControllerBase
    {
        private readonly IMonthlyReportService _monthlyReportService;

        public MonthlyReportsController(IMonthlyReportService monthlyReportService)
        {
            _monthlyReportService = monthlyReportService;
        }

        /// <summary>
        /// Get all generated monthly reports.
        /// </summary>
        /// <remarks>
        /// Returns all stored monthly reports ordered by generated date.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _monthlyReportService.GetAllAsync();
            return Ok(reports);
        }

        /// <summary>
        /// Get monthly report by id.
        /// </summary>
        /// <remarks>
        /// Returns a stored monthly report by its id.
        /// </remarks>
        /// <param name="id">Monthly report id.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var report = await _monthlyReportService.GetByIdAsync(id);

            if (report == null)
                return NotFound(new { message = "Monthly report not found" });

            return Ok(report);
        }

        /// <summary>
        /// Generate and store monthly report.
        /// </summary>
        /// <remarks>
        /// Generates a monthly report for a specific month and year.
        /// The report calculates total revenue, total expenses, net profit/loss,
        /// profit/loss percentage, generates a PDF file, stores the PDF path,
        /// and saves the report snapshot in the database.
        ///
        /// Example:
        /// POST /api/monthlyreports/generate?month=5&amp;year=2026
        /// </remarks>
        /// <param name="month">Month number from 1 to 12.</param>
        /// <param name="year">Report year, for example 2026.</param>
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMonthlyReport(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("uid")!.Value);

                var report = await _monthlyReportService.GenerateMonthlyReportAsync(month, year, userId);

                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Export monthly report as PDF without storing a new report record.
        /// </summary>
        /// <remarks>
        /// Generates a PDF file directly for a specific month and year.
        /// This endpoint returns the PDF as a downloadable file.
        ///
        /// Example:
        /// GET /api/monthlyreports/export-pdf?month=5&amp;year=2026
        /// </remarks>
        /// <param name="month">Month number from 1 to 12.</param>
        /// <param name="year">Report year, for example 2026.</param>
        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportPdf(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            try
            {
                var pdfBytes = await _monthlyReportService.ExportMonthlyReportPdfAsync(month, year);

                var fileName = $"monthly-report-{year}-{month}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}