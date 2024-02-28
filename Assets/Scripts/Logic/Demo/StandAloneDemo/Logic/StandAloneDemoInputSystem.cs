/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-05 15:44:31
-- 概述:
---------------------------------------------------------------------------------------*/

using LockStep.Define;
using TrueSync;
using UnityEngine;

namespace LockStep
{
    public class StandAloneDemoInputSystem:LSInputOperationSystemBase
    {
        protected override void OnUpdate()
        {
            LockStepCommandModuleDef moduleDef = LockStepCommandModuleDef.None;
            StandAloneCommandDef commandDef = StandAloneCommandDef.None;
            TSVector2 v = new();
            if (Input.GetKey(KeyCode.W))
            {
                v.y += 1;
                moduleDef = LockStepCommandModuleDef.StandAlone;
                commandDef = StandAloneCommandDef.Move;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                v.x -= 1;
                moduleDef = LockStepCommandModuleDef.StandAlone;
                commandDef = StandAloneCommandDef.Move;
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                v.y -= 1;
                moduleDef = LockStepCommandModuleDef.StandAlone;
                commandDef = StandAloneCommandDef.Move;
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                v.x += 1;
                moduleDef = LockStepCommandModuleDef.StandAlone;
                commandDef = StandAloneCommandDef.Move;
            }
            LockStepInput lockStepInput = new LockStepInput(moduleDef,(int)commandDef,v);
            this.input = lockStepInput;
        }
    }
}