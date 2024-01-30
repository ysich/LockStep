/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 14:54:16
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Core;
using MemoryPack;
using UnityEngine;

namespace LockStep
{
    [MemoryPackable(SerializeLayout.Explicit)]
    public partial class OneFrameInputs:IDisposable
    {
        [MemoryPackOrder(0)]
        public List<LockStepInput> inputQueue = new List<LockStepInput>(2);

        public void AddFrameInput(LockStepInput command)
        {
            inputQueue.Add(command);
        }
        
        public void Reset()
        {
            inputQueue.Clear();
        }

        public void CopyTo(OneFrameInputs oneFrameInputs)
        {
            oneFrameInputs.Reset();
            foreach (var lsCommand in inputQueue)
            {
                oneFrameInputs.AddFrameInput(lsCommand);    
            }
        }

        public static OneFrameInputs Create()
        {
            return ObjectPool.instance.Fetch<OneFrameInputs>();
        }

        public void Dispose()
        {
            Reset();
            ObjectPool.instance.Recycle(this);
        }
    }
}