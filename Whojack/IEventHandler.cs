using System.Threading.Tasks;

namespace WhoJack
{
    /// <summary>
    /// Defines a basic event handler.
    /// </summary>
    /// <typeparam name="T">Type of the event this handler can handle.</typeparam>
    public interface IEventHandler<in T>
    {
        /// <summary>
        /// Handles the given event by doing something useful.
        /// </summary>
        /// <param name="event">Event this handler is subscribed to.</param>
        Task Handle(T @event);
    }
}
