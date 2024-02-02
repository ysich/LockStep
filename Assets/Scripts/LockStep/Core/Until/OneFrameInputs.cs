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
        // public List<LockStepInput> inputQueue = new List<LockStepInput>(2);
        public LockStepInput Input = new LockStepInput();
        
        public void Clear()
        {
            // inputQueue.Clear();
            Input = new LockStepInput();
        }

        public void CopyTo(OneFrameInputs oneFrameInputs)
        {
            oneFrameInputs.Clear();
            // foreach (var lsCommand in inputQueue)
            // {
            //     oneFrameInputs.AddFrameInput(lsCommand);    
            // }
            oneFrameInputs.Input = Input;
        }
        
        public static bool operator ==(OneFrameInputs left,OneFrameInputs right)
        {
            if (left is null || right is null)
            {
                if (left is null && right is null)
                {
                    return true;
                }

                return false;
            }

            return left.Input.Equals(right.Input);
        }
        
        public static bool operator !=(OneFrameInputs left, OneFrameInputs right)
        {
            return !(left == right);
        }

        public bool Equals(OneFrameInputs right)
        {
            if (ReferenceEquals(this, null))
            {
                return false;
            }

            return ReferenceEquals(this, right);
        }

        public static OneFrameInputs Create()
        {
            return ObjectPool.instance.Fetch<OneFrameInputs>();
        }

        public void Dispose()
        {
            Clear();
            ObjectPool.instance.Recycle(this);
        }
    }
}