namespace QuickGridTest01.FormattedValue.Demo.Models;

/// <summary>
/// Employee record model demonstrating date, phone, and masked formats.
/// </summary>
public class Employee
{
    /// <summary>
    /// Gets or sets the employee unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the employee's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hire date.
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// Gets or sets the termination date (null if still employed).
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// Gets or sets the annual salary.
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Gets or sets whether the employee is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets the full name by combining first and last names.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
