using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wa7at_ElDr3yah_API.DTOs.Expense;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Get all expenses.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _expenseService.GetAllAsync();
            return Ok(data);
        }

        /// <summary>
        /// Get expense by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);

            if (expense == null)
                return NotFound("Expense not found");

            return Ok(expense);
        }

        /// <summary>
        /// Create a new expense.
        /// </summary>
        /// <remarks>
        /// Rules:
        /// - Amount must be greater than zero
        /// - Expense is linked to the logged-in user
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("uid")!.Value);

                var result = await _expenseService.CreateAsync(dto, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing expense.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto dto)
        {
            try
            {
                var result = await _expenseService.UpdateAsync(id, dto);

                if (result == null)
                    return NotFound("Expense not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an expense.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _expenseService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Expense not found");

            return Ok("Deleted successfully");
        }

        /// <summary>
        /// Filter expenses.
        /// </summary>
        /// <remarks>
        /// Example:
        /// GET /api/expenses/filter?keyword=clean&amp;category=Maintenance&amp;minAmount=100&amp;maxAmount=500
        /// </remarks>
        [HttpGet("filter")]
        public async Task<IActionResult> Filter(
            [FromQuery] string? keyword,
            [FromQuery] string? category,
            [FromQuery] decimal? minAmount,
            [FromQuery] decimal? maxAmount,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var result = await _expenseService.FilterAsync(
                keyword,
                category,
                minAmount,
                maxAmount,
                from,
                to
            );

            return Ok(result);
        }

        /// <summary>
        /// Get total expenses.
        /// </summary>
        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var total = await _expenseService.GetTotalExpensesAsync();
            return Ok(total);
        }

        /// <summary>
        /// Get total expenses for a specific month.
        /// </summary>
        [HttpGet("total-by-month")]
        public async Task<IActionResult> GetTotalByMonth(int month, int year)
        {
            var total = await _expenseService.GetTotalExpensesByMonthAsync(month, year);
            return Ok(total);
        }

        /// <summary>
        /// Get expenses created by a specific user.
        /// </summary>
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var data = await _expenseService.GetByUserAsync(userId);
            return Ok(data);
        }

        /// <summary>
        /// Get recent expenses.
        /// </summary>
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent(int count = 5)
        {
            var data = await _expenseService.GetRecentAsync(count);
            return Ok(data);
        }

        /// <summary>
        /// Get top expenses by amount.
        /// </summary>
        [HttpGet("top")]
        public async Task<IActionResult> GetTop(int count = 5)
        {
            var data = await _expenseService.GetTopAsync(count);
            return Ok(data);
        }

        /// <summary>
        /// Get paginated expenses.
        /// </summary>
        /// <remarks>
        /// Returns expenses by page number and page size.
        /// Example:
        /// GET /api/expenses/paged?pageNumber=1&amp;pageSize=10
        /// </remarks>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _expenseService.GetPagedAsync(pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Get all expense categories.
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var data = await _expenseService.GetCategoriesAsync();
            return Ok(data);
        }
    }
}