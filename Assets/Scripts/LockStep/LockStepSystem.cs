/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-17 15:20:16
-- 概述: 帧同步逻辑层，可以拥有多个逻辑层，每个逻辑层之间不会互相影响。
---------------------------------------------------------------------------------------*/

using Core;
using LockStep.Define;
using MemoryPack;
using UnityEngine;

namespace LockStep
{
    public abstract partial class LockStepSystem
    {
        private readonly LockStepUpdater k_lockStepUpdater = new LockStepUpdater();
        
        public readonly FixedFrameTimeCounter fixedFrameTimeCounter = new FixedFrameTimeCounter();

        private readonly FrameBuffer m_FrameBuffer = new FrameBuffer();

        /// <summary>
        /// 开始时间
        /// </summary>
        public long startTime;
        /// <summary>
        /// 当前帧数
        /// </summary>
        public int frame;
        
        /// <summary>
        /// 是否在回放逻辑
        /// </summary>
        public bool IsReplay { set; get; } = false;
        /// <summary>
        /// 是否保存回放
        /// </summary>
        public bool IsSaveReplay { set; get; } = false;
        
        public readonly LockStepReplay kLockStepReplay = new LockStepReplay();

        public void StartTick(long startTime,int frame)
        {
            this.startTime = startTime;
            this.frame = frame;
            fixedFrameTimeCounter.Init(startTime, frame,LockStepConstValue.k_UpdateInterval);
            m_FrameBuffer.Init(frame);
            // m_OneFrameInputs = OneFrameInputs.Create();
            
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

        private long lastFrameTime = 0;
        public void Update()
        {
            while(true)
            {
                //没到下一帧时间则不执行
                if (TimeInfo.instance.ClientNow() < fixedFrameTimeCounter.FrameTime(frame + 1))
                {
                    break;
                }

                ++frame;

                OneFrameInputs frameInputs = m_FrameBuffer.GetFrameInputs(frame);
                //如果是播放录制文件那么从这里取
                if (IsReplay)
                {
                    //超过重播的帧数直接退出
                    if (frame >= kLockStepReplay.FrameInputs.Count)
                    {
                        StopTick();
                        break;
                    }
                    OneFrameInputs replayFrameInputs = kLockStepReplay.FrameInputs[frame];
                    replayFrameInputs.CopyTo(frameInputs);
                }
                //下一帧再加，直接加会吧顺序打乱掉
                k_lockStepUpdater.Add(frameInputs);

                long frameTime = TimeInfo.instance.ClientNow();
                Debug.Log($"LockStepUpdate_Frame:{frame},期间时间间隔为{frameTime-lastFrameTime}");
                lastFrameTime = frameTime;
                    
                k_lockStepUpdater.LSUpdate(frame);
                //TODO:现在Replay需要表现先写这里！
                Tick(frame);
                //屏幕录制
                if (!IsReplay)
                {
                    Record();
                }
                //下一帧清空一下
                OneFrameInputs nextOneFrameInputs = m_FrameBuffer.GetFrameInputs(frame + 1);
                nextOneFrameInputs.Reset();
            }
        }

        public void Record()
        {
            OneFrameInputs recordFrameInputs = OneFrameInputs.Create();
            OneFrameInputs frameInputs = m_FrameBuffer.GetFrameInputs(frame);
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

        public void AddFrameInput(LSCommand lsCommand)
        {
            //下一帧加
            int nextFrame = this.frame + 1;
            OneFrameInputs oneFrameInputs = m_FrameBuffer.GetFrameInputs(nextFrame);
            oneFrameInputs.AddFrameInput(lsCommand);
        }
    }
}