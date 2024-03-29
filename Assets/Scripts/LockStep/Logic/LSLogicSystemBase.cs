﻿/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-04 14:52:37
-- 概述: 帧同步的业务逻辑基类，主要负责输入数据的收集和运行
---------------------------------------------------------------------------------------*/

using System;
using Core;
using Demo;

namespace LockStep
{
    public abstract class LSLogicSystemBase
    {
        protected LockStepSystem m_LockStepSystem;
        public LSInputOperationSystemBase inputOperationSystem;
        
        public virtual void Awake(LockStepSystem lockStepSystem)
        {
            m_LockStepSystem = lockStepSystem;
        }

        //TODO:这里这么桥接有点奇怪，对父级依赖比较深，考虑后续改为注册形式
        public void StartTick(long startTime,int frame)
        {
            m_LockStepSystem.StartTick(startTime,frame,this);
        }

        //TODO:这里这么桥接有点奇怪，对父级依赖比较深，考虑后续改为注册形式
        public void StopTick()
        {
            m_LockStepSystem.StopTick();
        }
        //TODO:这里这么桥接有点奇怪，对父级依赖比较深，考虑后续改为注册形式
        public void Replay()
        {
            m_LockStepSystem.Replay(this);
        }
        
        public void AddFrameInput(LockStepInput lsStepInput)
        {
            inputOperationSystem.waitInputs.Enqueue(lsStepInput);
        }

        public void UpdateInputOperation()
        {
            inputOperationSystem.Update();
        }

        public abstract void Run(OneFrameInputs oneFrameInputs);
    }
}