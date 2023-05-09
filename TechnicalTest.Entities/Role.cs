namespace TechnicalTest.Entities;

/// <summary>
/// Represents a tenant role.
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the primary key of the role.
    /// </summary>
    public long Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public List<Permission> Permissions { get; set; } = null!;
    
    public List<User> Users { get; set; } = null!;
}