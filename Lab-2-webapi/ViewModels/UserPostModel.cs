using Lab_2_webapi.Models;

namespace Lab_2_webapi.ViewModels
{
    public class UserPostModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string Password { get; set; }

        public static User ToUser(UserPostModel userModel)
        {
            UserRole role = Models.UserRole.Regular;

            if (userModel.UserRole == "User_Manager")
            {
                role = Models.UserRole.User_Manager;
            }
            else if (userModel.UserRole == "Admin")
            {
                role = Models.UserRole.Admin;
            }

            return new User
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Username = userModel.UserName,
                Email = userModel.Email,
                UserRole = role,
                Password=userModel.Password,                CreatedAt = System.DateTime.Today
        };
        }
    }
}