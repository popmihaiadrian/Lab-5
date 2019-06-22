using Lab_2_webapi.Models;
using Lab_2_webapi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2_webapi.Services
{
    public interface IUsersService
    {
        UserGetModel Authenticate(string username, string password);
        UserGetModel Register(RegisterPostModel registerInfo);
        IEnumerable<UserGetModel> GetAll();
        User Create(UserPostModel userPostModel,User user);
        User GetCurentUser(HttpContext httpContext);
        object Upsert(int id, UserPostModel userPostModel, User addedBy);
        object Delete(int id);
    }

    public class UsersService : IUsersService
    {
        private TasksDbContext context;
        private readonly AppSettings appSettings;

        public UsersService(TasksDbContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;

        }

        public UserGetModel Authenticate(string username, string password)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Username == username &&
                x.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new UserGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                UserRole=user.UserRole.ToString(),
                Token = tokenHandler.WriteToken(token)
            };

            // remove password before returning

            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public UserGetModel Register(RegisterPostModel registerInfo)
        {
            User existing = context.Users.FirstOrDefault(u => u.Username == registerInfo.Username);
            if (existing != null)
            {
                return null;
            }

            context.Users.Add(new User
            {
                Email = registerInfo.Email,
                LastName = registerInfo.LastName,
                FirstName = registerInfo.FirstName,
                Password = ComputeSha256Hash(registerInfo.Password),
                Username = registerInfo.Username,
                UserRole = UserRole.Regular,
                CreatedAt = DateTime.Today
            }) ;
            context.SaveChanges();
            return Authenticate(registerInfo.Username, registerInfo.Password);
        }

       

        public IEnumerable<UserGetModel> GetAll()
        {
            // return users without passwords
            return context.Users
                .Select(user => new UserGetModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    UserRole = user.UserRole.ToString(),
                    Token = null
                }) ;
        }

        public User Create(UserPostModel userPostModel,User addedby)
        {
            User toAdd = UserPostModel.ToUser(userPostModel);
            toAdd.Password = ComputeSha256Hash(toAdd.Password);
            toAdd.CreatedBy = addedby.Username;
            toAdd.CreatedByRole = addedby.UserRole.ToString();
            context.Users.Add(toAdd);
            context.SaveChanges();
            return toAdd;
        }

        public User GetCurentUser(HttpContext httpContext)
        {
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            //string accountType = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod).Value;
            //return _context.Users.FirstOrDefault(u => u.Username == username && u.AccountType.ToString() == accountType);
            return context.Users.FirstOrDefault(u => u.Username == username);
        }

        public object Upsert(int id, UserPostModel userPostModel, User requestedBy)
        {
            var existing = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                User toAdd = UserPostModel.ToUser(userPostModel);
                context.Users.Add(toAdd);
                context.SaveChanges();
                return toAdd;
            }

            User toUpdate = UserPostModel.ToUser(userPostModel);
            toUpdate.CreatedAt = existing.CreatedAt;
            toUpdate.Id = id;
            toUpdate.Password = ComputeSha256Hash(toUpdate.Password);
            if (requestedBy.UserRole.Equals(UserRole.User_Manager))
            {
                toUpdate.UserRole = existing.UserRole;
            }
            context.Users.Update(toUpdate);
            context.SaveChanges();
            return toUpdate;
        }

        public object Delete(int id)
        {
            var existing = context.Users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                return null;
            }

            context.Users.Remove(existing);
            context.SaveChanges();

            return existing;
        }
    }
    }