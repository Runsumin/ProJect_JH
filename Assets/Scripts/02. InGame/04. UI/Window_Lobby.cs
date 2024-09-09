using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        }
        public NSetting Setting = new NSetting();
        #endregion


        #region [Nested] Status_Upgrade
        public class Stat_Upgrade
        {
            public int Maxlv;
            public int Nowlv;
            //public int 
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
        }

        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Button
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

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
            if(stage > 1)
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
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Permanent Stat Up
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Status Up]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_AddStatus(int index)
        {

        }


        #endregion
    }
}
