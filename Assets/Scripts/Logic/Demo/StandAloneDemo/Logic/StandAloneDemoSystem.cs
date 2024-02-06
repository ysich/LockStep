﻿/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 16:48:14
-- 概述:
---------------------------------------------------------------------------------------*/

using Core;
using Demo;
using LockStep;
using LockStep.Define;
using UnityEngine;

namespace LockStep
{
    public class StandAloneDemoSystem:LSLogicSystemBase
    {
        public override void Awake(LockStepSystem lockStepSystem)
        {
            base.Awake(lockStepSystem);
            inputOperationSystem = new StandAloneDemoInputSystem();
        }

        public override void Run(OneFrameInputs oneFrameInputs)
        {
            StandAloneDemoData data = GameDatas.instance.GetData<StandAloneDemoData>();
            LockStepInput input = oneFrameInputs.Input;
            Demo_OneCommandDef commandDef = (Demo_OneCommandDef)input.commandId;
            switch (commandDef)
            {
                case Demo_OneCommandDef.Skill_1:
                    // Debug.Log($"Frame:{frame},Demo_One:Skill_1");
                    Debug.Log($"Frame:Demo_One:Skill_1");
                    break;
                case Demo_OneCommandDef.Skill_2:
                    // Debug.Log($"Frame:{frame},Demo_One:Skill_2");
                    Debug.Log($"Frame:Demo_One:Skill_2");
                    break;
                case Demo_OneCommandDef.Skill_3:
                    // Debug.Log($"Frame:{frame},Demo_One:Skill_3");
                    Debug.Log($"Frame:Demo_One:Skill_3");
                    break;
                case Demo_OneCommandDef.Skill_4:
                    // Debug.Log($"Frame:{frame},Demo_One:Skill_4");
                    Debug.Log($"Frame:Demo_One:Skill_4");
                    break;
                case Demo_OneCommandDef.Move:
                    // Debug.Log($"Frame:{frame},Demo_One:Move,v:{input.vector2}");
                    Debug.Log($"Frame:Demo_One:Move,v:{input.vector2}");
                    data.postion += input.vector2.ToVector();
                    EventBusSingleton.instance.Publish(EventBusSingletonDefine.Demo_One_Move,input.vector2);
                    break;
            }
        }
    }
}