/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-02 16:17:51
-- 概述:
---------------------------------------------------------------------------------------*/

namespace LockStep
{
    public class StandAloneLSSystem:LockStepSystem
    {
        public StandAloneLSSystem():base()
        {
            logicUpdateSystem = new StandAloneLSLogicUpdateSystem(this);
        }

        protected override void InitModule()
        {
            base.InitModule();
            AddModule<DemoOneModuleSystem>();
        }
    }
}