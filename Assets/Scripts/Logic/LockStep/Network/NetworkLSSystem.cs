/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-02 16:16:01
-- 概述:
---------------------------------------------------------------------------------------*/

using Core;
using Demo;
using LockStep.Define;

namespace LockStep
{
    public class NetworkLSSystem:LockStepSystem
    {
        public NetworkLSSystem():base()
        {
            logicUpdateSystem = new NetworkLSLogicUpdateSystem(this);
            
        }

        protected override void InitModule()
        {
            base.InitModule();
            AddModule<NetworkDemoSystem>();
            EventBusSingleton.instance.RegisterEvent<int,OneFrameInputs,int>(EventBusSingletonDefine.Network_Sync,SyncHandler);
        }
        
        public void SyncHandler(int frame,OneFrameInputs syncOneFrameInputs,int delay)
        {
            ChangeInterval(delay);
            
            OneFrameInputs oneFrameInputs = this.kFrameBuffer.GetFrameInputs(frame);
            syncOneFrameInputs.CopyTo(oneFrameInputs);
            this.authorityFrame++;
        }

        private void ChangeInterval(int delay)
        {
            int newInterval = (1000 + (delay - LockStepConstValue.k_UpdateInterval)) * LockStepConstValue.k_UpdateInterval / 1000;

            if (newInterval < 40)
            {
                newInterval = 40;
            }

            if (newInterval > 66)
            {
                newInterval = 66;
            }
            
            this.fixedFrameTimeCounter.ChangeInterval(newInterval, this.predictionFrame);
        }
    }
}