using Microsoft.EntityFrameworkCore;
using TechnicalTest.Entities;
using TechnicalTest.Services.Exceptions;

namespace TechnicalTest.Services.Services;

public class UserService
{
    private readonly RepositoryContext _repositoryContext;

    public UserService(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    /// <summary>
    /// Deletes a <see cref="Role"/>. If and only if the <see cref="Role"/> is not assigned to any <see cref="User"/>, then it is deleted.
    /// Otherwise, a fallback role must be specified. All <see cref="User"/>s with the <see cref="Role"/> to delete will be assigned the fallback role.
    /// </summary>
    /// <param name="roleToDelete">Name of the <see cref="Role"/> to delete.</param>
    /// <param name="fallbackRole">Name of the <see cref="Role"/> to be assigned to the <see cref="User"/> whose current <see cref="Role"/> name is <see cref="roleToDelete"/>.</param>
    /// <exception cref="InvalidInputException">.</exception>
    /// <exception cref="RoleDoesNotExistException">.</exception>
    /// <returns>.</returns>
    public async Task DeleteRoleAsync(string roleToDelete, string? fallbackRole)
    {
        // If roleToDelete is the same as fallbackRole, then throw an exception.
        if (!string.IsNullOrEmpty(fallbackRole) && roleToDelete.Trim().ToLower() == fallbackRole.Trim().ToLower())
        {
            throw new InvalidInputException("The roleToDelete cannot be the same as fallbackRole.");
        }
        var deleteRole = await GetRoleAsync(roleToDelete, "roleToDelete");

        // get the list of users associated with roleToDelete.
        var usersAssociatedWithRoleToDelete = await _repositoryContext.Users
            // .AsNoTracking()
            .Where(user => user.RoleId == deleteRole.Id).ToListAsync();

        // If the usersAssociatedWithRoleToDelete count is greater than zero, assign the fallbackRole to the users.
        if (usersAssociatedWithRoleToDelete.Any())
        {
            //only need to validate and get fallbackRole if there are users has roleToDelete
            var fallbackRoleEntity =await GetRoleAsync(fallbackRole, "fallbackRole");
            foreach (var user in usersAssociatedWithRoleToDelete)
            {
                user.RoleId = fallbackRoleEntity.Id;
            }
        }
        _repositoryContext.Remove(deleteRole);
        await _repositoryContext.SaveChangesAsync();
    }

    private async Task<Role> GetRoleAsync(string? roleName, string roleType)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new InvalidInputException($"The {roleType} cannot be null or empty.");
        }
        var role = await _repositoryContext.Roles.FirstOrDefaultAsync(roleEntity => roleEntity.Name.Trim().ToLower() == roleName.Trim().ToLower());

        // If rolee does not exist, then throw an exception.
        if (role == null)
        {
            throw new RoleDoesNotExistException(message: $"The {roleType} does not exist.");
        }
        return role;
    }
}