using FotoWorldBackend.Models;
using FotoWorldBackend.Models.AuthModels;

namespace FotoWorldBackend.Services.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Validates data and returns User
        /// </summary>
        /// <param name="login">Login Credentials</param>
        /// <returns>User with given credentials</returns>
        User LoginUser(LoginModel login);

        /// <summary>
        /// Registers new User or new Operator
        /// </summary>
        /// <param name="register">new account information</param>
        /// <returns></returns>
        User RegisterUser(RegisterUserModel register);

        /// <summary>
        /// Activates accont
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>if activation was sucessful</returns>
        bool ActivateUser(int id);

        /// <summary>
        /// Returns User with given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User GetUserByMail(string email);

        /// <summary>
        /// restarts password 
        /// </summary>
        /// <param name="userID">id of user that want to reset password</param>
        /// <param name="newPassword">new password</param>
        /// <returns></returns>
        bool RestartPassword(int userID, string newPassword);
    }
}
