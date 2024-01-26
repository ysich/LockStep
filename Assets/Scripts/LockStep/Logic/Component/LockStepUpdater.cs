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
        private List<LSCommand> m_UpdateCommands = new List<LSCommand>();
        // private List<LSCommand> m_NewUpdateCommands = new List<LSCommand>();
        
        public void LSUpdate(int frame)
        {
            // if (m_NewUpdateCommands.Count > 0)
            // {
            //     foreach (var newUpdateCommand in m_NewUpdateCommands)
            //     {
            //         m_UpdateCommands.Add(newUpdateCommand);
            //     }
            //     m_NewUpdateCommands.Clear();
            // }

            if (m_UpdateCommands.Count > 0)
            {
                foreach (var updateCommand in m_UpdateCommands)
                {
                    LockStepModuleSingletom.instance.UpdateCommand(updateCommand,frame);
                }
                m_UpdateCommands.Clear();
            }
            // for (int i = 0; i < updateCommands.Count; i++)
            // {
            //     LSCommand command = updateCommands[i];
            //     LockStepModuleSingletom.instance.UpdateCommand(command);
            // }
        }

        public void Add(OneFrameInputs oneFrameInputs)
        {
            foreach (var lsCommand in oneFrameInputs.commandQueue)
            {
                m_UpdateCommands.Add(lsCommand);    
            }
        }
    }
}