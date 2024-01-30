/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-17 15:12:09
-- 概述: 帧同步-执行帧逻辑的地方
---------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.ComponentModel.Design;

namespace LockStep
{
    public class LockStepUpdater
    {
        private List<LockStepInput> m_UpdateCommands = new List<LockStepInput>();
        public void LSUpdate(int frame)
        {
            if (m_UpdateCommands.Count > 0)
            {
                foreach (var updateCommand in m_UpdateCommands)
                {
                    LockStepModuleSingletom.instance.UpdateCommand(updateCommand,frame);
                }
                m_UpdateCommands.Clear();
            }
        }

        public void Add(OneFrameInputs oneFrameInputs)
        {
            foreach (var lsCommand in oneFrameInputs.inputQueue)
            {
                m_UpdateCommands.Add(lsCommand);    
            }
        }
    }
}