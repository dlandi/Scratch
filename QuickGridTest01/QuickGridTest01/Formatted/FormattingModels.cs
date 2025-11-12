namespace QuickGridExamples.Models;

/// <summary>
/// Financial transaction model demonstrating currency formatting
/// </summary>
public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "USD";
    public TransactionType Type { get; set; }
}

public enum TransactionType
{
    Deposit,
    Withdrawal,
    Transfer,
    Fee
}

/// <summary>
/// Product inventory model demonstrating various numeric formats
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public double DiscountPercent { get; set; }
    public decimal Weight { get; set; }  // in kg
    public long SizeBytes { get; set; }  // file size for digital products
    public DateTime LastRestocked { get; set; }
    public bool InStock { get; set; }
}

/// <summary>
/// Employee record demonstrating date and contact formatting
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
    public string Department { get; set; } = string.Empty;
}

/// <summary>
/// Performance metric demonstrating percentage and conditional formatting
/// </summary>
public class PerformanceMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double CurrentValue { get; set; }
    public double TargetValue { get; set; }
    public double PreviousValue { get; set; }
    public DateTime MeasuredDate { get; set; }
    
    public double PercentOfTarget => TargetValue > 0 ? CurrentValue / TargetValue : 0;
    public double ChangePercent => PreviousValue > 0 ? (CurrentValue - PreviousValue) / PreviousValue : 0;
}

/// <summary>
/// Log entry demonstrating time and duration formatting
/// </summary>
public class LogEntry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = "INFO";
    public string Message { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public long MemoryBytes { get; set; }
}

/// <summary>
/// Financial report demonstrating multi-currency and accounting formats
/// </summary>
public class FinancialReport
{
    public string Category { get; set; } = string.Empty;
    public decimal Q1 { get; set; }
    public decimal Q2 { get; set; }
    public decimal Q3 { get; set; }
    public decimal Q4 { get; set; }
    public decimal Total => Q1 + Q2 + Q3 + Q4;
    public decimal YoYChange { get; set; }  // Year over year change
}

/// <summary>
/// Customer with sensitive data demonstrating masking
/// </summary>
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string CreditCard { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public DateTime MemberSince { get; set; }
    public decimal TotalPurchases { get; set; }
}
