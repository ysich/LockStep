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
        
        public void RegisterEvent<TArgs,TArgs1>(EventBusSingletonDefine key, Action<TArgs,TArgs1> handler)
        {
            if (!events.TryGetValue(key, out var handlers))
            {
                handlers = new HashSet<Delegate>();
                events.Add(key, handlers);
            }

            handlers.Add(handler);
        }
        
        public void RegisterEvent<TArgs,TArgs1,TArgs2>(EventBusSingletonDefine key, Action<TArgs,TArgs1,TArgs2> handler)
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
        
        public void Publish<TArgs,TArgs1>(EventBusSingletonDefine key, TArgs args,TArgs1 args1)
        {
            HashSet<Action<TArgs,TArgs1>> handlers = null;
            
            if (events.TryGetValue(key, out var potentialHandlers))
            {
                foreach (var potentialHandler in potentialHandlers)
                {
                    if (potentialHandler is Action<TArgs,TArgs1> handler)
                    {
                        handler.Invoke(args,args1);
                    }
                }
            }
        }
        
        public void Publish<TArgs,TArgs1,TArgs2>(EventBusSingletonDefine key, TArgs args,TArgs1 args1,TArgs2 args2)
        {
            HashSet<Action<TArgs,TArgs1,TArgs2>> handlers = null;
            
            if (events.TryGetValue(key, out var potentialHandlers))
            {
                foreach (var potentialHandler in potentialHandlers)
                {
                    if (potentialHandler is Action<TArgs,TArgs1,TArgs2> handler)
                    {
                        handler.Invoke(args,args1,args2);
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