using FotoWorldBackend.Models;


namespace FotoWorldBackend.Services.Email
{
    public interface IEmailService
    {
        public void SendEmail(EmailModel request);

        /// <summary>
        /// Sends activation email to given user
        /// </summary>
        /// <param name="user">user</param>
        public void SendActivationEmailUser(User user);

        /// <summary>
        /// sends restart password email to given user
        /// </summary>
        /// <param name="user">user</param>
        public void SendRestartPasswordEmail(User user);
    }
}
