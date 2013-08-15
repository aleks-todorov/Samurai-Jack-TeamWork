using System.Collections.Generic;
using ImageGallery.Model;

namespace ImageGallery.Services.Models
{
    public class AlbumFullModel
    {
        public int AlbumID { get; set; }
        public string AlbumName { get; set; }
        public int UserId { get; set; }
        public virtual IEnumerable<ImageModel> Images { get; set; }
        public virtual IEnumerable<AlbumModel> Albums { get; set; }
    }
}