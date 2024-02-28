/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-29 15:16:57
-- 概述: 网络模式，需要有预测帧，和权威帧比对不同则需要进行回滚。
---------------------------------------------------------------------------------------*/

using Core;
using Demo;
using UnityEngine;

namespace LockStep
{
    public class NetworkLSLogicUpdateSystem:LockStepLogicUpdateSystem
    {
        
        public NetworkLSLogicUpdateSystem(LockStepSystem lockStepSystem) : base(lockStepSystem)
        {
        }
        public override void Update()
        {
            while (true)
            {
                //没到下一帧时间则不执行
                if (TimeInfo.instance.ClientNow() < m_system.fixedFrameTimeCounter.FrameTime(m_system.predictionFrame + 1))
                {
                    break;
                }
                
                // 最多只预测5帧
                if (m_system.predictionFrame - m_system.authorityFrame > 5)
                {
                    return;
                }
                //逻辑执行到之前先收集操作指令
                m_system.runningLogicSystem.UpdateInputOperation();
                
                ++m_system.predictionFrame;
                OneFrameInputs oneFrameInputs = GetOneFrameInputs(m_system.predictionFrame);
                m_system.LSUpdate(oneFrameInputs);
                
                NetworkSyncSystemSingleton.instance.Receive(m_system.predictionFrame,oneFrameInputs);
            }
        }

        public OneFrameInputs GetOneFrameInputs(int frame)
        {
            //预测帧小于等于权威帧那么直接取数据
            if (frame <= m_system.authorityFrame)
            {
                OneFrameInputs oneFrameInputs = m_system.kFrameBuffer.GetFrameInputs(frame);
                return oneFrameInputs;
            }
            //predictionFrame
            m_system.kFrameBuffer.MoveForward(m_system.predictionFrame);
            OneFrameInputs predictionFrameInputs = m_system.kFrameBuffer.GetFrameInputs(frame);
            predictionFrameInputs.Input = m_system.runningLogicSystem.inputOperationSystem.input;
            return predictionFrameInputs;
        }
    }
}