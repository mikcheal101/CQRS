using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infra.DBAccess;

namespace Post.Query.Infra.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DbContextFactory _contextFactory;

        public PostRepository(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(PostEntity postEntity)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Add(postEntity);
            _ = await context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);

            if (post == null) return;

            context.Posts.Remove(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task<PostEntity> GetByIdAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(_post => _post.Comments)
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListByAuthorAsync(string author)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(_post => _post.Comments).AsNoTracking()
                .Where(_post => _post.Author.Contains(author, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithCommentsAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(_post => _post.Comments).AsNoTracking()
                .Where(_post => _post.Comments != null && _post.Comments.Any())
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(_post => _post.Comments).AsNoTracking()
                .Where(_post => _post.LikesCount >= numberOfLikes)
                .ToListAsync();
        }

        public async Task UpdateAsync(PostEntity postEntity)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Update(postEntity);
            _ = await context.SaveChangesAsync();
        }
    }
}