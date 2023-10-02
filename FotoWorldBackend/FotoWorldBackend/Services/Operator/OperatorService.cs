using FotoWorldBackend.Models;
using FotoWorldBackend.Models.OperatorModels;
using FotoWorldBackend.Services.Email;
using FotoWorldBackend.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Security.Cryptography;

namespace FotoWorldBackend.Services.Operator
{

    /// <summary>
    /// Provides operations on database for operator controller
    /// </summary>
    public class OperatorService : IOperatorService
    {
        private readonly FotoWorldContext _context;
        private readonly IConfiguration _config;
        public OperatorService(FotoWorldContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public Offer CreateOffer(CreateOfferModel offer, string authorId)
        {
            var photosID = UploadPhotos(offer); 
            if(photosID != null)
            {
                var newOffer = new Offer();

                var authorOperator = _context.Operators.FirstOrDefault(m => m.AccountId == Convert.ToInt32(SymmetricEncryption.Decrypt(_config["SECRET_KEY"], authorId)));

                newOffer.OperatorId = authorOperator.Id;
                newOffer.Title= offer.Title;
                newOffer.Description = offer.Description;


                try
                {
                    _context.Offers.Add(newOffer);
                }
                catch(Exception ex) {
                    Console.WriteLine("Error while createing offer in context\n" + ex.ToString());
                    return null;
                }
                
                try
                {
                    foreach (int id in photosID)
                    {
                        var offerPhoto = new OfferPhoto();
                        offerPhoto.OfferId = newOffer.Id;
                        offerPhoto.PhotoId = id;

                        _context.OfferPhotos.Add(offerPhoto);
                        
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine("Error while connecting photos to offer\n"+ex.ToString());
                    return null;
                }

                try
                {
                    _context.SaveChanges();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error while saving new offer to database\n"+ex.ToString());
                    return null;
                }
                
                return newOffer;
            }
            Console.WriteLine("No photos included");
            return null;

        }

        public bool RemoveOffer(int offerId, string authorId)
        {
            var offerToRemove = _context.Offers.FirstOrDefault(m => m.Id == offerId);
            if (offerToRemove == null)
            {
                return false;
            }

            var authorIdDecypher = Convert.ToInt32(SymmetricEncryption.Decrypt(_config["SECRET_KEY"], authorId));
            var authorOperator = _context.Operators.FirstOrDefault(m => m.AccountId == authorIdDecypher);

            if (offerToRemove.OperatorId != authorOperator.Id)
            {
                return false;
            }


            var oldPhotos = _context.OfferPhotos.Where(m => m.OfferId == offerId).ToList();
            foreach (var photoOffer in oldPhotos)
            {
                try
                {
                    var photo = _context.Photos.FirstOrDefault(m => m.Id == photoOffer.Id);

                    File.Delete(photo.PhotoUrl);

                    _context.OfferPhotos.Remove(photoOffer);
                    _context.Photos.Remove(photo);
                }
                catch(Exception ex ) { 
                    Console.WriteLine("Error while removing photos\n"+ex.ToString());
                    return false;
                }

            }

            try
            {
                _context.Offers.Remove(offerToRemove);
            }catch(Exception ex) { 
                
                Console.WriteLine("Error while removing offer\n"+ex.ToString());
                return false;
            }



            try
            {
                _context.SaveChanges();
            }
            catch
            (Exception ex)
            {
                Console.WriteLine("Error while saving changes in removing offer\n"+ex.ToString());
                return false;
            }
            

            return true;
        }

        public Offer UpdateOffer(CreateOfferModel newOffer, string authorId, int oldOfferId)
        {
            var oldOffer = _context.Offers.FirstOrDefault(m => m.Id == oldOfferId);
            if(oldOffer == null)
            {
                return null;
            }

            var authorIdDecypher = Convert.ToInt32(SymmetricEncryption.Decrypt(_config["SECRET_KEY"], authorId));
            var authorOperator = _context.Operators.FirstOrDefault(m => m.AccountId == authorIdDecypher);

            if ( oldOffer.OperatorId != authorOperator.Id)
            {
                Console.WriteLine("Incorrect Operator");
                return null;
            }

            //if edit change photos then remove old and add newOnes
            if (newOffer.Photos != null)
            {
                try
                {
                    //remove old photos
                    var oldPhotos = _context.OfferPhotos.Where(m => m.OfferId == oldOfferId).ToList();
                    foreach (var photoOffer in oldPhotos)
                    {
                        var photo = _context.Photos.FirstOrDefault(m => m.Id == photoOffer.Id);

                        File.Delete(photo.PhotoUrl);

                        _context.OfferPhotos.Remove(photoOffer);
                        _context.Photos.Remove(photo);

                        
                    }
                }
                catch  (Exception ex) { 
                    Console.WriteLine("Error while removing old photos\n"+ex.ToString());
                    return null;
                }


                //upload new photos
                var photosID = UploadPhotos(newOffer);
                if(photosID== null)
                {
                    Console.WriteLine("No edited photos\n");
                    return null;
                }

                try
                {
                    //connect new ones to offer
                    foreach (int id in photosID)
                    {
                        var offerPhoto = new OfferPhoto();
                        offerPhoto.OfferId = oldOfferId;
                        offerPhoto.PhotoId = id;

                        _context.OfferPhotos.Add(offerPhoto);

                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error while connecting photos to offer\n"+ex.ToString());
                    return null;
                }


            }

            //update text
            oldOffer.Title = newOffer.Title;
            oldOffer.Description = newOffer.Description;
            try
            {
                _context.SaveChanges();
            } catch(Exception ex)
            {
                Console.WriteLine("Error while saving edited offer\n"+ex.ToString());
                return null;
            }
            
            return oldOffer;
        }

        public List<int> UploadPhotos(CreateOfferModel offer)
        {
            var ret= new List<int>();
            var baseUrl = _config["UploadsDirectory"];
            foreach (var photo in offer.Photos) {

                string newFileName = Convert.ToString( Guid.NewGuid()) + Path.GetExtension(photo.FileName);
                string filePath = Path.Combine(baseUrl, newFileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyToAsync(stream);
                    }
                }catch(Exception ex) { 
                    Console.WriteLine("Error while uploading photos\n"+ex.ToString());
                    return null;
                }

                var databasePhoto = new Photo();
                databasePhoto.PhotoUrl = filePath;
                try
                {
                    _context.Photos.Add(databasePhoto);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error while adding photos to database\n"+ex.ToString());
                    return null;
                }
                
                ret.Add(databasePhoto.Id);
            }
            try
            {
                _context.SaveChanges();

            }catch(Exception ex)
            {
                Console.WriteLine("Error while saving photos in database\n"+ex.ToString());
                return null;
            }
            return ret;

        }
    }
}
