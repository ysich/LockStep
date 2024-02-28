/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-17 15:20:16
-- 概述: 帧同步逻辑层，可以拥有多个逻辑层，每个逻辑层之间不会互相影响。
---------------------------------------------------------------------------------------*/

using System;
using Core;
using LockStep.Define;
using MemoryPack;
using UnityEngine;
using Core;

namespace LockStep
{
    public partial class LockStepSystem:ILockStepAwake
    {
        // private readonly LockStepUpdater k_lockStepUpdater = new LockStepUpdater();
        
        public readonly FixedFrameTimeCounter fixedFrameTimeCounter = new FixedFrameTimeCounter();

        public readonly FrameBuffer kFrameBuffer = new FrameBuffer();

        public LockStepLogicUpdateSystem logicUpdateSystem ;
        public LockStepReplayUpdateSystem replayUpdateSystem;
        
        private LSLogicSystemBase m_runningLogicSystem;

        public LSLogicSystemBase runningLogicSystem
        {
            private set { m_runningLogicSystem = value; }
            get
            {
                if (m_runningLogicSystem == null)
                {
                    Debug.LogError("LockStepSystem:没有正在运行的逻辑类！！检查逻辑！！");
                }
                return m_runningLogicSystem;
            }
        }

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

        public void Awake()
        {
            replayUpdateSystem = new LockStepReplayUpdateSystem(this);
            InitModule();
        }

        public void StartTick(long startTime,int frame,LSLogicSystemBase lsLogicSystem)
        {
            this.StopTick();
            
            this.startTime = startTime;
            this.authorityFrame = frame;
            this.predictionFrame = frame;
            fixedFrameTimeCounter.Init(startTime, frame,LockStepConstValue.k_UpdateInterval);
            kFrameBuffer.Init(frame);

            runningLogicSystem = lsLogicSystem;
            // LockStepModuleSingletom.instance.RegisterUpdate(this);
            Debug.Log($"开始帧同步！！！！startTime:{startTime},frame:{frame}");
        }
        
        public void StopTick()
        {
            runningLogicSystem = null;
            // LockStepModuleSingletom.instance.UnRegisterUpdate(this);
            if (!IsReplay && IsSaveReplay)
            {
                kLockStepReplay.SaveSnapshotsToFile();
            }
            Debug.Log($"帧同步停止!!!frame:{authorityFrame}");
        }

        public void Update()
        {                    
            //这里没有正在跑的逻辑则直接退出
            if (m_runningLogicSystem == null)
            {
                return;
            }
            if (IsReplay)
            {
                replayUpdateSystem.Update();
            }
            else
            {
                //这里如果调用InputSystem的Update频率和逻辑帧的频率会不同，会是Unity内Update的调用频率。导致一些操作收集会被覆盖。
                // //逻辑执行到之前先收集操作指令
                // lockStepInputOperationSystem.Update();
                logicUpdateSystem.Update();
            }
        }

        private long lastTime = 0;
        /// <summary>
        /// 帧逻辑执行到的地方
        /// </summary>
        /// <param name="oneFrameInputs"></param>
        public void LSUpdate(OneFrameInputs oneFrameInputs)
        {
            long timeNow =  TimeInfo.instance.ClientNow();
            Debug.Log($"逻辑帧执行,权威帧:{authorityFrame},预测帧:{predictionFrame},时间间隔{timeNow - lastTime}");
            lastTime = timeNow;
            
            runningLogicSystem.Run(oneFrameInputs);
            // k_lockStepUpdater.oneFrameInputs = oneFrameInputs;
            // k_lockStepUpdater.LSUpdate();
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

        public void Replay(LSLogicSystemBase lsLogicSystemBase)
        {
            IsReplay = true;
            kLockStepReplay.DeSerializeSnapshots();
            StartTick(TimeInfo.instance.ClientNow(),0,lsLogicSystemBase);
        }

        public void JumpToReplayFrame(int frame)
        {
            Debug.Log($"跳到指定帧:{frame}");
        }
    }
}