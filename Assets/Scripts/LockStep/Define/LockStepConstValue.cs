/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-17 14:42:04
-- 概述:
---------------------------------------------------------------------------------------*/

using UnityEngine;

namespace LockStep.Define
{
    public class LockStepConstValue
    {
        /// <summary>
        /// 帧同步一帧的时间
        /// </summary>
        public const int k_UpdateInterval = 50;

        /// <summary>
        /// 一秒的帧数
        /// </summary>
        public const int k_FrameCountSecond = 1000 / k_UpdateInterval;
        
        /// <summary>
        /// 一分钟存一个内存快照避免占用内存太大
        /// TODO:Test一下，先设置十秒保存一次
        /// </summary>
        public const int k_SaveFrameCount = 10 * k_FrameCountSecond;
        
        public static string k_ReplaySavePath = Application.streamingAssetsPath + "/LSReplayData.byte";
    }
}