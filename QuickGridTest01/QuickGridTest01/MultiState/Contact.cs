namespace QuickGridTest01.MultiState.Demo;

/// <summary>
/// Contact domain model for demo purposes.
/// </summary>
public class Contact
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the contact's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact's phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact's company.
    /// </summary>
    public string Company { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact's job title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the contact is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the date the contact was created.
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the date the contact was last modified.
    /// </summary>
    public DateTime? LastModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the contact.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Creates a clone of the contact.
    /// </summary>
    public Contact Clone()
    {
        return new Contact
        {
            Id = Id,
            Name = Name,
            Email = Email,
            Phone = Phone,
            Company = Company,
            Title = Title,
            IsActive = IsActive,
            CreatedDate = CreatedDate,
            LastModifiedDate = LastModifiedDate,
            Notes = Notes
        };
    }

    /// <summary>
    /// Returns a string representation of the contact.
    /// </summary>
    public override string ToString()
    {
        return $"{Name} ({Email})";
    }
}
