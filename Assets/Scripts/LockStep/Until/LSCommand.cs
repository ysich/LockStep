using System;
using System.Reflection;
using LockStep.Define;

namespace LockStep
{
    [Serializable]
    public struct LSCommand
    {
        public int ModuleId;
        public int CommandId;

        public LSCommand(int moduleId,int commandId)
        {
            this.ModuleId = moduleId;
            this.CommandId = commandId;
        }
        public LSCommand(LSCommandModuleDef moduleDef,int commandId)
        {
            this.ModuleId = (int)moduleDef;
            this.CommandId = commandId;
        }
    }
}