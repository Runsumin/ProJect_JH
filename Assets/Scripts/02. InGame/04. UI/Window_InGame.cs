using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // Window_InGame
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class Window_InGame : WindowBase
    {
        public static Window_InGame Instance;
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NSetting
        {
            public GameObject WindowRoot;
        }
        public NSetting Setting = new NSetting();
        #endregion

        #region [Nested] Popup_Result
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NPopupResult : NPopUpBase
        {
            public GameObject Text_Clear;
            public GameObject Text_Fail;
        }
        public NPopupResult Popup_Result = new NPopupResult();
        #endregion


        #region [Nested] Popup_Pause
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NPopupPause : NPopUpBase
        {

        }
        public NPopupPause Popup_Pause = new NPopupPause();
        #endregion

        #region [Nested] InGame_Timer
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NInGame_Timer : NPopUpBase
        {
            public TextMeshProUGUI MainTimer;
        }
        public NInGame_Timer InGame_Timer = new NInGame_Timer();
        #endregion

        #region [Nested] InGame_Choice
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NInGame_Choice : NPopUpBase
        {
            public TextMeshProUGUI Timer;
            public TextMeshProUGUI[] ExPlanationTextArr;
            public Image[] ChoiceIconArr;
        }
        public NInGame_Choice InGame_Choice = new NInGame_Choice();
        #endregion

        #region [Nested] NDebug
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NDebug : NPopUpBase
        {
            public TextMeshProUGUI HP;
            public TextMeshProUGUI EXP;
            public TextMeshProUGUI LEVEL;
        }
        public NDebug InGame_Debug = new NDebug();
        #endregion

        #region [Nested] MiniGame
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NMiniGame : NPopUpBase
        {
            public TextMeshProUGUI ExplainText;
            public TextMeshProUGUI Timer;
            public TextMeshProUGUI Result;
            public TextMeshProUGUI QuestTable;
            public bool MiniGameStart;

        }
        public NMiniGame InGame_MiniGame = new NMiniGame();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Choice]
        private DCL_Status_Choice choice;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] 스테이지 데이터
        private DCL_StageBase StageData;
        #endregion

        #region [Property] 플레이어 정보
        private DCL_PlayerBase PlayerData;
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Awake()
        {
            base.Start();
            Instance = this;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            StageData = GameObject.Find("DCL_InGame").GetComponent<DCL_StageBase>();
            StageData.SetMiniGameCallback(Show_MiniGame);
            SetPlayerData();
            choice = new DCL_Status_Choice();
            choice.MakeChoiceData();
        }
        #endregion

        #region [Update]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            Update_MainTimer();
            Update_PlayerData();
            if (InGame_MiniGame.MiniGameStart)
            {
                MiniGame_TimeCheck();
            }
        }
        #endregion

        #region [Init] SetPlayerData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetPlayerData()
        {
            PlayerData = GameObject.FindWithTag("Player").GetComponent<DCL_PlayerBase>();
            PlayerData.LevelUpSetCallback(Show_ChoiceList);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. GamePlay
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Button] Pause
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_Pause()
        {
            Time.timeScale = 0f;
            Popup_Pause.Root.SetActive(true);
        }

        #endregion
        #region [Button] RePlay
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_Replay()
        {
            ObjectManager.Instance.ResetAllObject();
            if (Popup_Pause.Root.activeSelf == true) Popup_Pause.Root.SetActive(false);
            if (Popup_Result.Root.activeSelf == true)
            {
                Popup_Result.Root.SetActive(false);
                Popup_Result.Text_Clear.SetActive(false);
                Popup_Result.Text_Fail.SetActive(false);
            }
            Time.timeScale = 1f;
        }
        #endregion

        #region [Button] GoToMainMenu
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.Instance.LoadScene("DCL_Lobby");
            CloseWindow(true);
        }
        #endregion

        #region [Button] LevelSelect
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_LevelSelect()
        {
            Time.timeScale = 1f;
            SceneManager.Instance.LoadScene("DCL_Lobby");
            CloseWindow(true);
        }
        #endregion

        #region [Button] Continue
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_Continue()
        {
            Time.timeScale = 1f;
            Popup_Pause.Root.SetActive(false);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Result
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Result] ShowResult
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ShowResult(bool result)
        {

            Popup_Result.Root.SetActive(true);
            if (result)
            {
                Popup_Result.Text_Clear.SetActive(true);
            }
            else
            {
                Popup_Result.Text_Fail.SetActive(true);
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Timer
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Timer] Update Maintimer
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Update_MainTimer()
        {
            InGame_Timer.MainTimer.text = ((int)StageData.Setting.StageTime.StageStreamNowTime).ToString();
        }
        #endregion

        #region [Debug] Update PlayerData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Update_PlayerData()
        {
            InGame_Debug.HP.text = $" HP : {PlayerData.PL_Status.HP} / {PlayerData.Setting.NowHP}";
            InGame_Debug.EXP.text = $" EXP : {PlayerData.ToTalEXP} / {PlayerData.Setting.NowEXP}";
            InGame_Debug.LEVEL.text = $" LEVEL : {PlayerData.NowPlayerLevel + 1}";
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Choice Item & Status
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Choice Item & Status] Select
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_ChoiceStatus_Item(int select)
        {
            Time.timeScale = 1f;
            string value = choice.ChoiceData_val[select].AddStatSort;

            switch (value)
            {
                case "Attack_Power":
                    PlayerData.Setting.Pl_Status_InGame.Attack_Power += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "Attack_Speed":
                    PlayerData.Setting.Pl_Status_InGame.Attack_Speed += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "Cri_Percent":
                    PlayerData.Setting.Pl_Status_InGame.Cri_Percent += (int)choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "Critical_Damage":
                    PlayerData.Setting.Pl_Status_InGame.Critical_Damage += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "Move_Speed":
                    PlayerData.Setting.Pl_Status_InGame.Move_Speed += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "Defense":
                    PlayerData.Setting.Pl_Status_InGame.Defense += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "HP":
                    PlayerData.Setting.Pl_Status_InGame.HP += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "HP_Recovery":
                    PlayerData.Setting.Pl_Status_InGame.HP_Recovery += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "Cleaning_Speed":
                    PlayerData.Setting.Pl_Status_InGame.Cleaning_Speed += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "Gain_Range":
                    PlayerData.Setting.Pl_Status_InGame.Gain_Range += choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
                case "Weapon_LevelUp":
                    PlayerData.NowPlayerWeaponLevel += (int)choice.SetPlayerAddData(value, choice.ChoiceData_val[select].Grade);
                    break;
            }

            InGame_Choice.Root.SetActive(false);

        }
        #endregion

        #region [Choice Item & Status] ShowChoiceList
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Show_ChoiceList()
        {
            Time.timeScale = 0f;
            InGame_Choice.Root.SetActive(true);
            int[] data = choice.SetChoiceData();
            for (int i = 0; i < 3; i++)
            {
                InGame_Choice.ExPlanationTextArr[i].text = choice.ChoiceData_val[i].Explain;
                InGame_Choice.ChoiceIconArr[i].sprite = choice.ImageArr[data[i]];
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 4. MiniGame
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [MiniGame] Show_MiniGame
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Show_MiniGame()
        {
            InGame_MiniGame.Root.SetActive(true);
            InGame_MiniGame.ExplainText.text = StageData.MiniGameSet.nowData.Setting.Explanation;
            InGame_MiniGame.Timer.text = ((int)StageData.MiniGameSet.nowData.Setting.GameIngTime).ToString();
            InGame_MiniGame.MiniGameStart = true;
            switch (StageData.MiniGameSet.nowData.Setting.GameType)
            {
                case DCL_MiniGame_Base.MiniGameType.Flag:
                    InGame_MiniGame.QuestTable.gameObject.SetActive(true);
                    break;
                case DCL_MiniGame_Base.MiniGameType.Prison:
                    InGame_MiniGame.QuestTable.gameObject.SetActive(false);
                    break;
                case DCL_MiniGame_Base.MiniGameType.Cleaning:
                    InGame_MiniGame.QuestTable.gameObject.SetActive(true);
                    break;
            }
        }
        #endregion

        #region [MiniGame] Result_MiniGame
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Result_MiniGame(bool result)
        {
            InGame_MiniGame.ExplainText.gameObject.SetActive(false);
            InGame_MiniGame.Timer.gameObject.SetActive(false);
            InGame_MiniGame.Result.gameObject.SetActive(true);
            if (result)
                InGame_MiniGame.Result.text = "성공";
            else
                InGame_MiniGame.Result.text = "실패";

            StartCoroutine(Hide_MiniGame());
        }
        #endregion


        #region [MiniGame] Hide_MiniGame
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerator Hide_MiniGame()
        {
            float time = 0;
            while (true)
            {
                time += Time.deltaTime;

                if (time > 2)
                {
                    InGame_MiniGame.Root.SetActive(false);
                    InGame_MiniGame.ExplainText.gameObject.SetActive(true);
                    InGame_MiniGame.Timer.gameObject.SetActive(true);
                    InGame_MiniGame.Result.gameObject.SetActive(false);
                    StageData.MiniGameSet.nowData.Destroy();
                    break;
                }
                yield return null;
            }
        }
        #endregion

        #region [MiniGame] MiniGame_TimeCheck
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void MiniGame_TimeCheck()
        {
            InGame_MiniGame.Timer.text = ((int)StageData.MiniGameSet.nowData.Setting.GameIngTime).ToString();
            switch (StageData.MiniGameSet.nowData.Setting.GameType)
            {
                case DCL_MiniGame_Base.MiniGameType.Flag:
                    int all = StageData.MiniGameSet.nowData.GetComponent<DCL_MiniGame_Flag>().Item_MaxCount;
                    int now = StageData.MiniGameSet.nowData.GetComponent<DCL_MiniGame_Flag>().Item_NowCount;
                    InGame_MiniGame.QuestTable.text = string.Format("{0} / {1}", now, all);
                    break;
                case DCL_MiniGame_Base.MiniGameType.Prison:
                    break;
                case DCL_MiniGame_Base.MiniGameType.Cleaning:
                    int all2 = StageData.MiniGameSet.nowData.GetComponent<DCL_MiniGame_Cleaning>().Item_MaxCount;
                    int now2 = StageData.MiniGameSet.nowData.GetComponent<DCL_MiniGame_Cleaning>().Item_NowCount;
                    InGame_MiniGame.QuestTable.text = string.Format("{0} / {1}", now2, all2);
                    break;
            }
            // GameEndCheck
            if (StageData.MiniGameSet.nowData.Setting.MiniGameEnd == true &&
                StageData.MiniGameSet.nowData.Setting.MiniGameClear == false)
            {
                // 미니게임 실패
                Result_MiniGame(false);
                InGame_MiniGame.MiniGameStart = false;
            }
            else if (StageData.MiniGameSet.nowData.Setting.MiniGameEnd == true &&
                   StageData.MiniGameSet.nowData.Setting.MiniGameClear == true)
            {
                // 미니게임 성공
                Result_MiniGame(true);
                InGame_MiniGame.MiniGameStart = false;
            }
        }
        #endregion

    }
}