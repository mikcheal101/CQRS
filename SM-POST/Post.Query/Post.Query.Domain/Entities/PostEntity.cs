using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities
{
    [Table("Post")]
    public class PostEntity
    {
        [Key]
        public Guid PostId { get; set; }
        public string Author { get; set; }
        public DateTime PostedDate { get; set; }
        public int LikesCount { get; set; }
        public string Message { get; set; }
        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}