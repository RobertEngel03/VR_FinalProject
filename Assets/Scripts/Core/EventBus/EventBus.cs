using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A simple singleton event bus for publishing and subscribing to events with optional filtering.
/// Allows multiple subscribers per event type and supports automatic unsubscription via IDisposable.
/// </summary>
public class EventBus
{
    /// <summary>
    /// Singleton instance of the EventBus.
    /// </summary>
    public static EventBus Instance => _instance ??= new EventBus();
    private static EventBus _instance;

    /// <summary>
    /// Internal dictionary storing lists of filtered listeners for each event type.
    /// </summary>
    private readonly Dictionary<Type, IList> _filteredListeners = new();

    private EventBus() { }

    /// <summary>
    /// Represents a subscription to an event of type T with an optional filter.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    private class FilteredSubscription<T>
    {
        public readonly Predicate<T> Filter;
        public readonly Action<T> Callback;

        // <summary>
        /// Initializes a new filtered subscription with the specified filter and callback.
        /// </summary>
        /// <param name="filter">Predicate to filter events. Pass null to receive all events.</param>
        /// <param name="callback">Action to invoke when an event occurs.</param>
        public FilteredSubscription(Predicate<T> filter, Action<T> callback)
        {
            Filter = filter;
            Callback = callback;
        }
    }

    /// <summary>
    /// Represents an unsubscriber handle that removes a subscription when disposed.
    /// </summary>
    /// <typeparam name="T">The type of the event the subscription listens to.</typeparam>
    private class Unsubscriber<T> : IDisposable
    {
        private readonly List<FilteredSubscription<T>> _subscriptions;
        private readonly FilteredSubscription<T> _subscription;

        /// <summary>
        /// Initializes a new unsubscriber for a specific subscription.
        /// </summary>
        /// <param name="subscriptions">The list of subscriptions to remove from.</param>
        /// <param name="subscription">The specific subscription to remove.</param>
        public Unsubscriber(List<FilteredSubscription<T>> subscriptions, FilteredSubscription<T> subscription)
        {
            _subscriptions = subscriptions;
            _subscription = subscription;
        }

        public void Dispose()
        {
            _subscriptions.Remove(_subscription);
        }
    }

    /// <summary>
    /// Subscribes to events of type T with an optional filter.
    /// </summary>
    /// <typeparam name="T">The type of event to listen for.</typeparam>
    /// <param name="filter">Predicate to filter events. Pass null to receive all events.</param>
    /// <param name="callback">Callback invoked when an event passes the filter.</param>
    /// <returns>An IDisposable that can be disposed to unsubscribe.</returns>
    public IDisposable Subscribe<T>(Predicate<T> filter, Action<T> callback)
    {
        var type = typeof(T);
        if (!_filteredListeners.TryGetValue(type, out var rawList))
        {
            rawList = new List<FilteredSubscription<T>>();
            _filteredListeners[type] = rawList;
        }

        var subscription = new FilteredSubscription<T>(filter, callback);
        ((List<FilteredSubscription<T>>)rawList).Add(subscription);
        return new Unsubscriber<T>((List<FilteredSubscription<T>>)rawList, subscription);
    }

    /// <summary>
    /// Subscribes to events of type T without a filter (receives all events).
    /// </summary>
    /// <typeparam name="T">The type of event to listen for.</typeparam>
    /// <param name="callback">Callback invoked when an event is published.</param>
    /// <returns>An IDisposable that can be disposed to unsubscribe.</returns>
    public IDisposable Subscribe<T>(Action<T> callback)
    {
        return Subscribe<T>(null, callback);
    }

    /// <summary>
    /// Publishes an event of type T to all subscribed listeners whose filter matches.
    /// </summary>
    /// <typeparam name="T">The type of the event to publish.</typeparam>
    /// <param name="evt">The event instance to publish.</param>
    public void Publish<T>(T evt)
    {
        var type = typeof(T);
        if (_filteredListeners.TryGetValue(type, out var rawList))
        {
            var listeners = (List<FilteredSubscription<T>>)rawList;
            for (int i = 0; i < listeners.Count; i++)
            {
                var sub = listeners[i];
                if (sub.Filter == null || sub.Filter(evt))
                {
                    sub.Callback.Invoke(evt);
                }
            }
        }
    }

    /// <summary>
    /// Clears all event listeners for all event types.
    /// Useful for scene unloads or resetting the event bus state.
    /// </summary>
    public void ClearAll()
    {
        _filteredListeners.Clear();
    }
}