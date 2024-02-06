/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-31 17:55:15
-- 概述:
---------------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.UIElements;

namespace Demo
{
    public class StandAloneDemoData:ILogicData
    {
        public Vector3 postion { get; set; } = Vector3.zero;

        public void Clear()
        {
            postion = Vector3.zero;
        }
    }
}