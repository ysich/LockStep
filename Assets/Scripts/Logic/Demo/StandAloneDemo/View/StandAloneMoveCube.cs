/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-31 14:55:50
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using Core;
using TrueSync;
using UnityEngine;
using DG.Tweening;
using UnityEditor.UI;

namespace Demo
{
    public class StandAloneMoveCube:MonoBehaviour
    {
        private StandAloneDemoData m_Data;

        private float distance;
        private float speed = 2f;
        private float totalTime;
        private float t;
        private void Awake()
        {
            m_Data = GameDatas.instance.GetData<StandAloneDemoData>();
            EventBusSingleton.instance.RegisterEvent<TSVector2>(EventBusSingletonDefine.Demo_One_Move,Move);
        }

        private void Update()
        {
            t += Time.deltaTime;
            transform.position =Vector3.Lerp(transform.position, m_Data.postion, t / totalTime);
        }

        public void Move(TSVector2 vector2)
        {
            distance = (m_Data.postion - transform.position).magnitude;
            totalTime = distance / speed;
            t = 0f;
        }

        private void OnDestroy()
        {
            EventBusSingleton.instance.UnregisterEvent<TSVector2>(EventBusSingletonDefine.Demo_One_Move,Move);
        }
    }
}