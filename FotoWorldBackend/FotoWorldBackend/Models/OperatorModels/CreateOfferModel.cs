namespace FotoWorldBackend.Models.OperatorModels
{
    public class CreateOfferModel
    {
        public string Title { get; set; }

        public string Description { get; set; } 

        public List<IFormFile>? Photos { get; set; }
    }
}
