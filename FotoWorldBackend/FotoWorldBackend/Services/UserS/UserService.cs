using FotoWorldBackend.Models;
using FotoWorldBackend.Models.UserModels;
using FotoWorldBackend.Services.Email;
using FotoWorldBackend.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FotoWorldBackend.Services.UserS
{
    public class UserService : IUserService
    {
        private readonly FotoWorldContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public UserService(FotoWorldContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public bool FollowOffer(int offerId, string userId)
        {

            try
            {
                var userToFavourite = _context.Users.FirstOrDefault(m => m.Id == Convert.ToInt32(SymmetricEncryption.Decrypt(_config["SECRET_KEY"], userId)));
                if(userToFavourite == null)
                {
                    return false;
                }
                var followed = new FollowedOffer
                {
                    OfferId = offerId,
                    UserId = userToFavourite.Id
                };

                _context.FollowedOffers.Add(followed);
                _context.SaveChanges();
                return true;
            } catch(Exception ex)
            {
                Console.WriteLine("Error while adding offer to favourites\n"+ex.ToString());
                return false;   
            }

        }

        public bool CreateOperatorOpinion(CreateOperatorOpinionModel opinion, int offerId, string userId)
        {
            try
            {
                var offer = _context.Offers.FirstOrDefault(m => m.Id == offerId);
                var reviewer = _context.Users.FirstOrDefault(m => m.Id == Convert.ToInt32(SymmetricEncryption.Decrypt(_config["SECRET_KEY"], userId)));
                if (offer == null || reviewer == null)
                {
                    return false;
                }

                var checkIfExist = _context.OperatorRatings.FirstOrDefault(m => m.UserId == reviewer.Id && m.OperatorId == offer.OperatorId);
                if(checkIfExist != null)
                {
                    Console.WriteLine("This user already left an opinion");
                    return false;
                }


                var review = new OperatorRating
                {
                    Stars = opinion.Stars,
                    Comment = opinion.Comment,
                    UserId = reviewer.Id,
                    OperatorId = offer.OperatorId

                };

                _context.OperatorRatings.Add(review);
                _context.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine("Error while leaving operator review\n"+ex.ToString());
                return false;
            }

        }

        public FileStream GetImageById(int id)
        {
            try
            {
                var photo = _context.Photos.FirstOrDefault(m => m.Id == id);
                var image = System.IO.File.OpenRead(photo.PhotoUrl);
                return image;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting image\n"+ex.Message);
                return null;
            }

        }

        public OfferWithPhotos GetOfferDetailed(int offerId)
        {
            var offer = _context.Offers.FirstOrDefault(o => o.Id == offerId);
            if (offer == null)
            {
                return null;
            }


            var oper = _context.Operators.FirstOrDefault(o => o.Id == offer.OperatorId);
            var operAcc = _context.Users.FirstOrDefault(o => o.Id == oper.AccountId);
            var cont = new List<string>
            {
                operAcc.Email,
                operAcc.PhoneNumber
            };


            var photos = _context.OfferPhotos.Where(m => m.OfferId == offerId).ToList();
            
            var attachments = new List<int>();

            foreach (var photo in photos)
            {
                attachments.Add(photo.Id);
            }

            var ret = new OfferWithPhotos {
                Description = offer.Description,
                Title = offer.Title,
                OperatorId = offer.OperatorId,
                OperatorName = operAcc.Username,
                OperatorContact = cont,
                PhotosId = attachments,
                OfferId = offerId
            
            };


            return ret;

        }

        public List<OfferListObject> GetOfferList()
        {
            var offers= _context.Offers.ToList();
            if(offers == null)
            {
                return null;
            }
            var ret = new List<OfferListObject>();
            foreach (var offer in offers)
            {
                try
                {

                    var oper = _context.Operators.FirstOrDefault(o => o.Id == offer.OperatorId);
                    var operAcc = _context.Users.FirstOrDefault(o => o.Id == oper.AccountId);
                    var x = new OfferListObject
                    {
                        OfferId = offer.Id,
                        Title = offer.Title,
                        OperatorName = operAcc.Username
                    };

                    ret.Add(x);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return ret;
        }

        public bool UnfollowOffer(int offerId, string userId)
        {

            try
            {
                var offer = _context.Offers.FirstOrDefault(m => m.Id == offerId);
                var userToFavourite = _context.Users.FirstOrDefault(m => m.Id == Convert.ToInt32(SymmetricEncryption.Decrypt(_config["SECRET_KEY"], userId)));

                var favourite = _context.FollowedOffers.FirstOrDefault(m => m.OfferId == offer.Id && m.UserId == userToFavourite.Id);
                if (favourite == null)
                {
                    return false;
                }

                _context.FollowedOffers.Remove(favourite);
                _context.SaveChanges();
                return true;
            } catch(Exception ex)
            {
                Console.WriteLine("Error while removing offer from followed\n"+ex.Message);
                return false;
            }


        }

        public bool RemoveOperatorOpinion(int offerId, string userId)
        {

            try
            {
                var offer = _context.Offers.FirstOrDefault(m => m.Id == offerId);
                var reviewer = _context.Users.FirstOrDefault(m => m.Id == Convert.ToInt32(SymmetricEncryption.Decrypt(_config["SECRET_KEY"], userId)));

                var review = _context.OperatorRatings.FirstOrDefault(m => m.UserId == reviewer.Id && m.OperatorId == offer.OperatorId);

                if (review == null)
                {
                    return false;
                }


                _context.OperatorRatings.Remove(review);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error while removing operator review\n"+ex.Message);
                return false;
            }


        }
    }
}
