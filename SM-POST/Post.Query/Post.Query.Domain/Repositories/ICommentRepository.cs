using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentEntity commetnEntity);
        Task DeleteAsync(Guid commentId);
        Task UpdateAsync(CommentEntity commentEntity);
        Task<CommentEntity> GetByIdAsync(Guid commentId);
    }
}