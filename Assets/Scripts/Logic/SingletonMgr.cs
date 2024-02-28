/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 15:42:26
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Core;
using LockStep;

namespace Demo
{
    public class SingletonMgr:IDisposable
    {
        private Stack<ASingleton> m_singletons = new Stack<ASingleton>();
        public void Awake()
        {
            AddSingleton<ObjectPool>();
            AddSingleton<TimeInfo>();
            AddSingleton<EventBusSingleton>();
            //Game
            AddSingleton<GameDatas>();
            AddSingleton<LockStepModuleSingletom>();

            AddSingleton<NetworkSyncSystemSingleton>();
        }
        public T AddSingleton<T>() where T:ASingleton,ISingletonAwake,new()
        {
            T singleton = new T();
            singleton.Awake();
            m_singletons.Push(singleton);
            singleton.Register();
            return singleton;
        }

        public void Dispose()
        {
            while (m_singletons.Count>0)
            {
                ASingleton singleton = m_singletons.Pop();
                singleton.Dispose();
            }
        }
    }
}