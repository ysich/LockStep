/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 11:24:26
-- 概述: 帧同步-重播记录逻辑，重播数据源
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using LockStep.Define;
using MemoryPack;
using UnityEngine;
using UnityEngine.Windows;
using Until;


namespace LockStep
{
    public class LockStepReplay
    {
        /// <summary>
        /// 记录每一帧的操作
        /// </summary>
        public List<OneFrameInputs> FrameInputs = new List<OneFrameInputs>();
        
        /// <summary>
        /// 内存快照
        /// </summary>
        public List<byte[]> Snapshots = new List<byte[]>();

        /// <summary>
        /// 保存内存快照
        /// </summary>
        public void SaveSnapshot()
        {
            byte[] snapshot = MemoryPackSerializer.Serialize(FrameInputs);
            //这里显式Dispose,不然回收不了
            foreach (var oneFrameInput in FrameInputs)
            {
                oneFrameInput.Dispose();
            }
            FrameInputs.Clear();
            Snapshots.Add(snapshot);
        }
        
        /// <summary>
        /// 保存内存快照到本地
        /// </summary>
        public void SaveSnapshotsToFile()
        {
            //保存到本地的时候需要把剩下的数据保存到快照
            if (FrameInputs.Count>0)
            {
                SaveSnapshot();
            }
            byte[] snapshots = MemoryPackSerializer.Serialize(Snapshots);
            Snapshots.Clear();
            FrameInputs.Clear();
            FileHelper.SaveBytesToPath(LockStepConstValue.k_ReplaySavePath,snapshots);
            Debug.Log($"快照保存到本地目录：{LockStepConstValue.k_ReplaySavePath}!!");
        }

        public void DeSerializeSnapshots()
        {
            byte[] snapshotsDatas =  File.ReadAllBytes(LockStepConstValue.k_ReplaySavePath);
            Snapshots = MemoryPackSerializer.Deserialize<List<byte[]>>(snapshotsDatas);
            //TODO：先在这里全部序列化，后面再做分段序列化的逻辑
            foreach (var snapshot in Snapshots)
            {
                List<OneFrameInputs> oneFrameInputsList = MemoryPackSerializer.Deserialize<List<OneFrameInputs>>(snapshot);
                FrameInputs.AddRange(oneFrameInputsList);
            }
        }
    }
}