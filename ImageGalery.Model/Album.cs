using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ImageGallery.Model
{
    public class Album
    {
        public int AlbumId { get; set; }

        public Album ParentId { get; set; }

        public string AlbumName { get; set; }

        public string Path { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
