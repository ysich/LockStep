/*---------------------------------------------------------------------------------------
-- 负责人: YeSiCheng
-- 创建时间: 2023-07-04 14:31:39
-- 概述: 对象池模块
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPool: Singleton<ObjectPool>,ISingletonAwake
    {
        private Dictionary<Type, Queue<object>> m_Pool;
        private Dictionary<Type, int> m_PoolInstanceCountMap;

        public void Awake()
        {
            if(m_Pool == null)
                m_Pool = new Dictionary<Type, Queue<object>>();
            if (m_PoolInstanceCountMap == null)
                m_PoolInstanceCountMap = new Dictionary<Type, int>();
        }
        
        public T Fetch<T>() where T: class
        {
            return this.Fetch(typeof (T)) as T;
        }

        public object Fetch(Type type)
        {
            Queue<object> queue = null;
            if (!m_Pool.TryGetValue(type, out queue))
            {
                //因为回收才有队列所以这里需要判断下
                if (m_PoolInstanceCountMap.TryGetValue(type,out int instanceCount))
                    m_PoolInstanceCountMap[type] = ++instanceCount;
                else
                    m_PoolInstanceCountMap.Add(type,1);
                
                return Activator.CreateInstance(type);
            }

            if (queue.Count == 0)
            {
                int instanceCount = m_PoolInstanceCountMap[type];
                m_PoolInstanceCountMap[type] = ++instanceCount;
                
                return Activator.CreateInstance(type);
            }
            return queue.Dequeue();
        }

        public void Recycle(object obj)
        {
            Type type = obj.GetType();
            Queue<object> queue = null;
            if (!m_Pool.TryGetValue(type, out queue))
            {
                queue = new Queue<object>();
                m_Pool.Add(type, queue);
            }
            // 一种对象最大为1000个，避免一直回收导致内存泄露
            if (queue.Count > 1000)
            {
                //超出的不算内存泄漏
                int instanceCount = m_PoolInstanceCountMap[type];
                m_PoolInstanceCountMap[type] =  --instanceCount;
                return;
            }
            queue.Enqueue(obj);
        }

        public void Dispose()
        {
            foreach (var instanceKv in m_PoolInstanceCountMap)
            {
                int instanceCount = instanceKv.Value;
                if (instanceCount > 0)
                {
                    Type type = instanceKv.Key;
                    Queue<object> queue = m_Pool[type];
                    int notRecycleCount = instanceCount - queue.Count;
                    Debug.Log($"ObjectPool,Dispose:type:{type.Name}，NotRecycleCount：{notRecycleCount}");
                }
            }
        }
    }
}