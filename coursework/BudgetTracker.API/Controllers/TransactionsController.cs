using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BudgetTracker.API.Data;
using BudgetTracker.API.DTOs;

namespace BudgetTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TransactionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? categoryId)
    {
        var userId = GetUserId();
        var query = _context.Transactions
            .Where(t => t.UserId == userId)
            .Include(t => t.Category)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(t => t.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.Date <= endDate.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId);
        }

        var transactions = await query
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Date = t.Date,
                Type = t.Type,
                CategoryId = t.CategoryId,
                CategoryName = t.Category != null ? t.Category.Name : null
            })
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
    {
        var userId = GetUserId();
        var transaction = await _context.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (transaction == null)
        {
            return NotFound();
        }

        return Ok(new TransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type,
            CategoryId = transaction.CategoryId,
            CategoryName = transaction.Category?.Name
        });
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        var userId = GetUserId();

        if (dto.CategoryId.HasValue)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId);
            
            if (!categoryExists)
            {
                return BadRequest(new { message = "Category not found" });
            }
        }

        var transaction = new BudgetTracker.API.Models.Transaction
        {
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date,
            Type = dto.Type,
            CategoryId = dto.CategoryId,
            UserId = userId
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        var category = dto.CategoryId.HasValue
            ? await _context.Categories.FindAsync(dto.CategoryId)
            : null;

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, new TransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type,
            CategoryId = transaction.CategoryId,
            CategoryName = category?.Name
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionDto dto)
    {
        var userId = GetUserId();
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (transaction == null)
        {
            return NotFound();
        }

        if (dto.CategoryId.HasValue)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId);
            
            if (!categoryExists)
            {
                return BadRequest(new { message = "Category not found" });
            }
        }

        transaction.Description = dto.Description;
        transaction.Amount = dto.Amount;
        transaction.Date = dto.Date;
        transaction.Type = dto.Type;
        transaction.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var userId = GetUserId();
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (transaction == null)
        {
            return NotFound();
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetSummary(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var userId = GetUserId();
        var query = _context.Transactions
            .Where(t => t.UserId == userId)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(t => t.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.Date <= endDate.Value);
        }

        var income = await query
            .Where(t => t.Type == BudgetTracker.API.Models.TransactionType.Income)
            .SumAsync(t => (decimal?)t.Amount) ?? 0;

        var expenses = await query
            .Where(t => t.Type == BudgetTracker.API.Models.TransactionType.Expense)
            .SumAsync(t => (decimal?)t.Amount) ?? 0;

        return Ok(new
        {
            totalIncome = income,
            totalExpenses = expenses,
            balance = income - expenses
        });
    }
}

