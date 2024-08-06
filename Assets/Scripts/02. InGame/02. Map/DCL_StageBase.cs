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
    //  이벤트 - 게임 내 큰 분기점
    //  웨이브 - 게임 내 몬스터 생성 패턴
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
        public enum eWaveType { CountUp, StatUp, TypeUp }           // 웨이브 종류

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
            public List<NMonsterWave> WaveArr = new List<NMonsterWave>();
            public eWaveType NowMonsterWaveType;

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
            public int[] EventTime;
        }
        #endregion

        #region [NestedClass] Monster Wave
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NMonsterWave
        {
            public eWaveType WaveType;
            public int WaveTime;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        private int MonsterWaveIndex;
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public int NowWaveType => ((int)Setting.NowMonsterWaveType);
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

            //Setting.NowMonsterWave.WaveTime = 0;
            //Setting.NowMonsterWave.WaveType = eWaveType.CountUp;
            SetStageData();
            SetWaveData();
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

        #region [Init] SetStageData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetWaveData()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    NMonsterWave data = new NMonsterWave();
            //    data.WaveType = Random_WaveType(i);
            //    data.WaveTime = UnityEngine.Random.Range(30 * i, 30 + (30 * i));
            //    Setting.WaveArr.Add(data);
            //}

            //Json_Utility_Extend.FileSaveList(Setting.WaveArr, "Data/Json_Data/Monster/MonsterWave/MonsterWave.Json");
            Setting.WaveArr = Json_Utility_Extend.FileLoadList<NMonsterWave>("Data/Json_Data/Monster/MonsterWave/MonsterWave.Json");
        }
        #endregion

        #region [Update]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            Setting.StageTime.StageStreamNowTime += Time.deltaTime;
            WaveTimeSetting(Setting.StageTime.StageStreamNowTime);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Game Timer
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [GameTimer] WaveTimeSetting
        public void WaveTimeSetting(float time)
        {
            if ((int)time == Setting.WaveArr[MonsterWaveIndex].WaveTime)
            {
                Setting.NowMonsterWaveType = Setting.WaveArr[MonsterWaveIndex].WaveType;
                MonsterWaveIndex++;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Random Data
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [RandomData] Random WaveType
        public eWaveType Random_WaveType(int rof)
        {
            eWaveType data = eWaveType.CountUp;
            //int rnd = UnityEngine.Random.Range(0, 3);
            int rnd = rof % 3;
            switch (rnd)
            {
                case 0:
                    data = eWaveType.CountUp;
                    break;
                case 1:
                    data = eWaveType.StatUp;
                    break;
                case 2:
                    data = eWaveType.TypeUp;
                    break;
            }

            return data;
        }
        #endregion
    }

}
