namespace TechnicalTest.Entities;

/// <summary>
/// Represents a permission.
/// </summary>
public class Permission
{
    /// <summary>
    /// Gets or sets the primary key of the permission.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the permission.
    /// </summary>
    public string Name { get; set; } = null!;
    
    
    /// <summary>
    /// Gets or sets the roles, for each of which the permission exists.
    /// </summary>
    public ISet<Role> Roles { get; set; } = null!;
}