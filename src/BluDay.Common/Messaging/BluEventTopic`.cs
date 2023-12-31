using BluDay.Common.Extensions;
using System.Collections.Generic;
using System;

namespace BluDay.Common.Messaging
{
    public sealed class BluEventTopic<TEvent> : IBluEventTopic where TEvent : Events.IBluEvent
    {
        private readonly Dictionary<object, BluEventHandler<TEvent>> _subscribersMap =
            new Dictionary<object, BluEventHandler<TEvent>>();

        private readonly HashSet<BluEventInfo<TEvent>> _eventInfos =
            new HashSet<BluEventInfo<TEvent>>();

        public bool Enabled { get; set; } = true;

        public int SubscribersCount => _subscribersMap.Count;

        public string Name { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public Type EventType { get; } = typeof(TEvent);

        public BluEventTopicInfo Info { get; }

        public IReadOnlyDictionary<object, BluEventHandler<TEvent>> SubscribersMap
        {
            get => new Dictionary<object, BluEventHandler<TEvent>>(_subscribersMap);
        }

        public BluEventInfo<TEvent>[] EventInfos => _eventInfos.BluToArray();

        public BluEventHandler<TEvent>[] Handlers => _subscribersMap.Values.BluToArray();

        public BluEventTopic(string name)
        {
            Name = name.BluOr(GetDefaultName());

            Info = new BluEventTopicInfo(this);
        }

        private Exception[] RaiseHandlers(object sender, TEvent e)
        {
            BluValidator.NotNull(
                (sender, nameof(sender)),
                (e,      nameof(e))
            );

            var exceptions = new List<Exception>();

            foreach (BluEventHandler<TEvent> handler in _subscribersMap.Values)
            {
                if (handler.Target == sender)
                {
                    continue;
                }

                try
                {
                    handler(e);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            return exceptions.ToArray();
        }

        public bool HasSubscriber(object target)
        {
            return _subscribersMap.ContainsKey(target);
        }

        public bool Subscribe(BluEventHandler<TEvent> handler)
        {
            return _subscribersMap.TryAdd(handler.Target, handler);
        }

        public bool Unsubscribe(object target)
        {
            return _subscribersMap.Remove(target);
        }

        public string GetDefaultName()
        {
            return EventType.Name.BluRemove(BluConstants.Event);
        }

        public string GetDefaultUniqueName()
        {
            return $"{Id}_{GetDefaultName()}";
        }

        public BluEventInfo<TEvent> Notify(object sender, TEvent e)
        {
            var info = new BluEventInfo<TEvent>(
                sender:     sender,
                e:          e,
                topic:      this,
                issuedAt:   DateTime.Now,
                exceptions: RaiseHandlers(sender, e)
            );

            return _eventInfos.BluAddAndReturn(info);
        }

        public override string ToString()
        {
            return "{0} (Enabled: {1}, Subscribers: {2}, Name: \"{3}\")".BluFormat(
                nameof(BluEventTopic<TEvent>),
                Enabled,
                SubscribersCount,
                Name
            );
        }
    }
}