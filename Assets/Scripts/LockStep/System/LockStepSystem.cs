/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-17 15:20:16
-- 概述: 帧同步逻辑层，可以拥有多个逻辑层，每个逻辑层之间不会互相影响。
---------------------------------------------------------------------------------------*/

using Core;
using LockStep.Define;
using MemoryPack;
using Unity.VisualScripting;
using UnityEngine;

namespace LockStep
{
    public abstract partial class LockStepSystem
    {
        private readonly LockStepUpdater k_lockStepUpdater = new LockStepUpdater();
        
        public readonly FixedFrameTimeCounter fixedFrameTimeCounter = new FixedFrameTimeCounter();

        public readonly FrameBuffer kFrameBuffer = new FrameBuffer();

        private LockStepLogicUpdateSystem m_LogicUpdateSystem ;
        private LockStepReplayUpdateSystem m_ReplayUpdateSystem;

        /// <summary>
        /// 开始时间
        /// </summary>
        public long startTime;

        /// <summary>
        /// 权威帧
        /// </summary>
        public int authorityFrame;

        /// <summary>
        /// 预测帧
        /// </summary>
        public int predictionFrame;
        
        /// <summary>
        /// 是否在回放逻辑
        /// </summary>
        public bool IsReplay { set; get; } = false;
        /// <summary>
        /// 是否保存回放
        /// </summary>
        public bool IsSaveReplay { set; get; } = false;
        
        public readonly LockStepReplay kLockStepReplay = new LockStepReplay();

        public LockStepSystem()
        {
            m_LogicUpdateSystem = new LockStepLogicUpdateSystem(this);
            m_ReplayUpdateSystem = new LockStepReplayUpdateSystem(this);
        }
        
        public void StartTick(long startTime,int frame)
        {
            this.startTime = startTime;
            this.authorityFrame = frame;
            this.predictionFrame = frame;
            fixedFrameTimeCounter.Init(startTime, frame,LockStepConstValue.k_UpdateInterval);
            kFrameBuffer.Init(frame);
            
            LockStepModuleSingletom.instance.RegisterUpdate(this);
            Debug.Log($"开始帧同步！！！！startTime:{startTime},frame:{frame}");
        }
        
        public void StopTick()
        {
            LockStepModuleSingletom.instance.UnRegisterUpdate(this);
            if (!IsReplay && IsSaveReplay)
            {
                kLockStepReplay.SaveSnapshotsToFile();
            }
        }

        public void Update()
        {
            if (IsReplay)
            {
                m_ReplayUpdateSystem.Update();
            }
            else
            {
                m_LogicUpdateSystem.Update();
            }
        }
        /// <summary>
        /// 帧逻辑执行到的地方
        /// </summary>
        /// <param name="oneFrameInputs"></param>
        public void LSUpdate(OneFrameInputs oneFrameInputs)
        {
            k_lockStepUpdater.Add(oneFrameInputs);
            //TDOO:先用权威帧代替
            k_lockStepUpdater.LSUpdate(authorityFrame);
            if (!IsReplay)
            {
                Record(authorityFrame);
            }
        }

        /// <summary>
        /// 收集每帧的数据用于回滚或者重播逻辑
        /// </summary>
        public void Record(int frame)
        {
            OneFrameInputs recordFrameInputs = OneFrameInputs.Create();
            OneFrameInputs frameInputs = kFrameBuffer.GetFrameInputs(frame);
            frameInputs.CopyTo(recordFrameInputs);
            kLockStepReplay.FrameInputs.Add(recordFrameInputs);
            //到了阈值保存内存快照
            if (frame % LockStepConstValue.k_SaveFrameCount == 0)
            {
                kLockStepReplay.SaveSnapshot();
                Debug.Log("保存内存快照~~~~~~~~");
            }
        }

        public void Replay()
        {
            IsReplay = true;
            kLockStepReplay.DeSerializeSnapshots();
            StartTick(TimeInfo.instance.ClientNow(),0);
        }

        public void JumpToReplayFrame(int frame)
        {
            Debug.Log($"跳到指定帧:{frame}");
        }
        
        // public void AddFrameInput(LockStepInput lockStepInput)
        // {
        //     //TDOO:先用权威帧代替
        //     //下一帧加
        //     int nextFrame = this.authorityFrame + 1;
        //     OneFrameInputs oneFrameInputs = kFrameBuffer.GetFrameInputs(nextFrame);
        //     oneFrameInputs.AddFrameInput(lockStepInput);
        // }
    }
}