/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 16:57:58
-- 概述: 帧同步-命令绑定单例
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Core;
using Demo;
using LockStep.Define;
using UnityEngine;

namespace LockStep
{
    public class LockStepModuleSingletom:Singleton<LockStepModuleSingletom>,ISingletonAwake,ISingletonUpdate
    {
        private Dictionary<Type, LockStepSystem> m_SystemsMap = new Dictionary<Type, LockStepSystem>();
        private Queue<LockStepSystem> m_Systems = new Queue<LockStepSystem>();
        public void Awake()
        {
            AddModule<StandAloneLSSystem>();
            AddModule<NetworkLSSystem>();
        }

        public T AddModule<T>() where T:LockStepSystem,new()
        {
            T system = new T();
            system.Awake();
            m_SystemsMap.Add(typeof(T),system);
            m_Systems.Enqueue(system);
            return system;
        }

        public void Update()
        {
            int count = m_Systems.Count;
            while (count-- > 0 )
            {
                LockStepSystem system = m_Systems.Dequeue();
                m_Systems.Enqueue(system);
                try
                {
                    system.Update();
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }

            }
        }

        public T GetModule<T>()where T:LockStepSystem
        {
            Type type = typeof(T);
            if (!m_SystemsMap.ContainsKey(type))
            {
                Debug.LogError($"$cannot found LockStepSystem:{type.Name}");
            }
            return m_SystemsMap[type] as T;
        }
        
        // public bool TryGetCommandModule(LockStepCommandModuleDef moduleDef,out LockStepSystem lsUpdateSystem)
        // {
        //     return TryGetCommandModule((int)moduleDef,out lsUpdateSystem);
        // }
        // public bool TryGetCommandModule(int moduleId,out LockStepSystem lsUpdateSystem)
        // {
        //     if (!m_SystemManager.TryGetValue(moduleId, out lsUpdateSystem))
        //     {
        //         Debug.LogError($"LockStepCommandSingleton:no found LSLogicSystemModuleID:{moduleId}");
        //         return false;
        //     }
        //     return true;
        // }
        // public void UpdateCommand(OneFrameInputs oneFrameInputs,int frame)
        // {
        //     LockStepInput input = oneFrameInputs.Input;
        //     if (input == LockStepConstValue.kNoneInputs)
        //     {
        //         return;
        //     }
        //     if(!TryGetCommandModule(input.moduleId, out LockStepSystem lsUpdateSystem))
        //     {
        //         Debug.LogError($"LockStepCommandSingleton:UpdateCommand failed!");
        //         return;
        //     }
        //     // lsUpdateSystem.Run(oneFrameInputs,frame);
        // }
        
        
        //
        // public void RegisterUpdate(LockStepSystem lockStepSystem)
        // {
        //     // if (m_NewUpdateSystems.Contains(lockStepSystem))
        //     // {
        //     //     Debug.LogError("LockStepSystem在同一帧内被重复注册Update！！！！");
        //     //     return;
        //     // }
        //     //
        //     // if (m_UpdateSystems.Contains(lockStepSystem))
        //     // {
        //     //     Debug.Log("LockStepSystem已存在Update！！重复添加无效");
        //     //     return;
        //     // }
        //     // Debug.Log("System主程Update");
        //     // m_NewUpdateSystems.Add(lockStepSystem);
        // }
        //
        // public void UnRegisterUpdate(LockStepSystem lockStepSystem)
        // {
        //     // bool isRemoveNew = m_NewUpdateSystems.Remove(lockStepSystem);
        //     // if (!isRemoveNew)
        //     // {
        //     //     m_UpdateSystems.Remove(lockStepSystem);
        //     // }
        //     // else
        //     // {
        //     //     Debug.Log("系统还未执行到Tick就被卸载！！");
        //     // }
        // }
    }
}