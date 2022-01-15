using System;
using System.Threading;
using System.Threading.Tasks;

namespace WhoJack
{
    /// <summary>
    /// Defines a configurable event dispatcher.
    /// </summary>
    /// <remarks>
    /// When injected as a dependency, the actual object should be the same
    /// <see cref="IEventDispatcher"/> used elsewhere to dispatch events
    /// so that the event handlers would be added to the same object.
    /// </remarks>
    public interface IConfigurableEventDispatcher : IEventDispatcher
    {
        /// <summary>
        /// Adds a synchronous event handler to the event dispatcher.
        /// </summary>
        /// <param name="action">Action to perform on an occurrence of the given type of events.</param>
        /// <param name="priority">Priority of this action. The higher priority is the earlier this action will be invoked.</param>
        /// <typeparam name="T">Event type.</typeparam>
        void AddHandler<T>(Action<T> action, int priority = 0) where T : class;
        
        /// <summary>
        /// Adds a new action to the event dispatcher.
        /// </summary>
        /// <param name="action">Action which may be asynchronous.</param>
        /// <param name="priority">Priority of this handler. The higher priority is the earlier this action will be invoked.</param>
        /// <typeparam name="T">Event type.</typeparam>
        void AddHandler<T>(Func<T, CancellationToken?, Task> action, int priority = 0) where T : class;
        
        /// <summary>
        /// Adds another event handler to the event dispatcher.
        /// </summary>
        /// <param name="handler">The event handler.</param>
        /// <typeparam name="T">Event type.</typeparam>
        void AddHandler<T>(IEventHandler<T> handler) where T : class;
    }
}
