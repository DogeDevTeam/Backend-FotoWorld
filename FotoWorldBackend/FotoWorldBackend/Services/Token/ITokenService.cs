using FotoWorldBackend.Models;

namespace FotoWorldBackend.Services.Token
{
    public interface ITokenService
    {

        /// <summary>
        /// Generates token for given user
        /// </summary>
        /// <param name="user">User data</param>
        /// <param name="isOperator">Determins if user should be loged as operator or not</param>
        /// <returns>JWT </returns>
        public string GenerateToken(User user,  bool isOperator);
    }
}
