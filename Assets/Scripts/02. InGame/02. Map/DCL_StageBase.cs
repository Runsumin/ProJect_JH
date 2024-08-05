using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_StageBase
    // 현재 진행될 스테이지에 관한 정보 관리 클래스
    //
    //	메인퀘스트
    //  - 메인 퀘스트 진행 시간
    //	- 분기점 생성 시간
    //	- 분기점 개수
    //	- 
    //	서브퀘스트
    //  - 서브 퀘스트 종류
    //  - 서브 퀘스트 발생 시간
    // 
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class DCL_StageBase : MonoBehaviour
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum eStageDifficulty { EASY, NORMAL, HARD, HELL }   // 스테이지 난이도
        public enum eEventType { BIGWAVE, ELITE, MINIGAME, BOSS }   // 분기점(이벤트 종류)
        public enum eSubQuestType { MARATHON, CLEANING }            // 서브퀘스트 종류

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NSetting
        {
            // 맵 정보
            public int NowStageIndex;                       // 진행할 맵 인덱스
            public int NowLevelIndex;                       // 진행할 레벨 인덱스
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
            public float StageStreamToTalTime;  // 단위 : sec
            public float StageStreamNowTime;    // 단위 : sec
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
