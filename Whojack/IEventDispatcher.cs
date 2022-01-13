namespace WhoJack
{
    /// <summary>
    /// Defines a dispatcher for events.
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Dispatches the given event to all registered listeners.
        /// </summary>
        /// <param name="event">The event to pass to the event listeners.</param>
        void Dispatch(object @event);
    }
}
