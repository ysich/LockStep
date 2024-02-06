/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-29 10:57:39
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using Core;
using LockStep.Define;

namespace LockStep
{
    public class LockStepReplayUpdateSystem:LockStepUpdateSystemBase
    {
        public float ReplaySpeed { get; set; } = 1;

        public LockStepReplayUpdateSystem(LockStepSystem lockStepSystem) : base(lockStepSystem)
        {
        }

        public override void Update()
        {
            while (true)
            {
                //重播逻辑下都是权威帧
                if (m_system.authorityFrame + 1 >= m_system.kLockStepReplay.FrameInputs.Count)
                {
                    break;
                }
                
                if (TimeInfo.instance.ClientNow() < m_system.fixedFrameTimeCounter.FrameTime(m_system.authorityFrame + 1))
                {
                    break;
                }

                ++m_system.authorityFrame;
                
                OneFrameInputs oneFrameInputs = m_system.kLockStepReplay.FrameInputs[m_system.authorityFrame];
                m_system.LSUpdate(oneFrameInputs);
            }
        }

        public void ChangeReplaySpeed(int replaySpeed)
        {
            if (replaySpeed >= 8)
            {
                ReplaySpeed = 8;
            }
            else
            {
                ReplaySpeed = replaySpeed;
            }
            int updateInterval = (int)(LockStepConstValue.k_UpdateInterval / ReplaySpeed);
            m_system.fixedFrameTimeCounter.ChangeInterval(updateInterval,m_system.authorityFrame);
        }
    }
}