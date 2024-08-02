using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_StageBase
    // ���� ����� ���������� ���� ���� ���� Ŭ����
    //
    //	��������Ʈ
    //  - ���� ����Ʈ ���� �ð�
    //	- �б��� ���� �ð�
    //	- �б��� ����
    //	- 
    //	��������Ʈ
    //  - ���� ����Ʈ ����
    //  - ���� ����Ʈ �߻� �ð�
    // 
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class DCL_StageBase : MonoBehaviour
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum eStageDifficulty { EASY, NORMAL, HARD, HELL }   // �������� ���̵�
        public enum eEventType { BIGWAVE, ELITE, MINIGAME, BOSS }   // �б���(�̺�Ʈ ����)
        public enum eSubQuestType { MARATHON, CLEANING }            // ��������Ʈ ����

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NSetting
        {
            // �� ����
            public int NowStageIndex;                       // ������ �� �ε���
            public int NowLevelIndex;                       // ������ ���� �ε���
            public eStageDifficulty NowStageDifficulty;

            public NStageStreamtime StageTime;
            public NStageSubQuest SubQuest;

        }
        public NSetting Setting = new NSetting();
        #endregion

        #region [NestedClass] StageTime
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NStageStreamtime
        {
            public float StageStreamToTalTime;  // ���� : sec
            public float StageStreamNowTime;    // ���� : sec
        }
        #endregion


        #region [NestedClass] SubQuest
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NStageSubQuest
        {
            public int EventCount;
            public float[] EventTime;
        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base

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

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Start()
        {
            if (SceneManager.Instance != null)
            {
                Setting.NowLevelIndex = SceneManager.Instance.NowLevelIndex;
                Setting.NowStageIndex = SceneManager.Instance.NowStageIndex;
            }

            SetStageData();
        }
        #endregion

        #region [Init] SetStageData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetStageData()
        {
            switch (Setting.NowLevelIndex)
            {
                case 1:
                    Setting.NowStageDifficulty = eStageDifficulty.EASY;
                    break;
                case 2:
                    Setting.NowStageDifficulty = eStageDifficulty.NORMAL;
                    break;
                case 3:
                    Setting.NowStageDifficulty = eStageDifficulty.HARD;
                    break;
                case 4:
                    Setting.NowStageDifficulty = eStageDifficulty.HELL;
                    break;

            }

            // Time
            Setting.StageTime.StageStreamToTalTime = 600f;
            Setting.StageTime.StageStreamNowTime = 0;

            //
            if (Setting.NowStageDifficulty == eStageDifficulty.EASY ||
                Setting.NowStageDifficulty == eStageDifficulty.NORMAL)
                Setting.SubQuest.EventCount = 3;
            else
                Setting.SubQuest.EventCount = 4;
        }
        #endregion

        #region [Update]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            Setting.StageTime.StageStreamNowTime += Time.deltaTime;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Game Timer
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [GameTimer] InGameTime Setting
        public void InGameTimer(float time)
        {

        }
        #endregion

    }

}
