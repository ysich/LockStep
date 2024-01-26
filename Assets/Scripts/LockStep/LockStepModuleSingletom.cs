﻿/*---------------------------------------------------------------------------------------
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
        private Dictionary<int, LockStepSystem> m_SystemManager = new Dictionary<int, LockStepSystem>();
        // private Queue<LockStepSystem> m_LockStepSystems = new Queue<LockStepSystem>();

        private HashSet<LockStepSystem> m_UpdateSystems = new HashSet<LockStepSystem>();
        private HashSet<LockStepSystem> m_NewUpdateSystems = new HashSet<LockStepSystem>();
        public void Awake()
        {
            AddCommandModule<Demo_OneModuleSystem>(LSCommandModuleDef.Demo_One);
        }
        public void Update()
        {
            if (m_NewUpdateSystems.Count>0)
            {
                foreach (var newUpdateSystem in m_NewUpdateSystems)
                {
                    m_UpdateSystems.Add(newUpdateSystem);
                }
                m_NewUpdateSystems.Clear();
            }

            if (m_UpdateSystems.Count > 0)
            {
                try
                {
                    foreach (var updateSystem in m_UpdateSystems)
                    {
                        updateSystem.Update();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                
            }
        }

        public void RegisterUpdate(LockStepSystem lockStepSystem)
        {
            if (m_NewUpdateSystems.Contains(lockStepSystem))
            {
                Debug.LogError("LockStepSystem在同一帧内被重复注册Update！！！！");
                return;
            }

            if (m_UpdateSystems.Contains(lockStepSystem))
            {
                Debug.Log("LockStepSystem已存在Update！！重复添加无效");
                return;
            }
            Debug.Log("System主程Update");
            m_NewUpdateSystems.Add(lockStepSystem);
        }

        public void UnRegisterUpdate(LockStepSystem lockStepSystem)
        {
            bool isRemoveNew = m_NewUpdateSystems.Remove(lockStepSystem);
            if (!isRemoveNew)
            {
                m_UpdateSystems.Remove(lockStepSystem);
            }
            else
            {
                Debug.Log("系统还未执行到Tick就被卸载！！");
            }
        }

        public T AddCommandModule<T>(LSCommandModuleDef lsCommandModuleDef) where T:LockStepSystem,new()
        {
            return AddCommandModule<T>((int)lsCommandModuleDef);
        }
        public T AddCommandModule<T>(int commandModuleId) where T:LockStepSystem,new()
        {
            T system = new T();
            m_SystemManager.Add(commandModuleId,system);
            return system;
        }

        public bool TryGetCommandModule(LSCommandModuleDef moduleDef,out LockStepSystem lsUpdateSystem)
        {
            return TryGetCommandModule((int)moduleDef,out lsUpdateSystem);
        }
        public bool TryGetCommandModule(int moduleId,out LockStepSystem lsUpdateSystem)
        {
            if (!m_SystemManager.TryGetValue(moduleId, out lsUpdateSystem))
            {
                Debug.LogError($"LockStepCommandSingleton:no found LSLogicSystemModuleID:{moduleId}");
                return false;
            }
            return true;
        }
        public void UpdateCommand(LSCommand command,int frame)
        {
            if(!TryGetCommandModule(command.ModuleId, out LockStepSystem lsUpdateSystem))
            {
                Debug.LogError($"LockStepCommandSingleton:UpdateCommand failed!");
                return;
            }
            lsUpdateSystem.Run(command.CommandId,frame);
        }

        // public void AddFrameInput(LSCommand command)
        // {
        //     if(!TryGetCommandModule(command.ModuleId, out LockStepSystem lsUpdateSystem))
        //     {
        //         Debug.LogError($"LockStepCommandSingleton:AddFrameInput failed!");
        //         return;
        //     }
        //     lsUpdateSystem.AddFrameInput(command);
        // }
    }
}