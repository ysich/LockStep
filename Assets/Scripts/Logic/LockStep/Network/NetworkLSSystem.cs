/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-02 16:16:01
-- 概述:
---------------------------------------------------------------------------------------*/

namespace LockStep
{
    public class NetworkLSSystem:LockStepSystem
    {
        public NetworkLSSystem():base()
        {
            logicUpdateSystem = new NetworkLSLogicUpdateSystem(this);
        }
    }
}