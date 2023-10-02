namespace FotoWorldBackend.Models.AuthModels
{
    public class RegisterUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public bool isOperator { get; set; }

        public bool IsCompany { get; set; }
        public string Availability { get; set; }
        public string LocationCity { get; set; }

        public int OperatingRadius { get; set; }

        public bool PhotoService { get; set; }
        public bool DronePhotoService { get; set; }
        public bool DroneFilmService { get; set; }
        public bool FilmingService { get; set; }
    }
}