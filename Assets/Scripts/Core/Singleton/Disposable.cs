/*---------------------------------------------------------------------------------------
-- 负责人: ming.zhang
-- 创建时间: 2023-03-13 20:32:57
-- 概述:
---------------------------------------------------------------------------------------*/

using System;

namespace Core
{
    public class Disposable : IDisposable
    {
        private bool m_Disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any Other objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            m_Disposed = true;
        }
    }
}
