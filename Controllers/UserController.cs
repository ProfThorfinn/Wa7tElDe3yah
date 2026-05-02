using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wa7at_ElDr3yah_API.DTOs.User;
using Wa7at_ElDr3yah_API.Models;
using Wa7at_ElDr3yah_API.Services.Interfaces;
using Wa7at_ElDr3yah_API.Data;

namespace Wa7at_ElDr3yah_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public UsersController(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <remarks>
        /// Returns all system users including admins and owners.
        /// Password hashes are not returned.
        /// </remarks>
        /// <returns>List of users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <remarks>
        /// Returns user information using the user id.
        /// Password hash is not returned.
        /// </remarks>
        /// <param name="id">User id.</param>
        /// <returns>User details if found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        /// <summary>
        /// Initialize first owner (Development only).
        /// </summary>
        /// <remarks>
        /// Creates the first Owner in the system.
        /// This endpoint should be used ONLY once and then removed.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("init-owner")]
        public async Task<IActionResult> InitOwner([FromBody] UserRequestDto dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Role == Role.Owner);

            if (exists)
                return BadRequest("Owner already exists");

            dto.Role = Role.Owner;

            var user = await _userService.CreateAsync(dto);

            return Ok(user);
        }

        /// <summary>
        /// Create a new system user.
        /// </summary>
        /// <remarks>
        /// Business rules:
        /// - Email must be unique.
        /// - Password must be strong.
        /// - Password is stored as a BCrypt hash.
        /// - User can be Admin or Owner.
        /// </remarks>
        /// <param name="dto">User creation data.</param>
        /// <returns>The created user details.</returns>
        [HttpPost]
        [Authorize(Roles ="Owner")]
        public async Task<IActionResult> Create([FromBody] UserRequestDto dto)
        {
            try
            {
                var result = await _userService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <remarks>
        /// Business rules:
        /// - User must exist.
        /// - Email must remain unique.
        /// - If password is provided, it must be strong and will be hashed.
        /// - If password is empty, the old password remains unchanged.
        /// </remarks>
        /// <param name="id">User id.</param>
        /// <param name="dto">Updated user data.</param>
        /// <returns>The updated user details.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> Update(int id, [FromBody] UserRequestDto dto)
        {
            try
            {
                var result = await _userService.UpdateAsync(id, dto);

                if (result == null)
                    return NotFound("User not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Toggle user active status.
        /// </summary>
        /// <remarks>
        /// Activates or deactivates a user account.
        /// Inactive users cannot log in.
        /// </remarks>
        /// <param name="id">User id.</param>
        /// <returns>Success message if status is updated.</returns>
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _userService.ToggleStatusAsync(id);

            if (!result)
                return NotFound("User not found");

            return Ok("User status updated successfully");
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <remarks>
        /// Deletes a system user by id.
        /// If the user is linked to bookings, expenses, or reports, deletion may fail because of database restrictions.
        /// </remarks>
        /// <param name="id">User id.</param>
        /// <returns>Success message if deleted.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result)
                return NotFound("User not found");

            return Ok("User deleted successfully");
        }
    }
}