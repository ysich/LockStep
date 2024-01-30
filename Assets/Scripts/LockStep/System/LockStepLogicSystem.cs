// /*---------------------------------------------------------------------------------------
// -- 负责人: onemt
// -- 创建时间: 2024-01-25 15:16:13
// -- 概述:帧同步逻辑层，可以拥有多个逻辑层，每个逻辑层之间不会互相影响。
// ---------------------------------------------------------------------------------------*/
//
// using Core;
//
// namespace LockStep
// {
//     public partial class LockStepSystem
//     {
//         public abstract void Run(int commandId,int frame);
//
//         public virtual void Tick(int frame)
//         {
//             EventBusSingleton.instance.Publish<int>(EventBusSingletonDefine.FrameTick,frame);
//         }
//     }
// }