/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-06 15:00:40
-- 概述:
---------------------------------------------------------------------------------------*/

using Core;
using LockStep;

namespace Demo
{
    public class NetworkDemoSystem:LSLogicSystemBase
    {
        public override void Awake(LockStepSystem lockStepSystem)
        {
            base.Awake(lockStepSystem);
            inputOperationSystem = new NetworkDemoInputSystem();
        }

        public void StartTick(long startTime,int frame,int delay)
        {
            m_LockStepSystem.StartTick(startTime,frame,this);
            NetworkSyncSystemSingleton.instance.Connect(frame,delay);
        }
        
        public override void Run(OneFrameInputs oneFrameInputs)
        {
            
        }
    }
}