/*---------------------------------------------------------------------------------------
-- 负责人: ming.zhang
-- 创建时间: 2023-03-13 20:31:39
-- 概述:
---------------------------------------------------------------------------------------*/

namespace Core
{
    public class Singleton<T> :ASingleton where T : Singleton<T>
    {
        private static T s_Instance;

        private static object helper_lock = new object();

        public static T instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (helper_lock)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = System.Activator.CreateInstance<T>();
                        }
                    }
                }
                return s_Instance;
            }
        }
        internal override void Register()
        {
            s_Instance = (T)this;
        }

        public override void Dispose()
        {
            s_Instance = null;
        }
    }
}
