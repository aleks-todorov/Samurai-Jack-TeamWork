using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Model
{
    public class Image
    {
        public int ImageId { get; set; }

        public string ImageName { get; set; }

        public string URL { get; set; }

        public string Path { get; set; }

        public int AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
