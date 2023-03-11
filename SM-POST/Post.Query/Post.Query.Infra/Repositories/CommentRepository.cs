using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infra.DBAccess;

namespace Post.Query.Infra.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DbContextFactory _databaseContextFactory;

        public CommentRepository(DbContextFactory databaseContextFactory)
        {
            _databaseContextFactory = _databaseContextFactory;
        }

        public async Task CreateAsync(CommentEntity commetnEntity)
        {
            using DatabaseContext databaseContext = _databaseContextFactory.CreateDbContext();
            databaseContext.Comments.Add(commetnEntity);
            _ = await databaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext databaseContext = _databaseContextFactory.CreateDbContext();
            CommentEntity comment = await GetByIdAsync(commentId);

            if (comment == null)
            {
                return;
            }

            databaseContext.Comments.Remove(comment);
            _ = await databaseContext.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using DatabaseContext databaseContext = _databaseContextFactory.CreateDbContext();
            return await databaseContext.Comments.FirstOrDefaultAsync(comment => comment.CommentId == commentId);
        }

        public async Task UpdateAsync(CommentEntity commentEntity)
        {
            using DatabaseContext databaseContext = _databaseContextFactory.CreateDbContext();
            CommentEntity comment = await GetByIdAsync(commentEntity.CommentId);

            if (comment == null)
            {
                return;
            }

            databaseContext.Comments.Update(commentEntity);
            _ = await databaseContext.SaveChangesAsync();
        }
    }
}