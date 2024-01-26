/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 15:46:06
-- 概述:
---------------------------------------------------------------------------------------*/

using System;

namespace Core
{
    public abstract class ASingleton:IDisposable
    {
        internal abstract void Register();
        public virtual void Dispose()
        {
            
        }
    }
}