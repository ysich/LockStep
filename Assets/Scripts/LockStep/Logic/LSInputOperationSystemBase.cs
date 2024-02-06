/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-04 14:53:53
-- 概述:
---------------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace LockStep
{
    public abstract class LSInputOperationSystemBase
    {
        public Queue<LockStepInput> waitInputs = new Queue<LockStepInput>();

        public LockStepInput input;
        
        public void Update()
        {
            if (waitInputs.Count > 0)
            {
                LockStepInput input = waitInputs.Dequeue();
                // m_system.logicUpdateSystem.input = input;
                this.input = input;
                return;
            }

            OnUpdate();
        }
        
        protected abstract void OnUpdate();
    }
}