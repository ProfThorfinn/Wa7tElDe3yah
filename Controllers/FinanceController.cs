using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        /// <summary>
        /// Get overall financial summary.
        /// </summary>
        /// <remarks>
        /// Calculates:
        /// - Total capital
        /// - Total revenue from paid booking amounts
        /// - Total expenses
        /// - Available balance
        /// - Net profit or loss
        /// - Profit/loss percentage
        /// </remarks>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _financeService.GetSummaryAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get financial summary for a specific month and year.
        /// </summary>
        /// <remarks>
        /// Calculates monthly revenue, expenses, profit/loss, and available balance.
        /// Example:
        /// GET /api/finance/summary-by-month?month=5&amp;year=2026
        /// </remarks>
        [HttpGet("summary-by-month")]
        public async Task<IActionResult> GetSummaryByMonth(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12");

            if (year < 2000)
                return BadRequest("Invalid year");

            var result = await _financeService.GetSummaryByMonthAsync(month, year);

            return Ok(result);
        }
    }
}