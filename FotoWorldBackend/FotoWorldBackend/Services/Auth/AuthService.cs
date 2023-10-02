using FotoWorldBackend.Models;
using FotoWorldBackend.Models.AuthModels;
using FotoWorldBackend.Services.Email;
using FotoWorldBackend.Utilities;


namespace FotoWorldBackend.Services.Auth
{
    /// <summary>
    /// Provides operations on database for auth controller
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly FotoWorldContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        public AuthService(FotoWorldContext context, IConfiguration config )
        {
            _context = context;
            _config = config;
        }


        public User LoginUser(LoginModel login)
        {
            var user = _context.Users.FirstOrDefault(m => m.Email == login.Username || m.Username == login.Username);
            if (user != null)
            {
                if (user.IsActive)
                {
                    if (PasswordHash.VerifyPassword(login.Password, user))
                    {
                        return user;
                    }
                }

            }
            return null;
        }


        public User RegisterUser(RegisterUserModel register)
        {
            if (_context.Users.FirstOrDefault(m => m.Email == register.Email) == null)
            {
                if (register.Password == register.RepeatPassword)
                {
                    var newUser = new User();
                    try
                    {
                        
                        newUser.Username = register.Username;
                        newUser.Email = register.Email;
                        newUser.PhoneNumber = register.PhoneNumber;
                        newUser.PasswordSalt = PasswordHash.GenerateSalt();
                        newUser.HashedPassword = PasswordHash.HashPassword(register.Password, newUser.PasswordSalt);
                        newUser.IsOperator = register.isOperator;
                        newUser.IsActive = false;
                        _context.Users.Add(newUser);
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine("Error while creating new user\n"+ex.ToString());
                        return null;
                    }

                    

                    if (newUser.IsOperator)
                    {
                        var newOperator = new FotoWorldBackend.Models.Operator();
                        try
                        {
                            
                            newOperator.AccountId = newUser.Id;
                            newOperator.IsCompany = register.IsCompany;
                            newOperator.Availability = register.Availability;
                            newOperator.LocationCity = register.LocationCity;
                            newOperator.OperatingRadius = register.OperatingRadius;
                            newOperator.Photo = register.PhotoService;
                            newOperator.DronePhoto = register.DronePhotoService;
                            newOperator.Filming = register.FilmingService;
                            newOperator.DroneFilming = register.DroneFilmService;

                            _context.Operators.Add(newOperator);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Error while creating new operator\n"+ex.ToString());
                            return null;
                        }

                        

                    }

                    try
                    {
                        _context.SaveChanges();
                    }catch(Exception ex)
                    {
                        Console.WriteLine("Error while saving new user to database\n"+ex.ToString());
                        return null;
                    }
                    
                    return newUser;
                }


            }
            return null;
        }


        public bool ActivateUser(int id)
        {
            User userToActivate = _context.Users.FirstOrDefault(m => m.Id == id);
            if (userToActivate != null)
            {
                try
                {
                    userToActivate.IsActive = true;
                    _context.SaveChanges();

                }catch(Exception ex)
                {
                    Console.WriteLine("Error while activating user\n"+ex.ToString());
                    return false;
                }

                return true;
            }

            return false;
        }




       public bool RestartPassword(int userID, string newPassword)
       {
            User user= _context.Users.FirstOrDefault(m=> m.Id== userID);
            if(user != null)
            {
                try
                {
                    user.PasswordSalt = PasswordHash.GenerateSalt();
                    user.HashedPassword = PasswordHash.HashPassword(newPassword, user.PasswordSalt);
                    _context.SaveChanges();
                }catch(Exception ex) { 
                    Console.WriteLine("Error while changing user password\n"+ex.ToString());
                    return false;   
                }

                return true;
            }
            return false;
       }



        public User GetUserByMail(string email) {
            var user=_context.Users.FirstOrDefault(m => m.Email == email);
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}
