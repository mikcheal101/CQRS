using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain
{
    public class PostAggregate: AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {}

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id, Author = author, Message = message, DatePosted = DateTime.Now
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _Id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if(!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post!");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The valud of {nameof(message)} cannot be null or empty. Please Provide a valid {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _Id,
                Message = message,
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _Id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post!");
            }

            RaiseEvent(new PostLikedEvent
            {
                Id = _Id,
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _Id = @event.Id;
        }
    
        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add a comment of an inactive post!");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The valud of {nameof(comment)} cannot be null or empty. Please Provide a valid {nameof(comment)}");
            }

            RaiseEvent(new CommentAddedEvent
            {
               Id = _Id, Comment = comment, Username = username, CommentDate = DateTime.Now, CommentId = Guid.NewGuid()
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _Id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit a comment of an inactive post!");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment created by another user!");
            }
            
            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _Id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.Now,
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _Id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment of an inactive post!");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment created by another user!");
            }

            RaiseEvent(new CommentRemovedEvent
            {
                Id = _Id,
                CommentId = commentId
            });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _Id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been removed!");
            }

            if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a Post created by another user!");
            }

            RaiseEvent(new PostRemovedEvent
            {
                Id = _Id,
            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _Id = @event.Id;
            _active = false;
        }
    }
}