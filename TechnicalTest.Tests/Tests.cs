using Microsoft.EntityFrameworkCore;
using TechnicalTest.Entities;
using TechnicalTest.Services;
using TechnicalTest.Services.Services;

namespace TechnicalTest.Tests;

/// <summary>
/// Tests for the <see cref="UserService"/> class.
/// </summary>
public class Tests
{
    private RepositoryContext _repositoryContext = null!;

    private UserService _userService = null!;

    [SetUp]
    public void Setup()
    {
        _repositoryContext = new RepositoryContext(new DbContextOptionsBuilder()
            .UseSqlite("Data Source=:memory:").Options);

        _userService = new UserService(_repositoryContext);

        _repositoryContext.Database.OpenConnection();
        _repositoryContext.Database.EnsureCreated();
        CreateSampleData();
    }

    private void CreateSampleData()
    {
        _repositoryContext.Add(new Role { Id = 1, Name = "Role 1" });
        _repositoryContext.Add(new Role { Id = 2, Name = "Role 2" });
        _repositoryContext.Add(new Role { Id = 3, Name = "Role 3" });
        _repositoryContext.Add(new Role { Id = 4, Name = "Role 4" });
        _repositoryContext.Add(new Role { Id = 5, Name = "Role 5" });
        _repositoryContext.Add(new User { Id = 1, Name = "User 1", RoleId = 1 });
        _repositoryContext.Add(new User { Id = 2, Name = "User 2", RoleId = 1 });
        _repositoryContext.Add(new User { Id = 3, Name = "User 3", RoleId = 2 });
        _repositoryContext.Add(new User { Id = 4, Name = "User 4", RoleId = 2 });
        _repositoryContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _repositoryContext.Database.EnsureDeleted();
    }

    /// <summary>
    /// Given there are no users who are assigned to the role being deleted, when the role is deleted, then
    /// there is no need to assign a fallback role to the users who are assigned to the role being deleted,
    /// and the role is deleted.
    /// </summary>
    [Test]
    public async Task GivenNoUsersAreAssignedRoleBeingDeleted_WhenDeleteRole_Return()
    {
        //no need to pass the fallbackRole
        await _userService.DeleteRoleAsync("Role 3", null);
        var affectedUsers = await _repositoryContext.Users.Where(s => s.RoleId == null).ToListAsync();
        Assert.IsEmpty(affectedUsers);
        var deletedRole3 = await _repositoryContext.Roles.FirstOrDefaultAsync(s => s.Name == "Role 3");
        Assert.IsNull(deletedRole3);

        //pass the fallbackRole but no user affected
        await _userService.DeleteRoleAsync("Role 4", "Role 5");
        var role5Users = await _repositoryContext.Users.Where(s => s.Role != null && s.Role.Name.Trim().ToLower() == "Role 5".Trim().ToLower()).ToListAsync();
        Assert.IsEmpty(affectedUsers);
        var deletedRole4 = await _repositoryContext.Roles.FirstOrDefaultAsync(s => s.Name == "Role 4");
        Assert.IsNull(deletedRole4);
    }

    // List any other test cases as comments that you can think of.

    [Test]
    public async Task DeleteRoleIsEmpty_WhenDeleteRole_ThrowException()
    {
        try
        {
            await _userService.DeleteRoleAsync("", "Role 2");
        }
        catch (Exception ex)
        {
            Assert.AreEqual("The roleToDelete cannot be null or empty.", ex.Message);
        }
    }

    [Test]
    public async Task ThereAreUsersAreAssignedRoleButFallbackRokeIsEmpty_WhenDeleteRole_ThrowException()
    {
        try
        {
            await _userService.DeleteRoleAsync("Role 2", "");
        }
        catch (Exception ex)
        {
            Assert.AreEqual("The fallbackRole cannot be null or empty.", ex.Message);
        }
    }

    [Test]
    public async Task DeleteRoleEqualToFallbackRole_WhenDeleteRole_ThrowException()
    {
        try
        {
            await _userService.DeleteRoleAsync("Role 2", "Role 2");
        }
        catch (Exception ex)
        {
            Assert.AreEqual("The roleToDelete cannot be the same as fallbackRole.", ex.Message);
        }
    }

    [Test]
    public async Task DeleteRoleDoesNotExist_WhenDeleteRole_ThrowException()
    {
        try
        {
            await _userService.DeleteRoleAsync("Role 999", null);
        }
        catch (Exception ex)
        {
            Assert.AreEqual("The roleToDelete does not exist.", ex.Message);
        };
    }

    [Test]
    public async Task FallbackRoleDoesNotExist_WhenDeleteRole_ThrowException()
    {
        try
        {
            await _userService.DeleteRoleAsync("Role 2", "Role 999");
        }
        catch (Exception ex)
        {
            Assert.AreEqual("The fallbackRole does not exist.", ex.Message);
        };
        Assert.Pass();
    }

    [Test]
    public async Task ThereAreUsersAreAssignedRoleBeingDeleted_WhenDeleteRole_Return()
    {
        await _userService.DeleteRoleAsync("Role 2", "Role 5");
        var affectedUsers = await _repositoryContext.Users.Where(s => s.Role != null && s.Role.Name.Trim().ToLower() == "Role 5".Trim().ToLower()).ToListAsync();
        Assert.AreEqual(2, affectedUsers.Count);
        var deletedRole3 = await _repositoryContext.Roles.FirstOrDefaultAsync(s => s.Name == "Role 2");
        Assert.IsNull(deletedRole3);
    }
}