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
using TrueSync;
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
        private int m_ReplaySpeedStep = 1;
        private int m_ReplaySpeed = 1;
        
        private Button BtnStartGame;
        private Button BtnStopGame;
        private Button BtnSkill_1;
        private Button BtnSkill_2;
        private Button BtnSkill_3;
        private Button BtnSkill_4;

        public GameObject instantiateCube;
        private GameObject m_Cube;

        private LockStepCommandModuleDef m_commandModuleDef = LockStepCommandModuleDef.Demo_One;

        private DemoOneModuleSystem m_ModuleSystem;
        private StandAloneLSSystem m_StandAloneLsSystem;
        
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
            BtnSkill_1 = transform.Find("BtnSkill_1").GetComponent<Button>();
            BtnSkill_2 = transform.Find("BtnSkill_2").GetComponent<Button>();
            BtnSkill_3 = transform.Find("BtnSkill_3").GetComponent<Button>();
            BtnSkill_4 = transform.Find("BtnSkill_4").GetComponent<Button>();
            BtnStartGame.onClick.AddListener(OnBtnStartGame);
            BtnStopGame.onClick.AddListener(OnBtnStopGame);
            BtnSkill_1.onClick.AddListener(()=>AddInputCommand(Demo_OneCommandDef.Skill_1));
            BtnSkill_2.onClick.AddListener(()=>AddInputCommand(Demo_OneCommandDef.Skill_2));
            BtnSkill_3.onClick.AddListener(()=>AddInputCommand(Demo_OneCommandDef.Skill_3));
            BtnSkill_4.onClick.AddListener(()=>AddInputCommand(Demo_OneCommandDef.Skill_4));

            m_StandAloneLsSystem = LockStepModuleSingletom.instance.GetModule<StandAloneLSSystem>();
            m_ModuleSystem = m_StandAloneLsSystem.GetModule<DemoOneModuleSystem>();

            // LockStepModuleSingletom.instance.TryGetCommandModule(LockStepCommandModuleDef.Demo_One, out lockStepSystem);
            RefreshBtnRecord();
            EventBusSingleton.instance.RegisterEvent<int>(EventBusSingletonDefine.FrameTick,RefreshFrameProgress);
        }

        public void OnBtnStartGame()
        {
            Debug.Log("StartGame");
            StartGame();
            m_ModuleSystem.StartTick(TimeInfo.instance.ClientNow(),0);
        }

        public void OnBtnStopGame()
        {
            Debug.Log("StopGame");
            m_ModuleSystem.StopTick();
        }

        #region Replay

        private void OnBtnRecord()
        {
            m_StandAloneLsSystem.IsSaveReplay = !m_StandAloneLsSystem.IsSaveReplay;
            RefreshBtnRecord();
        }

        private void OnBtnReplay()
        {
            StartGame();
            m_ModuleSystem.Replay();
            ReplayProgress.minValue = 0;
            ReplayProgress.maxValue = m_StandAloneLsSystem.kLockStepReplay.FrameInputs.Count;
            ReplayProgress.value = 0;
            RefreshReplaySpeed();
            GameDatas.instance.GetData<Demo_OneGameData>().Clear();
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
            m_StandAloneLsSystem.JumpToReplayFrame(targetFrame);
        }

        private void RefreshReplaySpeed()
        {
            m_StandAloneLsSystem.replayUpdateSystem.ChangeReplaySpeed(m_ReplaySpeed);
            TxtReplaySpeed.text = "Speed:" + m_ReplaySpeed;
        }

        private void RefreshBtnRecord()
        {
            TxtBtnRecord.text = m_StandAloneLsSystem.IsSaveReplay ? "StopRecord" : "StartRecord";
        }

        private void RefreshFrameProgress(int frame)
        {
            ReplayProgress.value = frame;
        }

        #endregion

        #region Cube

        public void StartGame()
        {
            if (m_Cube!=null)  
            {
                GameObject.Destroy(m_Cube);
                m_Cube = null;
            }
            m_Cube = Instantiate(instantiateCube);
            GameDatas.instance.GetData<Demo_OneGameData>().Clear();
        }

        #endregion

        private void AddInputCommand(Demo_OneCommandDef demoOneCommandDef)
        {
            LockStepInput lockStepInput = new LockStepInput(m_commandModuleDef, (int)demoOneCommandDef);
            m_ModuleSystem.AddFrameInput(lockStepInput);
        }
        
    }
}