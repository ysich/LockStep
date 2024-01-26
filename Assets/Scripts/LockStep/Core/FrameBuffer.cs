/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-23 10:19:49
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Core;
using Demo;
using LockStep.Define;
using UnityEngine.Pool;

namespace LockStep
{
    public class FrameBuffer:IDisposable
    {
        private readonly List<OneFrameInputs> m_FrameInputs ;

        /// <summary>
        /// 每一帧的HashCode，用来比对权威帧和预测帧是否一致
        /// </summary>
        private readonly List<int> m_HashCodes;
        private int m_Capacity;

        /// <summary>
        /// Buffer里的最大帧数
        /// </summary>
        private int m_MaxFrame;

        public FrameBuffer(int capacity = LockStepConstValue.k_FrameCountSecond * 30)
        {
            m_Capacity = capacity;
            m_FrameInputs = new List<OneFrameInputs>(capacity);
            m_HashCodes = new List<int>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                OneFrameInputs oneFrameInputs = OneFrameInputs.Create();
                m_FrameInputs.Add(oneFrameInputs);
            }
        }

        public void Init(int frame)
        {
            m_MaxFrame = frame + m_MaxFrame;
        }

        public OneFrameInputs GetFrameInputs(int frame)
        {
            int frameIndex = frame % LockStepConstValue.k_SaveFrameCount;
            return m_FrameInputs[frameIndex];
        }
        
        public bool CheckFrame(int frame)
        {
            if (frame < 0)
            {
                return false;
            }

            if (frame > this.m_MaxFrame)
            {
                return false;
            }

            return true;
        }
        
        
        public static FrameBuffer Create()
        {
            return ObjectPool.instance.Fetch<FrameBuffer>();
        }
        public void Dispose()
        {
            ObjectPool.instance.Recycle(this);
        }
    }
}