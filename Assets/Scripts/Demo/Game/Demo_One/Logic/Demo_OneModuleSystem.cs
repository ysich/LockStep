/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 16:48:14
-- 概述:
---------------------------------------------------------------------------------------*/
using LockStep;
using LockStep.Define;
using UnityEngine;

namespace Demo
{
    public class Demo_OneModuleSystem:LockStepSystem
    {
        public Demo_OneModuleSystem()
        {
            
        }

        public override void Run(int commandId,int frame)
        {
            Demo_OneCommandDef commandDef = (Demo_OneCommandDef)commandId;
            switch (commandDef)
            {
                case Demo_OneCommandDef.Left:
                    Debug.Log($"Frame:{frame},Demo_One:Left");
                    break;
                case Demo_OneCommandDef.Right:
                    Debug.Log($"Frame:{frame},Demo_One:Right");
                    break;
                case Demo_OneCommandDef.Down:
                    Debug.Log($"Frame:{frame},Demo_One:Down");
                    break;
                case Demo_OneCommandDef.Up:
                    Debug.Log($"Frame:{frame},Demo_One:Up");
                    break;
            }
        }
        
    }
}