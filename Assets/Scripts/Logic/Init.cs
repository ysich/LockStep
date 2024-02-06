using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using LockStep;
using UnityEngine;

namespace Demo
{
    public enum DemoType
    {
        StandAloneLSDemo,
        NetworkLSDemo,
    }
    public class Init : MonoBehaviour
    {
        public DemoType demoType;
        void Start()
        {
            SingletonMgr singletonMgr = new SingletonMgr();
            singletonMgr.Awake();

            LoadDemo();
        }

        private void Update()
        {
            TimeInfo.instance.Update();
            LockStepModuleSingletom.instance.Update();
        }

        private void LoadDemo()
        {
            GameObject demo;
            switch (demoType)
            {
                case  DemoType.StandAloneLSDemo:
                    demo = Resources.Load<GameObject>("Prefab/Demo/StandAlone/StandAloneDemo");
                    Instantiate(demo, transform);
                    break;
                case  DemoType.NetworkLSDemo:
                    demo = Resources.Load<GameObject>("Prefab/Demo/Network/NetworkDemo");
                    Instantiate(demo, transform);
                    break;
            }

        }
    }
}