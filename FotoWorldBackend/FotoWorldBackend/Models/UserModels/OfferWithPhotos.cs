using Microsoft.AspNetCore.Mvc;

namespace FotoWorldBackend.Models.UserModels
{
    public class OfferWithPhotos
    {
        public int OfferId { get; set; }

        public int OperatorId { get; set; }

        public string OperatorName { get; set; }

        public List<string> OperatorContact { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public List<int>? PhotosId { get; set; }
    }
}
