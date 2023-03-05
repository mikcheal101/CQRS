using CQRS.Core.Commands;

namespace CQRS.Core.Infra
{
    public interface ICommandDispatcher
    {
        void Registerhandler<T>(Func<T, Task> handler) where T: BaseCommand;
        Task SendAsync(BaseCommand command);
    }
}