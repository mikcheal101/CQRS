using CQRS.Core.Commands;
using CQRS.Core.Infra;

namespace Post.Cmd.Infra
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();
        public void Registerhandler<T>(Func<T, Task> handler) where T: BaseCommand
        {
            if (_handlers.ContainsKey(typeof(T)))
            {
                throw new IndexOutOfRangeException("Duplicate handler!");
            }

            _handlers.Add(typeof(T), x => handler((T)x));
        }

        public async Task SendAsync(BaseCommand command)
        {
            if (_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task> handler))
            {
                await handler(command);
            }
            else
            {
                throw new ArgumentNullException(nameof(handler), "No command handler was registered!");
            }
        }
    }
}