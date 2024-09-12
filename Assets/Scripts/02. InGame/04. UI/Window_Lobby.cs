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
    // Window_Lobby
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class Window_Lobby : WindowBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum eStatusType { Att_Power, Att_Speed, Cri_Percent, Cri_Damage, Move_Speed, Defense, HP, HP_Recovery, Cln_Speed, Gain_Range }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NSetting
        {
            public GameObject MainTitleRoot;
            public GameObject StageSelectRoot;
            public GameObject LevelSelectRoot;
            public GameObject Notification_Window;
            public GameObject UpgradeWindow;
            public Stat_Upgrade[] UpgradeDataShow;
            public TextMeshProUGUI NowDevilCoinCount;
            public TextMeshProUGUI NowNotifyMessage;
        }
        public NSetting Setting = new NSetting();
        #endregion


        #region [Nested] Status_Upgrade
        [Serializable]
        public class Stat_Upgrade
        {
            public TextMeshProUGUI Level;
            public TextMeshProUGUI Payment;
        }
        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        private DCL_Status_Permanent Player_PermanentStatus;
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Awake()
        {
            //base.Start();
        }

        public override void Start()
        {
            base.Start();
            Player_PermanentStatus = new DCL_Status_Permanent();
            Player_PermanentStatus.Initialize_Status_Permanent();

            StatUP_Setting();
        }

        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Button
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Button] MainTitle
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_MainTitle()
        {
            Setting.MainTitleRoot.SetActive(true);
            Setting.UpgradeWindow.SetActive(false);
        }
        #endregion
        #region [Button] GameStart
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_GameStart()
        {
            Setting.MainTitleRoot.SetActive(false);
            Setting.StageSelectRoot.SetActive(true);
        }
        #endregion

        #region [Button] Upgrade
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_Upgrade()
        {
            Setting.MainTitleRoot.SetActive(false);
            Setting.UpgradeWindow.SetActive(true);

        }
        #endregion

        #region [Button] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_Setting()
        {

        }
        #endregion

        #region [Button] GameEnd
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_GameEnd()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        #endregion

        #region [Button] Notofication_Window
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_Close_NotifyWin()
        {
            Setting.Notification_Window.SetActive(false);
        }
        #endregion

        #region [Button] StageSelect
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_StageSelect(int stage)
        {
            if (stage > 1)
                Open_NotifyWindow();
            else
            {
                Setting.StageSelectRoot.SetActive(false);
                Setting.LevelSelectRoot.SetActive(true);
                SceneManager.Instance.NowStageIndex = stage;
            }
        }
        #endregion

        #region [Button] StageSelect
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_LevelSelect(int Level)
        {
            if (Level > 1)
                Open_NotifyWindow();
            else
            {
                // 스테이지, 레벨 정보에 따라서 난이도 밸런스 조절.
                Setting.MainTitleRoot.SetActive(true);
                Setting.StageSelectRoot.SetActive(false);
                SceneManager.Instance.LoadScene("DCL_InGame");
                SceneManager.Instance.NowLevelIndex = Level;
                CloseWindow(true);
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Window
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Window] OpenWindow
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Open_NotifyWindow()
        {
            Setting.Notification_Window.SetActive(true);
            Setting.NowNotifyMessage.text = "준비중입니다.";
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Permanent Stat Up
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Status Up]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void StatUP_Setting()
        {
            // 순서 중요

            for (int i = 0; i < 10; i++)
            {
                Setting.UpgradeDataShow[i].Level.text = Player_PermanentStatus.Status_listSet[i][Player_PermanentStatus.Status_NowLevel[i]].Level.ToString();
                Setting.UpgradeDataShow[i].Payment.text = Player_PermanentStatus.Status_listSet[i][Player_PermanentStatus.Status_NowLevel[i]].Expense.ToString();
            }

            Setting.NowDevilCoinCount.text = Player_PermanentStatus.Player_PermanentData.DevilCoinCount.ToString();
            //// Att_Power
            //Setting.UpgradeDataShow[0].Level.text = Player_PermanentStatus.Now_Att_Power_Data.Level.ToString();
            //Setting.UpgradeDataShow[0].Payment.text = Player_PermanentStatus.Now_Att_Power_Data.Expense.ToString();

            //// Att_Speed
            //Setting.UpgradeDataShow[1].Level.text = Player_PermanentStatus.Now_Att_Speed_Data.Level.ToString();
            //Setting.UpgradeDataShow[1].Payment.text = Player_PermanentStatus.Now_Att_Speed_Data.Expense.ToString();

            //// Cri_Percent
            //Setting.UpgradeDataShow[2].Level.text = Player_PermanentStatus.Now_Cri_Percent_Data.Level.ToString();
            //Setting.UpgradeDataShow[2].Payment.text = Player_PermanentStatus.Now_Cri_Percent_Data.Expense.ToString();

            //// Cri_Damage
            //Setting.UpgradeDataShow[3].Level.text = Player_PermanentStatus.Now_Cri_Damage_Data.Level.ToString();
            //Setting.UpgradeDataShow[3].Payment.text = Player_PermanentStatus.Now_Cri_Damage_Data.Expense.ToString();

            //// Move_Speed
            //Setting.UpgradeDataShow[4].Level.text = Player_PermanentStatus.Now_Move_Speed_Data.Level.ToString();
            //Setting.UpgradeDataShow[4].Payment.text = Player_PermanentStatus.Now_Move_Speed_Data.Expense.ToString();

            //// Defense
            //Setting.UpgradeDataShow[5].Level.text = Player_PermanentStatus.Now_Defense_Data.Level.ToString();
            //Setting.UpgradeDataShow[5].Payment.text = Player_PermanentStatus.Now_Defense_Data.Expense.ToString();

            //// HP
            //Setting.UpgradeDataShow[6].Level.text = Player_PermanentStatus.Now_HP_Data.Level.ToString();
            //Setting.UpgradeDataShow[6].Payment.text = Player_PermanentStatus.Now_HP_Data.Expense.ToString();

            //// HP_Recovery
            //Setting.UpgradeDataShow[7].Level.text = Player_PermanentStatus.Now_HP_Recovery_Data.Level.ToString();
            //Setting.UpgradeDataShow[7].Payment.text = Player_PermanentStatus.Now_HP_Recovery_Data.Expense.ToString();

            //// Cln_Speed
            //Setting.UpgradeDataShow[8].Level.text = Player_PermanentStatus.Now_Cln_Speed_Data.Level.ToString();
            //Setting.UpgradeDataShow[8].Payment.text = Player_PermanentStatus.Now_Cln_Speed_Data.Expense.ToString();

            //// Gain_Range
            //Setting.UpgradeDataShow[9].Level.text = Player_PermanentStatus.Now_Gain_Range_Data.Level.ToString();
            //Setting.UpgradeDataShow[9].Payment.text = Player_PermanentStatus.Now_Gain_Range_Data.Expense.ToString();
        }
        #endregion

        #region [Status Up]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_AddStatus(int type)
        {
            eStatusType data = (eStatusType)type;

            if (Player_PermanentStatus.Status_listSet[type].Count > Player_PermanentStatus.Status_listSet[type][Player_PermanentStatus.Status_NowLevel[type]].Level &&
                Player_PermanentStatus.Player_PermanentData.DevilCoinCount >
                Player_PermanentStatus.Status_listSet[type][Player_PermanentStatus.Status_NowLevel[type]].Expense)
            {

                Player_PermanentStatus.Status_NowLevel[type]++;
                switch (data)
                {
                    case eStatusType.Att_Power:
                        Player_PermanentStatus.Stat_Per_List.Att_Power_Now_Level++;
                        break;
                    case eStatusType.Att_Speed:
                        Player_PermanentStatus.Stat_Per_List.Att_Speed_Now_Level++;
                        break;
                    case eStatusType.Cri_Percent:
                        Player_PermanentStatus.Stat_Per_List.Cri_Percent_Now_Level++;
                        break;
                    case eStatusType.Cri_Damage:
                        Player_PermanentStatus.Stat_Per_List.Cri_Damage_Now_Level++;
                        break;
                    case eStatusType.Move_Speed:
                        Player_PermanentStatus.Stat_Per_List.Move_Speed_Now_Level++;
                        break;
                    case eStatusType.Defense:
                        Player_PermanentStatus.Stat_Per_List.Defense_Now_Level++;
                        break;
                    case eStatusType.HP:
                        Player_PermanentStatus.Stat_Per_List.HP_Now_Level++;
                        break;
                    case eStatusType.HP_Recovery:
                        Player_PermanentStatus.Stat_Per_List.HP_Recovery_Now_Level++;
                        break;
                    case eStatusType.Cln_Speed:
                        Player_PermanentStatus.Stat_Per_List.Cln_Speed_Now_Level++;
                        break;
                    case eStatusType.Gain_Range:
                        Player_PermanentStatus.Stat_Per_List.Gain_Range_Now_Level++;
                        break;
                }

                Player_PermanentStatus.Player_PermanentData.DevilCoinCount -= Player_PermanentStatus.Status_listSet[type][Player_PermanentStatus.Status_NowLevel[type]].Expense;

                Player_PermanentStatus.Save_PermanentData();
                StatUP_Setting();

            }
            else
            {
                if (Player_PermanentStatus.Status_listSet[type].Count < type)
                {
                    Setting.Notification_Window.SetActive(true);
                    Setting.NowNotifyMessage.text = "더이상 강화할 수 없습니다.";
                }
                else if (Player_PermanentStatus.Player_PermanentData.DevilCoinCount <
                Player_PermanentStatus.Status_listSet[type][Player_PermanentStatus.Status_NowLevel[type]].Expense)
                {
                    Setting.Notification_Window.SetActive(true);
                    Setting.NowNotifyMessage.text = "마왕코인이 부족합니다.";
                }
            }
        }
        #endregion
    }
}
