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
        /// Higher priority means earlier invoke of the handler.
        /// </summary>
        int Priority => 0;
        
        /// <summary>
        /// Handles the given event by doing something useful.
        /// </summary>
        /// <param name="event">Event this handler is subscribed to.</param>
        Task Handle(T @event);
    }
}
