
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
namespace ImageGallery.Model
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string Content { get; set; }

        public int AuthorId { get; set; }

        public int ImageId { get; set; }
    }
}
