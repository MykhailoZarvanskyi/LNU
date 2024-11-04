using System.Threading.Tasks;
using DTO; 

namespace WpfApp2.Services
{
    public interface IAuthenticationService
    {
        Task<UserRole> AuthenticateAsync(string username, string password); 
    }
}
