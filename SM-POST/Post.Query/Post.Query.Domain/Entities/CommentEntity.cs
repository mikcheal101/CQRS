using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Entities
{
    [Table("Comment")]
    public class CommentEntity
    {
        [Key]
        public Guid CommentId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public DateTime CommentedDate { get; set; }
        public bool IsEdited { get; set; }
        public Guid PostId { get; set; }

        // avoiding circular reference
        [JsonIgnore]
        public virtual PostEntity Post { get; set; }
    }
}