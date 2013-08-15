using ImageGallery.Model;

namespace ImageGallery.Services.Models
{
    public class ImageModel
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string URL { get; set; }
        public string Path { get; set; }
        public int AlbumId { get; set; }
    }
}


