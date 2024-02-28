/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-31 17:46:08
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Core;

namespace Demo
{
    public class GameDatas:Singleton<GameDatas>,ISingletonAwake
    {
        private Dictionary<Type, ILogicData> m_LogicDatas;
        public void Awake()
        {
            m_LogicDatas = new Dictionary<Type, ILogicData>();

            AddData<StandAloneDemoData>();
            AddData<NetworkDemoData>();
        }

        public T AddData<T>()where T:ILogicData,new()
        {
            T data = new T();
            Type type = typeof(T);
            m_LogicDatas.Add(type,data);
            return data;
        }

        public T GetData<T>() where T : ILogicData,new()
        {
            Type type = typeof(T);
            if (m_LogicDatas.ContainsKey(type))
            {
                return (T)m_LogicDatas[type];
            }

            return AddData<T>();
        }
    }
}