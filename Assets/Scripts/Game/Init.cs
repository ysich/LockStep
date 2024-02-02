using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using LockStep;
using UnityEngine;

namespace Demo
{
    public class Init : MonoBehaviour
    {
        public GameObject Demo_One;
        void Start()
        {
            SingletonMgr singletonMgr = new SingletonMgr();
            singletonMgr.Awake();

            Instantiate(Demo_One, transform);
        }

        private void Update()
        {
            TimeInfo.instance.Update();
            LockStepModuleSingletom.instance.Update();
        }
    }
}