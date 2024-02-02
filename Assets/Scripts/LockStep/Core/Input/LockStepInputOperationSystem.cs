/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-29 17:29:56
-- 概述:收集玩家输出的数据
---------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using LockStep.Define;
using TrueSync;
using UnityEngine;

namespace LockStep
{
    public abstract class LockStepInputOperationSystem:LockStepUpdateSystemBase
    {
        public Queue<LockStepInput> inputs = new Queue<LockStepInput>();

        public LockStepInputOperationSystem(LockStepSystem lockStepSystem) : base(lockStepSystem)
        {
        }
        
        public override void Update()
        {
            if (inputs.Count > 0)
            {
                LockStepInput input = inputs.Dequeue();
                m_system.logicUpdateSystem.input = input;
                return;
            }

            OnUpdate();
        }

        protected abstract void OnUpdate();
        // {
        //     LockStepCommandModuleDef moduleDef = LockStepCommandModuleDef.None;
        //     Demo_OneCommandDef commandDef = Demo_OneCommandDef.None;
        //     TSVector2 v = new();
        //     if (Input.GetKey(KeyCode.W))
        //     {
        //         v.y += 1;
        //         moduleDef = LockStepCommandModuleDef.Demo_One;
        //         commandDef = Demo_OneCommandDef.Move;
        //     }
        //
        //     if (Input.GetKey(KeyCode.A))
        //     {
        //         v.x -= 1;
        //         moduleDef = LockStepCommandModuleDef.Demo_One;
        //         commandDef = Demo_OneCommandDef.Move;
        //     }
        //
        //     if (Input.GetKey(KeyCode.S))
        //     {
        //         v.y -= 1;
        //         moduleDef = LockStepCommandModuleDef.Demo_One;
        //         commandDef = Demo_OneCommandDef.Move;
        //     }
        //
        //     if (Input.GetKey(KeyCode.D))
        //     {
        //         v.x += 1;
        //         moduleDef = LockStepCommandModuleDef.Demo_One;
        //         commandDef = Demo_OneCommandDef.Move;
        //     }
        //     LockStepInput lockStepInput = new LockStepInput(moduleDef,(int)commandDef,v);
        //     m_system.logicUpdateSystem.input = lockStepInput;
        // }
    }
}