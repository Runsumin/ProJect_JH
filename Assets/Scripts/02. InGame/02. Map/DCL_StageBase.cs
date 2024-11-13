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
        #region [Enum]
        public enum eStageDifficulty { EASY, NORMAL, HARD, HELL }   // 스테이지 난이도
        public enum eEventType { BIGWAVE, ELITE, MINIGAME, BOSS, NONE }   // 분기점(이벤트 종류)
        public enum eSubQuestType { PRISION, MARATHON, CLEANING, NONE }   // 서브퀘스트(미니게임) 종류
        public enum eWaveType { CountUp, StatUp, TypeUp, NONE }           // 웨이브 종류
        #endregion

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
            public eEventType EventType;
            public eSubQuestType SubQuestType;
            public int NextWaveTime;
            public int MonsterLevel;
            public int MonsterMinCount;
            public int MonsterMaxCount;
            public int GenDelayMaxCount;
            public int GenDelayMinCount;
            public int MonsterTypeCnt;
        }
        #endregion

        #region [Nestedclass] MonsterWave-Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public class NMonWaveSetting
        {
            public int MonsterWaveCount;
        }
        #endregion

        #region [Nestedclass] MiniGame
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NMiniGameSetting
        {
            public GameObject Root;
            public GameObject PrisionPrefab;
            public DCL_MiniGame_Base nowData;
            ///
            public GameObject MiniGame_Item;
            public GameObject[] MiniGame_Arr;
        }
        public NMiniGameSetting MiniGameSet = new NMiniGameSetting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        private int MonsterWaveIndex;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // CallBack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [CallBack_WaveChange]
        public delegate void WaveChangeCallBack();
        private WaveChangeCallBack WC_CallBack = null;
        #endregion

        #region [CallBack_MiniGame]
        public delegate void Create_MiniGame_CallBack();
        private Create_MiniGame_CallBack CM_CallBack = null;
        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public int NowWaveType => ((int)Setting.NowMonsterWaveType);
        public int NowWaveIndex => MonsterWaveIndex;
        public int WaveMonMaxCount => Setting.WaveArr[MonsterWaveIndex].MonsterMaxCount;
        public int WaveMonMinCount => Setting.WaveArr[MonsterWaveIndex].MonsterMinCount;
        public int WaveMonTypeCnt => Setting.WaveArr[MonsterWaveIndex].MonsterTypeCnt;
        public int WaveMonLevel => Setting.WaveArr[MonsterWaveIndex].MonsterLevel;
        public int WaveMaxGenDelay => Setting.WaveArr[MonsterWaveIndex].GenDelayMaxCount;
        public int WaveMinGenDelay => Setting.WaveArr[MonsterWaveIndex].GenDelayMinCount;
        public float NowInGameTime => Setting.StageTime.StageStreamNowTime;
        #endregion




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Awake
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Awake()
        {
            SetWaveData();
        }
        #endregion

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

        #region [Init] SetStageData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetWaveData()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    NMonsterWave data = new NMonsterWave();
            //    data.WaveType = Random_WaveType(i);
            //    data.NextWaveTime = UnityEngine.Random.Range(30 * i, 30 + (30 * i));
            //    data.MonsterMinCount = i;
            //    data.MonsterMaxCount = i;
            //    data.MonsterLevel = i;
            //    data.MonsterTypeCnt = i;
            //    data.GenDelayMaxCount = 4;
            //    data.GenDelayMinCount = 6;
            //    Setting.WaveArr.Add(data);
            //}

            //Json_Utility_Extend.FileSaveList(Setting.WaveArr, "Data/Json_Data/Monster/MonsterWave/MonsterWave.Json");
            switch (Setting.NowStageDifficulty)
            {
                case eStageDifficulty.EASY:
                    Setting.WaveArr = Json_Utility_Extend.FileLoadList<NMonsterWave>("Data/Json_Data/Monster/MonsterWave/MonsterWave_Easy.Json");
                    break;
                case eStageDifficulty.NORMAL:
                    Setting.WaveArr = Json_Utility_Extend.FileLoadList<NMonsterWave>("Data/Json_Data/Monster/MonsterWave/MonsterWave_Normal.Json");
                    break;
                case eStageDifficulty.HARD:
                    Setting.WaveArr = Json_Utility_Extend.FileLoadList<NMonsterWave>("Data/Json_Data/Monster/MonsterWave/MonsterWave_Hard.Json");
                    break;
                case eStageDifficulty.HELL:
                    Setting.WaveArr = Json_Utility_Extend.FileLoadList<NMonsterWave>("Data/Json_Data/Monster/MonsterWave/MonsterWave_Hell.Json");
                    break;
            }
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
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void WaveTimeSetting(float time)
        {
            if ((int)time == Setting.WaveArr[MonsterWaveIndex].NextWaveTime)
            {
                MonsterWaveIndex++;
                Setting.NowMonsterWaveType = Setting.WaveArr[MonsterWaveIndex].WaveType;
                WC_CallBack();
            }
        }
        #endregion


        public void SetCallback(WaveChangeCallBack cal)
        {
            WC_CallBack = cal;
        }

        public void SetMiniGameCallback(Create_MiniGame_CallBack cal)
        {
            CM_CallBack = cal;
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Random Data
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [RandomData] Random WaveType
        //------------------------------------------------------------------------------------------------------------------------------------------------------
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

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Generate
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Generate] MiniGame Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Create_MiniGame(Vector3 pos)
        {
            GameObject minigame = Instantiate(MiniGameSet.PrisionPrefab, MiniGameSet.Root.transform);
            minigame.GetComponent<DCL_MiniGame_Base>().Initialize(Setting.StageTime.StageStreamNowTime, 25, pos);
            MiniGameSet.nowData = minigame.GetComponent<DCL_MiniGame_Base>();

            CM_CallBack();
        }
        #endregion

        #region [Generate] MiniGame Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Create_Boss(Vector3 pos)
        {
            //GameObject boss = Instantiate(MiniGameSet.PrisionPrefab, MiniGameSet.Root.transform);            
            //boss.GetComponent<DCL_MiniGame_Prison>().Initialize(Setting.StageTime.StageStreamNowTime, 30, pos);            

        }
        #endregion


    }

}
