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
        public LockStepInput(LockStepCommandModuleDef moduleDef,int commandId)
        {
            this.moduleId = (int)moduleDef;
            this.commandId = commandId;
            vector2 = default(TSVector2);
        }
        public LockStepInput(LockStepCommandModuleDef moduleDef,int commandId,TSVector2 vector2)
        {
            this.moduleId = (int)moduleDef;
            this.commandId = commandId;
            this.vector2 = vector2;
        }

        public static bool operator ==(LockStepInput left, LockStepInput right)
        {
            if (left.moduleId != right.moduleId)
            {
                return false;
            }

            if (left.commandId != right.commandId)
            {
                return false;
            }

            if (left.vector2 != right.vector2)
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(LockStepInput left, LockStepInput right)
        {
            return !(left == right);
        }
        
    }
}