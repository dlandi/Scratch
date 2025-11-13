using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickGridTest01.ConditionalStyling;

/// <summary>
/// Sales performance data for revenue tracking
/// </summary>
public class SalesPerformance
{
    public int Id { get; set; }
    public string SalesRep { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Target { get; set; }
    public decimal ActualVsTarget => Target > 0 ? ((Revenue - Target) / Target) * 100 : 0;
    public int DealsClosed { get; set; }
    public string Quarter { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Inventory item with stock levels
/// </summary>
public class InventoryItem
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int StockLevel { get; set; }
    public int ReorderPoint { get; set; }
    public int MaxStock { get; set; }
    public decimal UnitPrice { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime LastRestocked { get; set; }
    public int DaysSinceRestock => (DateTime.Now - LastRestocked).Days;
    public string StockStatus
    {
        get
        {
            if (StockLevel == 0) return "Out of Stock";
            if (StockLevel < ReorderPoint) return "Low Stock";
            if (StockLevel > MaxStock) return "Overstock";
            return "Normal";
        }
    }
}

/// <summary>
/// Task with priority and status
/// </summary>
public class TaskItem
{
    public int Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    public int CompletionPercentage { get; set; }
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now && Status != "Completed";
    public int? DaysUntilDue => DueDate?.Subtract(DateTime.Now).Days;
}

/// <summary>
/// Health metric with vital signs
/// </summary>
public class HealthMetric
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public decimal Temperature { get; set; }
    public int HeartRate { get; set; }
    public string BloodPressure { get; set; } = string.Empty;
    public decimal OxygenLevel { get; set; }
    public DateTime RecordedAt { get; set; }
    public string OverallStatus { get; set; } = string.Empty;
    public bool IsCritical { get; set; }
}

/// <summary>
/// Financial statement line item
/// </summary>
public class FinancialStatement
{
    public int Id { get; set; }
    public string Account { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CurrentPeriod { get; set; }
    public decimal PreviousPeriod { get; set; }
    public decimal Change => CurrentPeriod - PreviousPeriod;
    public decimal PercentageChange => PreviousPeriod != 0 ? ((CurrentPeriod - PreviousPeriod) / Math.Abs(PreviousPeriod)) * 100 : 0;
    public string Variance => Change >= 0 ? "Favorable" : "Unfavorable";
}

/// <summary>
/// Server health status
/// </summary>
public class ServerStatus
{
    public int Id { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string HealthStatus { get; set; } = string.Empty;
    public decimal CpuUsage { get; set; }
    public decimal MemoryUsage { get; set; }
    public decimal DiskUsage { get; set; }
    public int ResponseTime { get; set; }
    public DateTime LastChecked { get; set; }
    public bool IsOnline { get; set; }
    public int UptimePercentage { get; set; }
}

/// <summary>
/// Customer satisfaction data
/// </summary>
public class CustomerSatisfaction
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public string Sentiment { get; set; } = string.Empty;
    public int ResponseTime { get; set; }
    public DateTime SurveyDate { get; set; }
    public bool WouldRecommend { get; set; }
    public string FeedbackCategory { get; set; } = string.Empty;
    public int NpsScore { get; set; }
}

/// <summary>
/// Static data generator for demo purposes
/// </summary>
public static class ConditionalStyleDataGenerator
{
    private static readonly Random _random = new Random(42);

    public static List<SalesPerformance> GenerateSalesData()
    {
        var regions = new[] { "North", "South", "East", "West", "Central" };
        var quarters = new[] { "Q1 2024", "Q2 2024", "Q3 2024", "Q4 2024" };
        var statuses = new[] { "Active", "Pending", "Completed" };
        var names = new[] { "Alice Johnson", "Bob Smith", "Carol Williams", "David Brown", "Eva Martinez", 
                            "Frank Davis", "Grace Lee", "Henry Wilson", "Iris Chen", "Jack Taylor" };

        return Enumerable.Range(1, 50).Select(i => new SalesPerformance
        {
            Id = i,
            SalesRep = names[i % names.Length],
            Region = regions[i % regions.Length],
            Revenue = _random.Next(50000, 500000),
            Target = _random.Next(100000, 400000),
            DealsClosed = _random.Next(5, 50),
            Quarter = quarters[i % quarters.Length],
            Status = statuses[i % statuses.Length]
        }).ToList();
    }

    public static List<InventoryItem> GenerateInventoryData()
    {
        var categories = new[] { "Electronics", "Clothing", "Food", "Tools", "Books" };
        var products = new[] { "Widget A", "Gadget B", "Device C", "Item D", "Product E",
                               "Component F", "Part G", "Tool H", "Supply I", "Material J" };

        return Enumerable.Range(1, 40).Select(i => new InventoryItem
        {
            Id = i,
            ProductName = $"{products[i % products.Length]} {i}",
            Sku = $"SKU-{1000 + i}",
            StockLevel = _random.Next(0, 200),
            ReorderPoint = _random.Next(10, 30),
            MaxStock = _random.Next(100, 150),
            UnitPrice = _random.Next(10, 500),
            Category = categories[i % categories.Length],
            LastRestocked = DateTime.Now.AddDays(-_random.Next(1, 90))
        }).ToList();
    }

    public static List<TaskItem> GenerateTaskData()
    {
        var priorities = new[] { "High", "Medium", "Low", "Critical", "Minor" };
        var statuses = new[] { "Not Started", "In Progress", "Completed", "Pending", "Blocked" };
        var assignees = new[] { "John Doe", "Jane Smith", "Mike Johnson", "Sarah Williams", "Tom Brown" };
        var taskNames = new[] {
            "Update documentation",
            "Fix critical bug",
            "Review pull request",
            "Deploy to production",
            "Write unit tests",
            "Refactor legacy code",
            "Design new feature",
            "Database migration",
            "Performance optimization",
            "Security audit"
        };

        return Enumerable.Range(1, 30).Select(i => new TaskItem
        {
            Id = i,
            TaskName = $"{taskNames[i % taskNames.Length]} #{i}",
            Priority = priorities[i % priorities.Length],
            Status = statuses[i % statuses.Length],
            DueDate = i % 4 == 0 ? null : DateTime.Now.AddDays(_random.Next(-15, 30)),
            AssignedTo = assignees[i % assignees.Length],
            CompletionPercentage = _random.Next(0, 101)
        }).ToList();
    }

    public static List<HealthMetric> GenerateHealthData()
    {
        var names = new[] { "Patient A", "Patient B", "Patient C", "Patient D", "Patient E" };
        var statuses = new[] { "Normal", "Warning", "Critical", "Observation" };

        return Enumerable.Range(1, 25).Select(i => new HealthMetric
        {
            Id = i,
            PatientName = $"{names[i % names.Length]} ({1000 + i})",
            Temperature = 96m + (decimal)_random.NextDouble() * 6m,
            HeartRate = _random.Next(50, 140),
            BloodPressure = $"{_random.Next(90, 180)}/{_random.Next(60, 120)}",
            OxygenLevel = 88m + (decimal)_random.NextDouble() * 12m,
            RecordedAt = DateTime.Now.AddHours(-_random.Next(0, 24)),
            OverallStatus = statuses[i % statuses.Length],
            IsCritical = i % 7 == 0
        }).ToList();
    }

    public static List<FinancialStatement> GenerateFinancialData()
    {
        var accounts = new[] {
            "Revenue", "Cost of Goods Sold", "Operating Expenses", "Net Income",
            "Assets", "Liabilities", "Equity", "Cash Flow",
            "Accounts Receivable", "Accounts Payable"
        };
        var categories = new[] { "Income", "Expense", "Asset", "Liability" };

        return Enumerable.Range(1, 20).Select(i => new FinancialStatement
        {
            Id = i,
            Account = accounts[i % accounts.Length],
            Category = categories[i % categories.Length],
            CurrentPeriod = _random.Next(-50000, 500000),
            PreviousPeriod = _random.Next(-50000, 500000)
        }).ToList();
    }

    public static List<ServerStatus> GenerateServerData()
    {
        var healthStatuses = new[] { "Healthy", "Degraded", "Unhealthy", "Warning", "Unknown" };

        return Enumerable.Range(1, 15).Select(i => new ServerStatus
        {
            Id = i,
            ServerName = $"Server-{i:D3}",
            HealthStatus = healthStatuses[i % healthStatuses.Length],
            CpuUsage = (decimal)_random.NextDouble() * 100,
            MemoryUsage = (decimal)_random.NextDouble() * 100,
            DiskUsage = (decimal)_random.NextDouble() * 100,
            ResponseTime = _random.Next(10, 2000),
            LastChecked = DateTime.Now.AddMinutes(-_random.Next(0, 60)),
            IsOnline = i % 8 != 0,
            UptimePercentage = _random.Next(85, 100)
        }).ToList();
    }

    public static List<CustomerSatisfaction> GenerateCustomerSatisfactionData()
    {
        var sentiments = new[] { "Positive", "Neutral", "Negative", "Very Positive", "Very Negative" };
        var categories = new[] { "Product Quality", "Customer Service", "Delivery", "Pricing", "Overall Experience" };
        var names = new[] { "Customer A", "Customer B", "Customer C", "Customer D", "Customer E" };

        return Enumerable.Range(1, 30).Select(i => new CustomerSatisfaction
        {
            Id = i,
            CustomerName = $"{names[i % names.Length]} ({i})",
            Rating = (decimal)(_random.NextDouble() * 5),
            Sentiment = sentiments[i % sentiments.Length],
            ResponseTime = _random.Next(1, 48),
            SurveyDate = DateTime.Now.AddDays(-_random.Next(0, 90)),
            WouldRecommend = _random.NextDouble() > 0.3,
            FeedbackCategory = categories[i % categories.Length],
            NpsScore = _random.Next(-100, 101)
        }).ToList();
    }
}
