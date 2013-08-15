using ImageGallery.Model;
using System.Collections.Generic;

namespace ImageGallery.Services.Models
{
    public class AlbumModel
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public int ImagesCount { get; set; }
        public int AlbumCount { get; set; }
    }
}