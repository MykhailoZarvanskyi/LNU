using System;
using System.Threading.Tasks;
using WpfApp2.Services; 
using DAL.Interface;    
using DTO;             

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRoleDAL _userRoleDal; 

    public AuthenticationService(IUserRoleDAL userRoleDal)
    {
        _userRoleDal = userRoleDal; 
    }

    public async Task<UserRole> AuthenticateAsync(string username, string password)
    {
        try
        {
            
            var userRole = await _userRoleDal.GetByCredentialsAsync(username, password);
            return userRole; 
        }
        catch (Exception ex)
        {
            
            LogError(ex); 
            return null; 
        }
    }

    private void LogError(Exception ex)
    {
        
        Console.WriteLine($"Error during authentication: {ex.Message}");
        
    }
}
