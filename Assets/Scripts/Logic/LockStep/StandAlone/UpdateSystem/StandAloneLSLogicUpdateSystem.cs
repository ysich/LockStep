/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-02 16:18:37
-- 概述: 单机模式，只有权威帧没有预测帧
---------------------------------------------------------------------------------------*/

using Core;

namespace LockStep
{
    public class StandAloneLSLogicUpdateSystem:LockStepLogicUpdateSystem
    {
        public StandAloneLSLogicUpdateSystem(LockStepSystem lockStepSystem) : base(lockStepSystem)
        {
        }
        
        public override void Update()
        {
            while (true)
            {
                //没到下一帧时间则不执行
                if (TimeInfo.instance.ClientNow() < m_system.fixedFrameTimeCounter.FrameTime(m_system.authorityFrame + 1))
                {
                    break;
                }

                //逻辑执行到之前先收集操作指令
                m_system.runningLogicSystem.Update();
                
                ++m_system.authorityFrame;
                m_system.kFrameBuffer.MoveForward(m_system.authorityFrame);
                OneFrameInputs oneFrameInputs = m_system.kFrameBuffer.GetFrameInputs(m_system.authorityFrame);
                oneFrameInputs.Input = m_system.runningLogicSystem.inputOperationSystem.input;
                m_system.LSUpdate(oneFrameInputs);
            }
        }
    }
}