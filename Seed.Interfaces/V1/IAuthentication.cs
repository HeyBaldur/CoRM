using Seed.Models.V1.DTOs;
using Seed.Models.V1.Models;
using Seed.Services.Abstract;
using System.Threading.Tasks;

namespace Seed.Interfaces.V1
{
    public interface IAuthentication
    {
        /// <summary>
        /// Create a new account into the system
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResultOperationResponse<User>> CreateAccount(UserRequestDto user, string password);
        
        /// <summary>
        /// Access account
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<ResultOperationResponse<object>> AccessAccount(string emailAddress, string password);
        
        /// <summary>
        /// Get a single user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Task<ResultOperationResponse<User>> GetUser(int userId, string emailAddress);
        
        /// <summary>
        /// Validate if the user exists
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Task<ResultOperationResponse<bool>> EmailExist(string emailAddress);
    }
}
