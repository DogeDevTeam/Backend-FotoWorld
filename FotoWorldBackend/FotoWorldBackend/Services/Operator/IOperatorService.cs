using FotoWorldBackend.Models;
using FotoWorldBackend.Models.OperatorModels;

namespace FotoWorldBackend.Services.Operator
{
    public interface IOperatorService
    {
        /// <summary>
        /// Creates offer
        /// </summary>
        /// <param name="offer">offer data</param>
        /// <param name="authorId">encoded operator id</param>
        /// <returns>new offer</returns>
        public Offer CreateOffer(CreateOfferModel offer, string authorId);

        /// <summary>
        /// Edits offer with given id
        /// </summary>
        /// <param name="newOffer">edited data</param>
        /// <param name="authorId">encoded operator id</param>
        /// <param name="oldOfferId">id of eddited offer</param>
        /// <returns>edited offer</returns>
        public Offer UpdateOffer(CreateOfferModel newOffer, string authorId, int oldOfferId);

        /// <summary>
        /// saves photos passed in offer form
        /// </summary>
        /// <param name="offer">offer data</param>
        /// <returns>list of new photos ids</returns>
        public List<int> UploadPhotos(CreateOfferModel offer);


        /// <summary>
        /// Removes offer with given Id
        /// </summary>
        /// <param name="offerId">offer id</param>
        /// <param name="authorId">encrypted operator id</param>
        /// <returns>if operation was successful</returns>
        public bool RemoveOffer(int offerId, string authorId);
    }
}
