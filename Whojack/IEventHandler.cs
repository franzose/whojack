using System.Threading;
using System.Threading.Tasks;

namespace WhoJack
{
    /// <summary>
    /// Defines an event handler for the given type of events.
    /// </summary>
    /// <typeparam name="T">Type of the event this handler can handle.</typeparam>
    public interface IEventHandler<in T>
    {
        /// <summary>
        /// Priority of this handler among other handlers of the same event.
        /// The higher priority is the earlier this handler will be invoked.
        /// </summary>
        int Priority => 0;

        /// <summary>
        /// Handles the given event by doing something useful.
        /// </summary>
        /// <param name="event">Event this handler is subscribed to.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        Task Handle(T @event, CancellationToken? cancellationToken = default);
    }
}
