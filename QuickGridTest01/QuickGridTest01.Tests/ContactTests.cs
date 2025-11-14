using QuickGridTest01.MultiState.Demo;
using Xunit;

namespace QuickGridTest01.MultiState.Tests.Demo;

/// <summary>
/// Comprehensive tests for Contact model and ContactService.
/// Organized into sections: Model Tests and Service Tests.
/// </summary>
public class ContactTests
{
    #region Contact Model Tests

    [Fact]
    public void Constructor_InitializesWithDefaults()
    {
        // Act
        var contact = new Contact();

        // Assert
        Assert.Equal(0, contact.Id);
        Assert.Equal(string.Empty, contact.Name);
        Assert.Equal(string.Empty, contact.Email);
        Assert.Equal(string.Empty, contact.Phone);
        Assert.Equal(string.Empty, contact.Company);
        Assert.Equal(string.Empty, contact.Title);
        Assert.True(contact.IsActive);
        Assert.Equal(string.Empty, contact.Notes);
        Assert.NotEqual(default(DateTime), contact.CreatedDate);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var contact = new Contact
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "555-123-4567",
            Company = "Acme Corp",
            Title = "CEO",
            IsActive = true,
            Notes = "Test notes"
        };

        // Assert
        Assert.Equal(1, contact.Id);
        Assert.Equal("John Doe", contact.Name);
        Assert.Equal("john@example.com", contact.Email);
        Assert.Equal("555-123-4567", contact.Phone);
        Assert.Equal("Acme Corp", contact.Company);
        Assert.Equal("CEO", contact.Title);
        Assert.True(contact.IsActive);
        Assert.Equal("Test notes", contact.Notes);
    }

    [Fact]
    public void Clone_CreatesDeepCopy()
    {
        // Arrange
        var original = new Contact
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "555-123-4567",
            Company = "Acme Corp",
            Title = "CEO",
            IsActive = true,
            CreatedDate = DateTime.Now,
            LastModifiedDate = DateTime.Now,
            Notes = "Test notes"
        };

        // Act
        var clone = original.Clone();

        // Assert
        Assert.NotSame(original, clone);
        Assert.Equal(original.Id, clone.Id);
        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Email, clone.Email);
        Assert.Equal(original.Phone, clone.Phone);
        Assert.Equal(original.Company, clone.Company);
        Assert.Equal(original.Title, clone.Title);
        Assert.Equal(original.IsActive, clone.IsActive);
        Assert.Equal(original.CreatedDate, clone.CreatedDate);
        Assert.Equal(original.LastModifiedDate, clone.LastModifiedDate);
        Assert.Equal(original.Notes, clone.Notes);
    }

    [Fact]
    public void Clone_ModifyingCloneDoesNotAffectOriginal()
    {
        // Arrange
        var original = new Contact
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com"
        };

        // Act
        var clone = original.Clone();
        clone.Name = "Jane Doe";
        clone.Email = "jane@example.com";

        // Assert
        Assert.Equal("John Doe", original.Name);
        Assert.Equal("john@example.com", original.Email);
        Assert.Equal("Jane Doe", clone.Name);
        Assert.Equal("jane@example.com", clone.Email);
    }

    [Fact]
    public void ToString_ReturnsNameAndEmail()
    {
        // Arrange
        var contact = new Contact
        {
            Name = "John Doe",
            Email = "john@example.com"
        };

        // Act
        var result = contact.ToString();

        // Assert
        Assert.Equal("John Doe (john@example.com)", result);
    }

    [Fact]
    public void IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var contact = new Contact();

        // Assert
        Assert.True(contact.IsActive);
    }

    [Fact]
    public void CreatedDate_IsSetByDefault()
    {
        // Arrange
        var beforeCreate = DateTime.Now.AddSeconds(-1);
        
        // Act
        var contact = new Contact();
        var afterCreate = DateTime.Now.AddSeconds(1);

        // Assert
        Assert.True(contact.CreatedDate >= beforeCreate);
        Assert.True(contact.CreatedDate <= afterCreate);
    }

    [Fact]
    public void LastModifiedDate_IsNullByDefault()
    {
        // Arrange & Act
        var contact = new Contact();

        // Assert
        Assert.Null(contact.LastModifiedDate);
    }

    [Fact]
    public void LastModifiedDate_CanBeSet()
    {
        // Arrange
        var contact = new Contact();
        var modifiedDate = DateTime.Now;

        // Act
        contact.LastModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, contact.LastModifiedDate);
    }

    #endregion

    #region ContactService Tests

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoContactsExist()
    {
        // Arrange
        var service = new ContactService();

        // Act
        var contacts = await service.GetAllAsync();

        // Assert
        Assert.NotNull(contacts);
        Assert.Empty(contacts);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsClones_NotOriginals()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        await service.AddAsync(contact);

        // Act
        var contacts1 = await service.GetAllAsync();
        var contacts2 = await service.GetAllAsync();

        // Assert
        Assert.NotSame(contacts1[0], contacts2[0]);
        Assert.Equal(contacts1[0].Name, contacts2[0].Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsContact_WhenExists()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var retrieved = await service.GetByIdAsync(addedContact!.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(addedContact.Id, retrieved.Id);
        Assert.Equal(addedContact.Name, retrieved.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var service = new ContactService();

        // Act
        var retrieved = await service.GetByIdAsync(999);

        // Assert
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task AddAsync_AddsNewContact_WithGeneratedId()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact
        {
            Name = "Test User",
            Email = "test@example.com",
            Phone = "555-123-4567"
        };

        // Act
        var (success, error, addedContact) = await service.AddAsync(contact);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.NotNull(addedContact);
        Assert.True(addedContact.Id > 0);
    }

    [Fact]
    public async Task AddAsync_FailsWhenEmailAlreadyExists()
    {
        // Arrange
        var service = new ContactService();
        var contact1 = new Contact { Name = "User 1", Email = "duplicate@example.com", Phone = "555-111-1111" };
        var contact2 = new Contact { Name = "User 2", Email = "duplicate@example.com", Phone = "555-222-2222" };

        // Act
        await service.AddAsync(contact1);
        var (success, error, _) = await service.AddAsync(contact2);

        // Assert
        Assert.False(success);
        Assert.NotNull(error);
        Assert.Contains("already exists", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdateNameAsync_UpdatesContact_WhenValid()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Original Name", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var (success, error) = await service.UpdateNameAsync(addedContact!.Id, "Updated Name");
        var retrieved = await service.GetByIdAsync(addedContact.Id);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal("Updated Name", retrieved!.Name);
        Assert.NotNull(retrieved.LastModifiedDate);
    }

    [Fact]
    public async Task UpdateNameAsync_FailsWhenEmpty()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var (success, error) = await service.UpdateNameAsync(addedContact!.Id, "");

        // Assert
        Assert.False(success);
        Assert.NotNull(error);
        Assert.Contains("cannot be empty", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdateNameAsync_FailsWhenTooShort()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var (success, error) = await service.UpdateNameAsync(addedContact!.Id, "A");

        // Assert
        Assert.False(success);
        Assert.NotNull(error);
        Assert.Contains("at least 2 characters", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdateNameAsync_FailsWhenTooLong()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);
        var longName = new string('A', 101);

        // Act
        var (success, error) = await service.UpdateNameAsync(addedContact!.Id, longName);

        // Assert
        Assert.False(success);
        Assert.NotNull(error);
        Assert.Contains("cannot exceed 100 characters", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdateEmailAsync_UpdatesContact_WhenValid()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "old@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var (success, error) = await service.UpdateEmailAsync(addedContact!.Id, "new@example.com");
        var retrieved = await service.GetByIdAsync(addedContact.Id);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal("new@example.com", retrieved!.Email);
    }

    [Fact]
    public async Task UpdateEmailAsync_FailsWhenInvalidFormat()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var (success, error) = await service.UpdateEmailAsync(addedContact!.Id, "invalid-email");

        // Assert
        Assert.False(success);
        Assert.NotNull(error);
        Assert.Contains("Invalid email", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdateEmailAsync_FailsWhenAlreadyExists()
    {
        // Arrange
        var service = new ContactService();
        var contact1 = new Contact { Name = "User 1", Email = "user1@example.com", Phone = "555-111-1111" };
        var contact2 = new Contact { Name = "User 2", Email = "user2@example.com", Phone = "555-222-2222" };
        var (_, _, added1) = await service.AddAsync(contact1);
        var (_, _, added2) = await service.AddAsync(contact2);

        // Act
        var (success, error) = await service.UpdateEmailAsync(added2!.Id, "user1@example.com");

        // Assert
        Assert.False(success);
        Assert.NotNull(error);
        Assert.Contains("already exists", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdatePhoneAsync_UpdatesContact_WhenValid()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var (success, error) = await service.UpdatePhoneAsync(addedContact!.Id, "555-987-6543");
        var retrieved = await service.GetByIdAsync(addedContact.Id);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal("555-987-6543", retrieved!.Phone);
    }

    [Fact]
    public async Task UpdatePhoneAsync_FailsWhenInvalidFormat()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var (success, error) = await service.UpdatePhoneAsync(addedContact!.Id, "123456789");

        // Assert
        Assert.False(success);
        Assert.NotNull(error);
        Assert.Contains("555-123-4567", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdateCompanyAsync_UpdatesContact_WhenValid()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567", Company = "Old Corp" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var (success, error) = await service.UpdateCompanyAsync(addedContact!.Id, "New Corp");
        var retrieved = await service.GetByIdAsync(addedContact.Id);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal("New Corp", retrieved!.Company);
    }

    [Fact]
    public async Task DeleteAsync_RemovesContact_WhenExists()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var deleted = await service.DeleteAsync(addedContact!.Id);
        var retrieved = await service.GetByIdAsync(addedContact.Id);

        // Assert
        Assert.True(deleted);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotExists()
    {
        // Arrange
        var service = new ContactService();

        // Act
        var deleted = await service.DeleteAsync(999);

        // Assert
        Assert.False(deleted);
    }

    [Fact]
    public async Task SeedDataAsync_AddsContacts()
    {
        // Arrange
        var service = new ContactService();

        // Act
        await service.SeedDataAsync();
        var contacts = await service.GetAllAsync();

        // Assert
        Assert.NotEmpty(contacts);
        Assert.True(contacts.Count >= 10);
    }

    [Fact]
    public async Task SeedDataAsync_OnlyRunsOnce()
    {
        // Arrange
        var service = new ContactService();

        // Act
        await service.SeedDataAsync();
        var firstCount = (await service.GetAllAsync()).Count;
        
        await service.SeedDataAsync();
        var secondCount = (await service.GetAllAsync()).Count;

        // Assert
        Assert.Equal(firstCount, secondCount);
    }

    [Fact]
    public async Task IsEmailUniqueAsync_ReturnsTrue_WhenEmailNotUsed()
    {
        // Arrange
        var service = new ContactService();

        // Act
        var isUnique = await service.IsEmailUniqueAsync("unused@example.com");

        // Assert
        Assert.True(isUnique);
    }

    [Fact]
    public async Task IsEmailUniqueAsync_ReturnsFalse_WhenEmailExists()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "existing@example.com", Phone = "555-123-4567" };
        await service.AddAsync(contact);

        // Act
        var isUnique = await service.IsEmailUniqueAsync("existing@example.com");

        // Assert
        Assert.False(isUnique);
    }

    [Fact]
    public async Task IsEmailUniqueAsync_ReturnsTrue_WhenExcludingCurrentContact()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        var (_, _, addedContact) = await service.AddAsync(contact);

        // Act
        var isUnique = await service.IsEmailUniqueAsync("test@example.com", addedContact!.Id);

        // Assert
        Assert.True(isUnique);
    }

    [Fact]
    public async Task IsEmailUniqueAsync_IsCaseInsensitive()
    {
        // Arrange
        var service = new ContactService();
        var contact = new Contact { Name = "Test User", Email = "test@example.com", Phone = "555-123-4567" };
        await service.AddAsync(contact);

        // Act
        var isUnique = await service.IsEmailUniqueAsync("TEST@EXAMPLE.COM");

        // Assert
        Assert.False(isUnique);
    }

    [Fact]
    public async Task ConcurrentAccess_IsSafe()
    {
        // Arrange
        var service = new ContactService();
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            var index = i;
            tasks.Add(Task.Run(async () =>
            {
                var contact = new Contact
                {
                    Name = $"User {index}",
                    Email = $"user{index}@example.com",
                    Phone = "555-123-4567"
                };
                await service.AddAsync(contact);
            }));
        }

        await Task.WhenAll(tasks);
        var contacts = await service.GetAllAsync();

        // Assert
        Assert.Equal(10, contacts.Count);
        Assert.Equal(10, contacts.Select(c => c.Email).Distinct().Count());
    }

    #endregion
}
