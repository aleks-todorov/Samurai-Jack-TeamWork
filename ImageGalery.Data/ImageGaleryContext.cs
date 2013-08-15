using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ImageGallery.Model;

namespace ImageGallery.Data
{
    public class ImageGaleryContext : DbContext
    {
        public ImageGaleryContext()
            : base("ImageGaleryContext")
        {

        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
