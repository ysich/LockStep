/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 16:48:14
-- 概述:
---------------------------------------------------------------------------------------*/

using Core;
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

        public override void Run(OneFrameInputs oneFrameInputs,int frame)
        {
            Demo_OneGameData data = GameDatas.instance.GetData<Demo_OneGameData>();
            LockStepInput input = oneFrameInputs.Input;
            Demo_OneCommandDef commandDef = (Demo_OneCommandDef)input.commandId;
            switch (commandDef)
            {
                case Demo_OneCommandDef.Skill_1:
                    Debug.Log($"Frame:{frame},Demo_One:Skill_1");
                    break;
                case Demo_OneCommandDef.Skill_2:
                    Debug.Log($"Frame:{frame},Demo_One:Skill_2");
                    break;
                case Demo_OneCommandDef.Skill_3:
                    Debug.Log($"Frame:{frame},Demo_One:Skill_3");
                    break;
                case Demo_OneCommandDef.Skill_4:
                    Debug.Log($"Frame:{frame},Demo_One:Skill_4");
                    break;
                case Demo_OneCommandDef.Move:
                    Debug.Log($"Frame:{frame},Demo_One:Move,v:{input.vector2}");
                    data.postion += input.vector2.ToVector();
                    EventBusSingleton.instance.Publish(EventBusSingletonDefine.Demo_One_Move,input.vector2);
                    break;
            }
        }
        
    }
}