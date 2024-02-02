/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-25 17:11:58
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EventBusSingleton:Singleton<EventBusSingleton>,ISingletonAwake
    {
        private Dictionary<EventBusSingletonDefine, HashSet<Delegate>> events;
        public void Awake()
        {
            events = new Dictionary<EventBusSingletonDefine, HashSet<Delegate>>();
        }

        public void RegisterEvent<TArgs>(EventBusSingletonDefine key, Action<TArgs> handler)
        {
            if (!events.TryGetValue(key, out var handlers))
            {
                handlers = new HashSet<Delegate>();
                events.Add(key, handlers);
            }

            handlers.Add(handler);
        }

        public void UnregisterEvent<TArgs>(EventBusSingletonDefine key, Action<TArgs> handler)
        {
            if (events.TryGetValue(key, out var handlers))
            {
                if (handlers.Remove(handler))
                {
                    if (handlers.Count == 0)
                    {
                        events.Remove(key);
                    }
                }
            }
        }

        public void Publish<TArgs>(EventBusSingletonDefine key, TArgs args)
        {
            HashSet<Action<TArgs>> handlers = null;
            
            if (events.TryGetValue(key, out var potentialHandlers))
            {
                foreach (var potentialHandler in potentialHandlers)
                {
                    if (potentialHandler is Action<TArgs> handler)
                    {
                        handler.Invoke(args);
                    }
                }
            }
        }
        
        public void Publish(EventBusSingletonDefine key)
        {
            Publish<object>(key, null);
        }

    }
}