namespace TechnicalTest.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the primary key of the user.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; } = null!;
    
    public long? RoleId { get; set; }
    
    public Role? Role { get; set; }
}