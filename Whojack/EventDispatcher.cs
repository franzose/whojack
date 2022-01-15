using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WhoJack
{
    public sealed class EventDispatcher : IConfigurableEventDispatcher, IClearableEventDispatcher
    {
        private sealed record EventHandler(int Priority, Func<object, CancellationToken?, Task> Fn);
        private static readonly object Lock = new();
        private readonly ConcurrentDictionary<Type, List<EventHandler>> _listeners = new();

        public void AddHandler<T>(Action<T> action, int priority = 0) where T : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _listeners.AddOrUpdate(
                typeof(T),
                _ =>
                {
                    lock (Lock)
                    {
                        return new List<EventHandler>
                        {
                            new(priority, (@event, _) =>
                            {
                                action(@event as T);
                                return Task.CompletedTask;
                            })
                        };
                    }
                },
                (_, list) =>
                {
                    lock (Lock)
                    {
                        list.Add(new(priority, (@event, _) =>
                        {
                            action(@event as T);
                            return Task.CompletedTask;
                        }));
                        list.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                        return list;
                    }
                }
            );
        }

        public void AddHandler<T>(Func<T, CancellationToken?, Task> action, int priority = 0) where T : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _listeners.AddOrUpdate(
                typeof(T),
                _ =>
                {
                    lock (Lock)
                    {
                        return new List<EventHandler>
                        {
                            new(priority, async (@event, cancellationToken) => await action(@event as T, cancellationToken))
                        };
                    }
                },
                (_, list) =>
                {
                    lock (Lock)
                    {
                        list.Add(new(priority, async (@event, cancellationToken) => await action(@event as T, cancellationToken)));
                        list.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                        return list;
                    }
                }
            );
        }

        public void AddHandler<T>(IEventHandler<T> handler) where T : class
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            
            AddHandler<T>(async (@event, cancellationToken) => await handler.Handle(@event, cancellationToken), handler.Priority);
        }
        
        public async Task Dispatch(object @event, CancellationToken cancellationToken = default)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            
            if (!_listeners.TryGetValue(@event.GetType(), out var handlers))
            {
                return;
            }
            
            foreach (var handler in handlers)
            {
                await handler.Fn.Invoke(@event, cancellationToken);
            }
        }

        public void RemoveHandlers<T>() where T : class
        {
            if (!_listeners.TryGetValue(typeof(T), out var list))
            {
                return;
            }
            
            lock (Lock)
            {
                list.Clear();
            }
        }

        public void Clear() => _listeners.Clear();
    }
}
