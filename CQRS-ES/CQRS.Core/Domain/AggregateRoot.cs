using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _Id;
        private readonly List<BaseEvent> _changes = new();
        public Guid Id { get { return _Id; } }

        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommitedChanges()
        {
            return _changes;
        }

        public void MockChangesAsCommited()
        {
            _changes.Clear();
        }

        private void ApplyChanges(BaseEvent @event, bool isNewEvent)
        {
            // reflection
            var methodReflection = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });

            if (methodReflection == null)
            {
                throw new ArgumentNullException(nameof(methodReflection), $"The apply method was not found in the aggregate for {@event.GetType().Name}!");
            }

            methodReflection.Invoke(this, new Object[] { @event });

            if (isNewEvent)
            {
                _changes.Add(@event);
            }
        }

        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChanges(@event, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            events.ToList<BaseEvent>().ForEach((e) => ApplyChanges(e, false));
        }
    }
}