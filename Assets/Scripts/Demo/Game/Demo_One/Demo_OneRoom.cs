/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-18 17:07:45
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using Core;
using LockStep;
using LockStep.Define;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class Demo_OneRoom:MonoBehaviour
    {
        private Button BtnRecord;
        private TextMeshProUGUI TxtBtnRecord;
        private Button BtnReplay;
        private Button BtnReplayQuick;
        private Button BtnReplaySlow;
        private TextMeshProUGUI TxtReplaySpeed;
        private Slider ReplayProgress;
        private float m_ReplaySpeedStep = 0.1f;
        private float m_ReplaySpeed = 1;
        
        private Button BtnStartGame;
        private Button BtnStopGame;
        private Button BtnLeft;
        private Button BtnRight;
        private Button BtnUp;
        private Button BtnDown;

        private LSCommandModuleDef m_commandModuleDef = LSCommandModuleDef.Demo_One;

        private LockStepSystem lockStepSystem;

        private void Awake()
        {
            BtnRecord = transform.Find("Replay/BtnRecord").GetComponent<Button>();
            TxtBtnRecord = BtnRecord.GetComponentInChildren<TextMeshProUGUI>();
            BtnReplay = transform.Find("Replay/BtnReplay").GetComponent<Button>();
            BtnReplayQuick = transform.Find("Replay/BtnReplayQuick").GetComponent<Button>();
            BtnReplayQuick.GetComponentInChildren<TextMeshProUGUI>().text = "+" + m_ReplaySpeedStep;
            BtnReplaySlow = transform.Find("Replay/BtnReplaySlow").GetComponent<Button>();
            BtnReplaySlow.GetComponentInChildren<TextMeshProUGUI>().text = "-" + m_ReplaySpeedStep;
            TxtReplaySpeed = transform.Find("Replay/TxtReplaySpeed").GetComponent<TextMeshProUGUI>();
            ReplayProgress = transform.Find("Replay/ReplayProgress").GetComponent<Slider>();
            BtnRecord.onClick.AddListener(OnBtnRecord);
            BtnReplay.onClick.AddListener(OnBtnReplay);
            BtnReplayQuick.onClick.AddListener(OnBtnReplayQuick);
            BtnReplaySlow.onClick.AddListener(OnBtnReplaySlow);
            ReplayProgress.onValueChanged.AddListener(OnReplayProgressValueChange);
            
            BtnStartGame = transform.Find("BtnStartGame").GetComponent<Button>();
            BtnStopGame = transform.Find("BtnStopGame").GetComponent<Button>();
            BtnLeft = transform.Find("BtnLeft").GetComponent<Button>();
            BtnRight = transform.Find("BtnRight").GetComponent<Button>();
            BtnUp = transform.Find("BtnUp").GetComponent<Button>();
            BtnDown = transform.Find("BtnDown").GetComponent<Button>();
            BtnStartGame.onClick.AddListener(OnBtnStartGame);
            BtnStopGame.onClick.AddListener(OnBtnStopGame);
            BtnLeft.onClick.AddListener(()=>AddInputCommand(Demo_OneCommandDef.Left));
            BtnRight.onClick.AddListener(()=>AddInputCommand(Demo_OneCommandDef.Right));
            BtnUp.onClick.AddListener(()=>AddInputCommand(Demo_OneCommandDef.Up));
            BtnDown.onClick.AddListener(()=>AddInputCommand(Demo_OneCommandDef.Down));

            LockStepModuleSingletom.instance.TryGetCommandModule(LSCommandModuleDef.Demo_One, out lockStepSystem);
            RefreshBtnRecord();

            EventBusSingleton.instance.RegisterEvent<int>(EventBusSingletonDefine.FrameTick,RefreshFrameProgress);
        }

        public void OnBtnStartGame()
        {
            Debug.Log("StartGame");
            lockStepSystem.StartTick(TimeInfo.instance.ClientNow(),0);
        }

        public void OnBtnStopGame()
        {
            Debug.Log("StopGame");
            lockStepSystem.StopTick();
        }

        #region Replay

        private void OnBtnRecord()
        {
            lockStepSystem.IsSaveReplay = !lockStepSystem.IsSaveReplay;
            RefreshBtnRecord();
        }

        private void OnBtnReplay()
        {
            lockStepSystem.Replay();
            RefreshReplaySpeed();
            ReplayProgress.minValue = 0;
            ReplayProgress.maxValue = lockStepSystem.kLockStepReplay.FrameInputs.Count;
            ReplayProgress.value = 0;
        }

        /// <summary>
        /// 加快播放速度
        /// </summary>
        private void OnBtnReplayQuick()
        {
            m_ReplaySpeed += m_ReplaySpeedStep;
            RefreshReplaySpeed();
        }
        /// <summary>
        /// 减慢播放速度
        /// </summary>
        private void OnBtnReplaySlow()
        {
            m_ReplaySpeed -= m_ReplaySpeedStep;
            if (m_ReplaySpeed < m_ReplaySpeedStep)
            {
                m_ReplaySpeed = m_ReplaySpeedStep;
            }
            RefreshReplaySpeed();
        }

        private void OnReplayProgressValueChange(float value)
        {
            int targetFrame = (int)value;
            lockStepSystem.JumpToReplayFrame(targetFrame);
        }

        private void RefreshReplaySpeed()
        {
            float newInterval = LockStepConstValue.k_UpdateInterval / m_ReplaySpeed;
            lockStepSystem.fixedFrameTimeCounter.ChangeInterval((int)newInterval, lockStepSystem.frame);
            TxtReplaySpeed.text = "Speed:" + m_ReplaySpeed;
        }

        private void RefreshBtnRecord()
        {
            TxtBtnRecord.text = lockStepSystem.IsSaveReplay ? "StopRecord" : "StartRecord";
        }

        private void RefreshFrameProgress(int frame)
        {
            ReplayProgress.value = frame;
        }

        #endregion

        private void AddInputCommand(Demo_OneCommandDef demoOneCommandDef)
        {
            LSCommand lsCommand = new LSCommand(m_commandModuleDef, (int)demoOneCommandDef);
            lockStepSystem.AddFrameInput(lsCommand);      
        }
        
    }
}