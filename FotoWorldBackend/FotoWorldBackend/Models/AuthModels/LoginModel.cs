namespace FotoWorldBackend.Models.AuthModels
{
    public class LoginModel
    {
        public bool LoginAsOperator { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
