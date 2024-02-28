/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-06 14:59:50
-- 概述:
---------------------------------------------------------------------------------------*/

using UnityEngine;

namespace Demo
{
    public class NetworkDemoData:ILogicData
    {
        public Vector3 postion { get; set; } = Vector3.zero;

        public void Clear()
        {
            postion = Vector3.zero;
        }
    }
}