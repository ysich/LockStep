/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-30 15:30:14
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Core;
using Demo;
using UnityEngine;

namespace LockStep
{
    public class LockStepInputOperationManager:Singleton<LockStepInputOperationManager>
    {
        private readonly Dictionary<Type, LockStepInputSystem> m_InputSystems = new Dictionary<Type, LockStepInputSystem>();
        
        public void Awake()
        {
            AddLSInputSystem<Demo_OneInputSystem>();
        }

        public void AddLSInputSystem<T>() where T : LockStepInputSystem,new()
        {
            T t = new T();
            Type type = typeof(T);
            if (m_InputSystems.ContainsKey(type))
            {
                Debug.LogError("已经存在相同的InputSystem");
                return;
            }
            m_InputSystems.Add(typeof(T),t);
        }
        
    }
}