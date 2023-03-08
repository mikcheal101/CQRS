

using CQRS.Core.Handlers;
using Post.Cmd.Domain;

namespace Post.Cmd.Api.Commands
{

    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

        public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NewPostCommand command)
        {
            PostAggregate postAggregate = new(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(postAggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            PostAggregate postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            postAggregate.EditMessage(command.Message);
            await _eventSourcingHandler.SaveAsync(postAggregate);
        }

        public async Task HandleAsync(LikeMessageCommand command)
        {
            PostAggregate postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            postAggregate.LikePost();
            await _eventSourcingHandler.SaveAsync(postAggregate);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            PostAggregate postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            postAggregate.AddComment(command.Comment, command.Username);
            await _eventSourcingHandler.SaveAsync(postAggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            PostAggregate postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            postAggregate.EditComment(command.CommentId, command.Comment, command.Username);
            await _eventSourcingHandler.SaveAsync(postAggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            PostAggregate postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            postAggregate.RemoveComment(command.CommentId, command.Username);
            await _eventSourcingHandler.SaveAsync(postAggregate);
        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            PostAggregate postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            postAggregate.DeletePost(command.Username);
            await _eventSourcingHandler.SaveAsync(postAggregate);
        }
    }
}