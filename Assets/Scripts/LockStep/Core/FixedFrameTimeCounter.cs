/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-17 16:39:26
-- 概述: 用来计算每帧时间，到达FixedFrameTime的时间才会调用Update。动态调整每帧间隔。
---------------------------------------------------------------------------------------*/

namespace LockStep
{
    /// <summary>
    /// 帧时间计算组件
    /// </summary>
    public class FixedFrameTimeCounter
    {
        private long startTime;
        private int startFrame;
        public int Interval { get; private set; }

        // public FixedFrameTimeCounter(long startTime, int startFrame, int interval)
        // {
        //     this.startTime = startTime;
        //     this.startFrame = startFrame;
        //     this.Interval = interval;
        // }

        public void Init(long startTime, int startFrame, int interval)
        {
            this.startTime = startTime;
            this.startFrame = startFrame;
            this.Interval = interval;
        }
        
        /// <summary>
        /// 调整每一帧之间的间隔
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="frame"></param>
        public void ChangeInterval(int interval, int frame)
        {
            this.startTime += (frame - this.startFrame) * this.Interval;
            this.startFrame = frame;
            this.Interval = interval;
        }

        /// <summary>
        /// 获取某一帧的时间戳
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public long FrameTime(int frame)
        {
            return this.startTime + (frame - this.startFrame) * this.Interval;
        }
        
        public void Reset(long time, int frame)
        {
            this.startTime = time;
            this.startFrame = frame;
        }
    }
}