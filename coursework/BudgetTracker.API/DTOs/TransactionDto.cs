using BudgetTracker.API.Models;

namespace BudgetTracker.API.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
}

public class CreateTransactionDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public int? CategoryId { get; set; }
}

public class UpdateTransactionDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public int? CategoryId { get; set; }
}

