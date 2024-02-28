/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-07 10:05:21
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using LockStep;
using UnityEngine;

namespace Demo
{
    public class NetworkSyncSystemSingleton:Singleton<NetworkSyncSystemSingleton>,ISingletonAwake
    {
        private int m_Frame { get; set; } = 0;

        private readonly Queue<OneFrameInputs> m_OneFrameInputsQueue = new Queue<OneFrameInputs>();

        // private FrameBuffer m_FrameBuffer = new FrameBuffer();

        private CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();
        private bool m_IsConnect { get; set; } = false;

        private int m_CurUpdateDelay;

        public int curUpdateDelay
        {
            get
            {
                return m_CurUpdateDelay;
            }
            set
            {
                m_CurUpdateDelay = value;
                EventBusSingleton.instance.Publish<int>(EventBusSingletonDefine.Network_UpdateDelay,value);
            }
        }
        private int m_Delay { get; set; } = 0;
        
        public void Awake()
        {
        }

        public void Connect(int frame,int delay)
        {
            m_IsConnect = true;
            ChangeDelay(delay);
            StartSync(frame);
        }

        public void Disconnect()
        {
            m_IsConnect = false;
            m_CancellationTokenSource.Cancel();
        }

        public void Receive(int frame,OneFrameInputs receiveOneFrameInputs)
        {
            OneFrameInputs oneFrameInputs = OneFrameInputs.Create();
            receiveOneFrameInputs.CopyTo(oneFrameInputs);
            m_OneFrameInputsQueue.Enqueue(receiveOneFrameInputs);
        }

        private void Send(int frame,OneFrameInputs oneFrameInputs,int delay)
        {
            EventBusSingleton.instance.Publish<int,OneFrameInputs,int>(EventBusSingletonDefine.Network_Sync,frame,oneFrameInputs,delay);
        }

        public void ChangeDelay(int delay)
        {
            m_Delay = delay;
        }
        
        private async UniTaskVoid Sync()
        {
            
            int delay = m_Delay;
            float delayTime = delay * 0.001f;
            curUpdateDelay = delay;
            if (m_OneFrameInputsQueue.Count <= 0)
            {
                await UniTask.DelayFrame(1);
                Sync();
                return;
            }
            if (delayTime <= 0)
            {
                await UniTask.DelayFrame(1);
                SyncFrameData(delay, delayTime);
                Sync();
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime),cancellationToken:m_CancellationTokenSource.Token);

            SyncFrameData(delay, delayTime);
            
            Sync();
        }

        private void SyncFrameData(int delay,float delayTime)
        {
            m_Frame += 1;
            
            OneFrameInputs oneFrameInputs = m_OneFrameInputsQueue.Dequeue();
            Send(m_Frame,oneFrameInputs,delay);
            oneFrameInputs.Dispose();
            Debug.Log($"网络同步！！！！延迟:{delayTime}s = {delay}ms");
        }

        private void StartSync(int frame)
        {
            m_Frame = frame;
            Sync();
        }
    }
}