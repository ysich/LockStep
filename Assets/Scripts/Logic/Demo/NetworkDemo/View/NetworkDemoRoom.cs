/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-02-06 15:00:48
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using Core;
using LockStep;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class NetworkDemoRoom:MonoBehaviour
    {
        public bool interrupt = false;
        private int m_Delay = 0;
        private NetworkDemoData m_Data;
        private NetworkLSSystem m_NetworkLsSystem;
        private NetworkDemoSystem m_NetworkDemoSystem;

        private Button m_BtnInterrupt;
        private Text m_TxtBtnInterrupt;
        private Button m_BtnNetworkDelayQuick;
        private Button m_BtnNetworkDelaySlow;
        private Text m_TxtSettingDelay;
        private Text m_TxtDelay;

        private Button BtnRecord;
        private TextMeshProUGUI TxtBtnRecord;
        private Button BtnReplay;
        private Button BtnReplayQuick;
        private Button BtnReplaySlow;
        private TextMeshProUGUI TxtReplaySpeed;
        private Slider ReplayProgress;
        private int m_ReplaySpeedStep = 1;
        private int m_ReplaySpeed = 1;
        
        private Button BtnStartGame;
        private Button BtnStopGame;
        private Button BtnSkill_1;
        private Button BtnSkill_2;
        private Button BtnSkill_3;
        private Button BtnSkill_4;
        private void Awake()
        {
            m_BtnInterrupt = transform.Find("Network/BtnInterrupt").GetComponent<Button>();
            m_TxtBtnInterrupt = m_BtnInterrupt.GetComponentInChildren<Text>();
            m_BtnNetworkDelayQuick = transform.Find("Network/BtnNetworkDelayQuick").GetComponent<Button>();
            m_BtnNetworkDelaySlow = transform.Find("Network/BtnNetworkDelaySlow").GetComponent<Button>();
            m_TxtSettingDelay = transform.Find("Network/TxtSettingDelay").GetComponent<Text>();
            m_TxtDelay = transform.Find("Network/TxtDelay").GetComponent<Text>();
            m_BtnNetworkDelayQuick.onClick.AddListener(()=>ChangeDelay(50));
            m_BtnNetworkDelaySlow.onClick.AddListener(()=>ChangeDelay(-50));
            
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
            BtnSkill_1 = transform.Find("BtnSkill_1").GetComponent<Button>();
            BtnSkill_2 = transform.Find("BtnSkill_2").GetComponent<Button>();
            BtnSkill_3 = transform.Find("BtnSkill_3").GetComponent<Button>();
            BtnSkill_4 = transform.Find("BtnSkill_4").GetComponent<Button>();
            
            BtnStartGame.onClick.AddListener(OnBtnStartGame);
            BtnStopGame.onClick.AddListener(OnBtnStopGame);
            BtnSkill_1.onClick.AddListener(()=>AddFrameInput(NetworkCommandDef.Skill_1));
            BtnSkill_2.onClick.AddListener(()=>AddFrameInput(NetworkCommandDef.Skill_2));
            BtnSkill_3.onClick.AddListener(()=>AddFrameInput(NetworkCommandDef.Skill_3));
            BtnSkill_4.onClick.AddListener(()=>AddFrameInput(NetworkCommandDef.Skill_4));
            
        }

        private void Start()
        {
            m_NetworkLsSystem = LockStepModuleSingletom.instance.GetModule<NetworkLSSystem>();
            m_NetworkDemoSystem = m_NetworkLsSystem.GetModule<NetworkDemoSystem>();
            RefreshNetworkDelay(0);
            RefreshSettingDelay();
            RefreshNetworkState();
            AddEvent();
        }
        private void AddEvent()
        {
            EventBusSingleton.instance.RegisterEvent<int>(EventBusSingletonDefine.Network_UpdateDelay,RefreshNetworkDelay);
        }

        private void StartGame()
        {
            RefreshSettingDelay();
            RefreshNetworkState();
        }

        private void AddFrameInput(NetworkCommandDef networkCommandDef)
        {
            m_NetworkDemoSystem.AddFrameInput(new LockStepInput(LockStepCommandModuleDef.Network,(int)networkCommandDef));
        }
        
        public void OnBtnStartGame()
        {
            Debug.Log("StartGame");
            StartGame();
            m_NetworkDemoSystem.StartTick(TimeInfo.instance.ClientNow(),0,m_Delay);
        }

        public void OnBtnStopGame()
        {
            Debug.Log("StopGame");
            m_NetworkDemoSystem.StopTick();
            NetworkSyncSystemSingleton.instance.Disconnect();
        }

        #region network

        private void ChangeDelay(int delayDiff)
        {
            m_Delay += delayDiff;
            NetworkSyncSystemSingleton.instance.ChangeDelay(m_Delay);
            RefreshSettingDelay();
        }
        

        private void RefreshNetworkState()
        {
            string str = interrupt ? "网络恢复" : "模拟掉包";
            m_TxtBtnInterrupt.text = str;
        }
        private void RefreshSettingDelay()
        {
            m_TxtSettingDelay.text = $"SettingDelay:{m_Delay}ms";
        }

        private void RefreshNetworkDelay(int delay)
        {
            m_TxtDelay.text = $"Delay:{m_Delay}ms";
        }

        #endregion

        #region Replay

        private void OnBtnRecord()
        {
            m_NetworkLsSystem.IsSaveReplay = !m_NetworkLsSystem.IsSaveReplay;
            RefreshBtnRecord();
        }

        private void OnBtnReplay()
        {
            StartGame();
            m_NetworkDemoSystem.Replay();
            ReplayProgress.minValue = 0;
            ReplayProgress.maxValue = m_NetworkLsSystem.kLockStepReplay.FrameInputs.Count;
            ReplayProgress.value = 0;
            RefreshReplaySpeed();
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
            m_NetworkLsSystem.JumpToReplayFrame(targetFrame);
        }

        private void RefreshReplaySpeed()
        {
            m_NetworkLsSystem.replayUpdateSystem.ChangeReplaySpeed(m_ReplaySpeed);
            TxtReplaySpeed.text = "Speed:" + m_ReplaySpeed;
        }

        private void RefreshBtnRecord()
        {
            TxtBtnRecord.text = m_NetworkLsSystem.IsSaveReplay ? "StopRecord" : "StartRecord";
        }

        private void RefreshFrameProgress(int frame)
        {
            ReplayProgress.value = frame;
        }

        #endregion
    }
}