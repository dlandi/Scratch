namespace QuickGridTest01.MultiState.Demo;

/// <summary>
/// In-memory contact service for demo purposes.
/// Provides CRUD operations with simulated async delays and validation.
/// </summary>
public class ContactService
{
    private readonly List<Contact> _contacts = new();
    private int _nextId = 1;
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// Gets all contacts.
    /// </summary>
    public async Task<List<Contact>> GetAllAsync()
    {
        await SimulateDelay();
        await _lock.WaitAsync();
        try
        {
            return _contacts.Select(c => c.Clone()).ToList();
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Gets a contact by ID.
    /// </summary>
    public async Task<Contact?> GetByIdAsync(int id)
    {
        await SimulateDelay();
        await _lock.WaitAsync();
        try
        {
            return _contacts.FirstOrDefault(c => c.Id == id)?.Clone();
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Adds a new contact.
    /// </summary>
    public async Task<(bool Success, string? Error, Contact? Contact)> AddAsync(Contact contact)
    {
        await SimulateDelay();
        await _lock.WaitAsync();
        try
        {
            // Validate email uniqueness
            if (_contacts.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return (false, "Email address already exists", null);
            }

            // Assign ID
            contact.Id = _nextId++;
            contact.CreatedDate = DateTime.Now;
            contact.LastModifiedDate = null;

            _contacts.Add(contact.Clone());
            return (true, null, contact.Clone());
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Updates an existing contact's name.
    /// </summary>
    public async Task<(bool Success, string? Error)> UpdateNameAsync(int id, string name)
    {
        await SimulateDelay();
        await _lock.WaitAsync();
        try
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return (false, "Contact not found");

            if (string.IsNullOrWhiteSpace(name))
                return (false, "Name cannot be empty");

            if (name.Length < 2)
                return (false, "Name must be at least 2 characters");

            if (name.Length > 100)
                return (false, "Name cannot exceed 100 characters");

            contact.Name = name;
            contact.LastModifiedDate = DateTime.Now;
            return (true, null);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Updates an existing contact's email.
    /// </summary>
    public async Task<(bool Success, string? Error)> UpdateEmailAsync(int id, string email)
    {
        await SimulateDelay();
        await _lock.WaitAsync();
        try
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return (false, "Contact not found");

            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email cannot be empty");

            // Email format validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "Invalid email format");

            // Uniqueness check (excluding current contact)
            if (_contacts.Any(c => c.Id != id && c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return (false, "Email address already exists");

            contact.Email = email;
            contact.LastModifiedDate = DateTime.Now;
            return (true, null);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Updates an existing contact's phone.
    /// </summary>
    public async Task<(bool Success, string? Error)> UpdatePhoneAsync(int id, string phone)
    {
        await SimulateDelay();
        await _lock.WaitAsync();
        try
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return (false, "Contact not found");

            if (string.IsNullOrWhiteSpace(phone))
                return (false, "Phone cannot be empty");

            // Phone format validation (US format)
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{3}-\d{3}-\d{4}$"))
                return (false, "Phone must be in format: 555-123-4567");

            contact.Phone = phone;
            contact.LastModifiedDate = DateTime.Now;
            return (true, null);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Updates an existing contact's company.
    /// </summary>
    public async Task<(bool Success, string? Error)> UpdateCompanyAsync(int id, string company)
    {
        await SimulateDelay();
        await _lock.WaitAsync();
        try
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return (false, "Contact not found");

            if (string.IsNullOrWhiteSpace(company))
                return (false, "Company cannot be empty");

            contact.Company = company;
            contact.LastModifiedDate = DateTime.Now;
            return (true, null);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Deletes a contact.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        await SimulateDelay();
        await _lock.WaitAsync();
        try
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return false;

            _contacts.Remove(contact);
            return true;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Seeds the service with sample data.
    /// </summary>
    public async Task SeedDataAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (_contacts.Any())
                return; // Already seeded

            var sampleContacts = new[]
            {
                new Contact { Name = "John Smith", Email = "john.smith@example.com", Phone = "555-123-4567", Company = "Acme Corp", Title = "CEO" },
                new Contact { Name = "Jane Doe", Email = "jane.doe@example.com", Phone = "555-234-5678", Company = "TechStart Inc", Title = "CTO" },
                new Contact { Name = "Bob Johnson", Email = "bob.johnson@example.com", Phone = "555-345-6789", Company = "DataSys LLC", Title = "Developer" },
                new Contact { Name = "Alice Williams", Email = "alice.williams@example.com", Phone = "555-456-7890", Company = "CloudNet", Title = "Designer" },
                new Contact { Name = "Charlie Brown", Email = "charlie.brown@example.com", Phone = "555-567-8901", Company = "WebWorks", Title = "Manager" },
                new Contact { Name = "Diana Prince", Email = "diana.prince@example.com", Phone = "555-678-9012", Company = "SecureIT", Title = "Analyst" },
                new Contact { Name = "Edward Norton", Email = "edward.norton@example.com", Phone = "555-789-0123", Company = "DevOps Pro", Title = "Engineer" },
                new Contact { Name = "Fiona Apple", Email = "fiona.apple@example.com", Phone = "555-890-1234", Company = "AppStore", Title = "Product Manager" },
                new Contact { Name = "George Martin", Email = "george.martin@example.com", Phone = "555-901-2345", Company = "MusicTech", Title = "Director" },
                new Contact { Name = "Helen Hunt", Email = "helen.hunt@example.com", Phone = "555-012-3456", Company = "SearchCo", Title = "Researcher" }
            };

            foreach (var contact in sampleContacts)
            {
                contact.Id = _nextId++;
                contact.CreatedDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 365));
                _contacts.Add(contact);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Checks if an email is unique (excluding a specific contact ID).
    /// </summary>
    public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
    {
        await SimulateDelay(50); // Shorter delay for validation
        await _lock.WaitAsync();
        try
        {
            return !_contacts.Any(c => 
                c.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                c.Id != (excludeId ?? -1));
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Simulates a network delay.
    /// </summary>
    private static async Task SimulateDelay(int minMs = 100, int maxMs = 300)
    {
        var delay = Random.Shared.Next(minMs, maxMs);
        await Task.Delay(delay);
    }
}
