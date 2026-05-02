using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wa7at_ElDr3yah_API.DTOs.Capital;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CapitalsController : ControllerBase
    {
        private readonly ICapitalService _capitalService;

        public CapitalsController(ICapitalService capitalService)
        {
            _capitalService = capitalService;
        }

        /// <summary>
        /// Get all capital records.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _capitalService.GetAllAsync();
            return Ok(data);
        }

        /// <summary>
        /// Get capital by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var capital = await _capitalService.GetByIdAsync(id);

            if (capital == null)
                return NotFound("Capital not found");

            return Ok(capital);
        }

        /// <summary>
        /// Create a new capital record.
        /// </summary>
        /// <remarks>
        /// Rules:
        /// - Amount must be greater than zero
        /// - Linked to logged-in user
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CapitalDto dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("uid")!.Value);

                var result = await _capitalService.CreateAsync(dto, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update capital record.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CapitalDto dto)
        {
            try
            {
                var result = await _capitalService.UpdateAsync(id, dto);

                if (result == null)
                    return NotFound("Capital not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete capital record.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _capitalService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Capital not found");

            return Ok("Deleted successfully");
        }

        /// <summary>
        /// Get total capital.
        /// </summary>
        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var total = await _capitalService.GetTotalCapitalAsync();
            return Ok(total);
        }

        /// <summary>
        /// Get latest capital record.
        /// </summary>
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var data = await _capitalService.GetLatestCapitalAsync();

            if (data == null)
                return NotFound("No capital records found");

            return Ok(data);
        }
    }
}