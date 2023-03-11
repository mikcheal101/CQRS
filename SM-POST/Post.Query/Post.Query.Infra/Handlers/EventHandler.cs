using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Infra.Repositories;

namespace Post.Query.Infra.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly PostRepository _postRepository;
        private readonly CommentRepository _commentRepository;

        public EventHandler(PostRepository postRepository, CommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task On(PostCreatedEvent @event)
        {
            PostEntity postEntity = new()
            {
                PostId = @event.Id,
                Author = @event.Author,
                PostedDate = @event.DatePosted,
                Message = @event.Message
            };

            await _postRepository.CreateAsync(postEntity);
        }

        public async Task On(MessageUpdatedEvent @event)
        {
            PostEntity postEntity = await _postRepository.GetByIdAsync(@event.Id);

            if (postEntity == null)
            {
                return;
            }

            postEntity.Message = @event.Message;
            await _postRepository.UpdateAsync(postEntity);
        }

        public async Task On(PostLikedEvent @event)
        {
            PostEntity postEntity = await _postRepository.GetByIdAsync(@event.Id);

            if (postEntity == null)
            {
                return;
            }
            
            postEntity.LikesCount += 1;
            await _postRepository.UpdateAsync(postEntity);
        }

        public async Task On(CommentAddedEvent @event)
        {
            CommentEntity commentEntity = new()
            {
                PostId = @event.Id,
                Comment = @event.Comment,
                CommentId = @event.CommentId,
                CommentedDate = @event.CommentDate,
                Username = @event.Username,
                IsEdited = false
            };
            await _commentRepository.CreateAsync(commentEntity);
        }

        public async Task On(CommentUpdatedEvent @event)
        {
            CommentEntity commentEntity = await _commentRepository.GetByIdAsync(@event.CommentId);

            if (commentEntity == null) 
            {
                return;
            }

            commentEntity.Comment = @event.Comment;
            commentEntity.IsEdited = true;
            commentEntity.CommentedDate = @event.EditDate;
            await _commentRepository.UpdateAsync(commentEntity);
        }

        public async Task On(CommentRemovedEvent @event)
        {
            await _commentRepository.DeleteAsync(@event.CommentId);
        }

        public async Task On(PostRemovedEvent @event)
        {
            await _postRepository.DeleteAsync(@event.Id);
        }
    }
}