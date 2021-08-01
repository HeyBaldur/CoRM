using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Seed.Interfaces.V1;
using Seed.Models.V1.DTOs;
using Seed.Models.V1.Models;
using Seed.Repositories.Data;
using Seed.Services;
using Seed.Services.Abstract;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Repositories.Repositories
{
    public class AuthenticationRepository: IAuthentication
    {
        private DataContext _dataContext { get; set; }
        private readonly PasswordHash PasswordHash = new PasswordHash();
        private readonly IConfiguration _iConfiguration;
        private readonly IMapper _mapper;
        public AuthenticationRepository(
            DataContext _dataContext,
            IConfiguration iConfiguration,
            IMapper mapper)
        {
            this._dataContext = _dataContext;
            _iConfiguration = iConfiguration;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResultOperationResponse<User>> CreateAccount(UserRequestDto user, string password)
        {
            try
            {
                var passwordHandler = new PasswordHash();
                byte[] passwordHash, passwordSalt;
                passwordHandler.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                var userToCreate = _mapper.Map<User>(user);
                userToCreate.PasswordHash = passwordHash;
                userToCreate.PasswordSalt = passwordSalt;

                userToCreate.Uid = Guid.NewGuid().ToString();
                await _dataContext.Users.AddAsync(userToCreate);
                await _dataContext.SaveChangesAsync();

                return new ResultOperationResponse<User>(userToCreate, "Success");
            }
            catch (Exception ex)
            {
                return new ResultOperationResponse<User>(true, ex.Message);
            }
        }

        /// <summary>
        /// Access to the user's account
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<ResultOperationResponse<object>> AccessAccount(string emailAddress, string password)
        {
            try
            {
                var userFromRepo = await _dataContext.Users
                .Where(u => u.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();
                if (userFromRepo == null)
                    return new ResultOperationResponse<object>(true, "User does not exist.");

                if (!PasswordHash.VerifyPassword(password, userFromRepo.PasswordHash, userFromRepo.PasswordSalt))
                    return new ResultOperationResponse<object>(true, "Password does not match.");

                // Set claims
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.EmailAddress)
                };

                // Set key
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iConfiguration.GetSection("AppSettings:Token").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var userMapped = _mapper.Map<UserResponseDto>(userFromRepo);

                object userToReturn = new
                {
                    Token = tokenHandler.WriteToken(token),
                    User = userMapped
                };

                return new ResultOperationResponse<object>(userToReturn, "Success");
            }
            catch (Exception ex)
            {
                return new ResultOperationResponse<object>(true, ex.Message);
            }
        }

        /// <summary>
        /// Get a single user information
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<ResultOperationResponse<User>> GetUser(int userId, string emailAddress)
        {
            try
            {
                var userToReturn = await _dataContext.Users
                .Where(u => u.Id == userId && u.EmailAddress == emailAddress && u.AccountConfirmed)
                .FirstOrDefaultAsync();
                if (userToReturn == null)
                    return null;

                return new ResultOperationResponse<User>(userToReturn, "Success");
            }
            catch (Exception ex)
            {
                return new ResultOperationResponse<User>(true, ex.Message);
            }
        }

        /// <summary>
        /// Validate if the email exists
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<ResultOperationResponse<bool>> EmailExist(string emailAddress)
        {
            if (await _dataContext.Users.
                AnyAsync(x => x.EmailAddress == emailAddress))
                return new ResultOperationResponse<bool>(true, "User exists.");
            return new ResultOperationResponse<bool>(false, "User does not exist.");
        }
    }
}
