/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-02 16:30:00
-- 概述:
---------------------------------------------------------------------------------------*/

namespace LockStep
{
    public abstract class LockStepLogicUpdateSystem:LockStepUpdateSystemBase
    {
        // /// <summary>
        // /// 收集一帧内的输入
        // /// </summary>
        // public LockStepInput input;
        public LockStepLogicUpdateSystem(LockStepSystem lockStepSystem) : base(lockStepSystem)
        {
        }
    }
}