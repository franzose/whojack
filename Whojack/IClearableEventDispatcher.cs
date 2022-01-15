namespace WhoJack
{
    /// <summary>
    /// Defines an event dispatcher which could clean up event handlers.
    /// </summary>
    /// <remarks>
    /// When injected as a dependency, the actual object should be the same
    /// <see cref="IEventDispatcher"/> used elsewhere to dispatch events
    /// so that the event handlers would be removed from the same object.
    /// </remarks>
    public interface IClearableEventDispatcher
    {
        /// <summary>
        /// Removes all handlers of the given event type.
        /// </summary>
        /// <typeparam name="T">Event type.</typeparam>
        void RemoveHandlers<T>() where T : class;
        
        /// <summary>
        /// Removes all event handlers.
        /// </summary>
        void Clear();
    }
}
