using System;
using System.Numerics;
using System.Reflection;
using LockStep.Define;
using TrueSync;

namespace LockStep
{
    [Serializable]
    public struct LockStepInput
    {
        public int moduleId;
        public int commandId;
        public TSVector2 vector2;

        public LockStepInput(int moduleId,int commandId)
        {
            this.moduleId = moduleId;
            this.commandId = commandId;
            vector2 = default(TSVector2);
        }
        public LockStepInput(LSCommandModuleDef moduleDef,int commandId)
        {
            this.moduleId = (int)moduleDef;
            this.commandId = commandId;
            vector2 = default(TSVector2);
        }
        public LockStepInput(LSCommandModuleDef moduleDef,int commandId,TSVector2 vector2)
        {
            this.moduleId = (int)moduleDef;
            this.commandId = commandId;
            this.vector2 = vector2;
        }
    }
}