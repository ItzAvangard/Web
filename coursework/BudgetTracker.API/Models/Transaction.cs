using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetTracker.API.Models;

public class Transaction
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    [Required]
    public TransactionType Type { get; set; }
    
    public int? CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}

public enum TransactionType
{
    Income = 1,
    Expense = 2
}

