using FotoWorldBackend.Models;
using FotoWorldBackend.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace FotoWorldBackend.Services.UserS
{
    public interface IUserService
    {
        /// <summary>
        /// Returns detailed info about offer of given ID
        /// </summary>
        /// <param name="offerId">ID of offer we want info about</param>
        /// <returns>Offer info</returns>
        public OfferWithPhotos GetOfferDetailed(int offerId);

        /// <summary>
        /// Returns List of offers that is displayed on offer wall
        /// </summary>
        /// <returns>List of all offers</returns>
        public List<OfferListObject> GetOfferList();


        /// <summary>
        /// Adds offer to followed
        /// </summary>
        /// <param name="offerId">Offer that we want to follow</param>
        /// <param name="userId">Cipher of user Id that follows offer</param>
        /// <returns>If operation was successful</returns>
        public bool FollowOffer(int offerId, string userId);

        /// <summary>
        /// Removes offer from followed
        /// </summary>
        /// <param name="offerId">Offer what we want to unfollow</param>
        /// <param name="userId">Cipher of user Id that unfollow offer</param>
        /// <returns>If operation was successful</returns>
        public bool UnfollowOffer(int offerId, string userId);

        /// <summary>
        /// Creates operator opinion
        /// </summary>
        /// <param name="opinion">Opinion form</param>
        /// <param name="offerId">Offer that we want to rate</param>
        /// <param name="userId">User that rates offer/operator</param>
        /// <returns>If operation was successful</returns>
        public bool CreateOperatorOpinion(CreateOperatorOpinionModel opinion, int offerId, string userId);

        /// <summary>
        /// Removes operator opinion
        /// </summary>
        /// <param name="offerId">Offer of which opinion we want to remove</param>
        /// <param name="userId">Cipher of user It that removes opinion</param>
        /// <returns>If operation was successful</returns>
        public bool RemoveOperatorOpinion(int offerId, string userId);

        /// <summary>
        /// Returns photo of given id(used to retrieve photos in detailed offer view)
        /// </summary>
        /// <param name="id">Id of photo we want to retrieve</param>
        /// <returns>Photo of passed id</returns>
        FileStream GetImageById(int id);
    }
}
