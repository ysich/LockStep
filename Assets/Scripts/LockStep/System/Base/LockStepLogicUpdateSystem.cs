/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-29 15:16:57
-- 概述:
---------------------------------------------------------------------------------------*/

using Core;

namespace LockStep
{
    public class LockStepLogicUpdateSystem:LockStepUpdateSystemBase
    {
        /// <summary>
        /// 收集一帧内的输入
        /// </summary>
        public LockStepInput m_LockStepInput;
        public LockStepLogicUpdateSystem(LockStepSystem lockStepSystem) : base(lockStepSystem)
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
                
                ++m_system.authorityFrame;
                OneFrameInputs oneFrameInputs = m_system.kFrameBuffer.GetFrameInputs(m_system.authorityFrame);
                m_system.LSUpdate(oneFrameInputs);
            }
        }
    }
}