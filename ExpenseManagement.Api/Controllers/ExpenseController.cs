using ExpenseManagement.Api.DTOs;
using ExpenseManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ExpenseManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expense = await _expenseService.GetAllAsync();

            return Ok(expense);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);

            if(expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseDto dto)
        {
            await _expenseService.CreateAsync(dto);
            return Ok("Expense created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, UpdateExpenseDto dto)
        {
            var result = await _expenseService.UpdateAsync(id,dto);

            if (!result)
                return BadRequest("Expense could not be updated");

            return Ok("Expense updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _expenseService.DeleteAsync(id);

            if (!result)
                return BadRequest("expense could not be deleted");

            return Ok("Expense deleted successfully");
        }
        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _expenseService.ApproveAsync(id);

            if (!result)
                return BadRequest("Expense could not be approved.");

            return Ok("Expense approved successfully.");
        }

        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> Reject(int id, RejectExpenseDto dto)
        {
            var result = await _expenseService.RejectAsync(id, dto.ManagerNote);

            if (!result)
                return BadRequest("Expense could not be rejected.");

            return Ok("Expense rejected successfully.");
        }
    }
}
