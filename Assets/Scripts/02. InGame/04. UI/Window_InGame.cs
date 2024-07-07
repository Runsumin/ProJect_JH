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

        #region [Nested] InGame_Trash
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //[Serializable]
        public class NInGame_Trash : NPopUpBase
        {
            public GameObject TrashIcon;
        }
        public NInGame_Trash InGame_Trash = new NInGame_Trash();
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

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Item_Trash] Icon
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public List<NInGame_Trash> InGame_TrashList = new List<NInGame_Trash>();
        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

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

        public override void Start()
        {
            base.Start();
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

        #region [Button] ShowResult
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
        // 2. Item_Trash
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Item_Trash] Generate Icon
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void GenerateIcon()
        {

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Choice Item & Status
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Button] Select
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnClick_ChoiceStatus_Item()
        {
            Time.timeScale = 1f;
            InGame_Choice.Root.SetActive(false);
        }
        #endregion

        #region [Choice] ShowChoiceList
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Show_ChoiceList()
        {
            Time.timeScale = 0f;
            InGame_Choice.Root.SetActive(true);
        }
        #endregion

    }
}