/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-04 17:18:47
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using LockStep.Define;
using Core;
using UnityEngine;

namespace LockStep
{
    public partial class LockStepSystem: ILockStepAwake
    {
        public Dictionary<Type, LSLogicSystemBase> m_modules = new Dictionary<Type, LSLogicSystemBase>();

        protected virtual void InitModule()
        {
            
        }

        protected void AddModule<T>()where T:LSLogicSystemBase,new()
        {
            T t = new T();
            t.Awake(this);
            Type type = typeof(T);
            m_modules.Add(type,t);
        }
        
        public T GetModule<T>()where T:LSLogicSystemBase
        {
            Type type = typeof(T);
            if (!m_modules.ContainsKey(type))
            {
                Debug.LogError($"cannot found LockStepSystem:{type.Name}");
                return null;
            }
            return m_modules[type] as T;
        }
    }
}