using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_MonsterGenerator
    // 몬스터 생성기
    // 웨이브 기반 생성 V
    // 어느 정도의 무작위성, 특정 시간대에 특정 패턴 몬스터 등장 V
    // 화면 모든 방향에서 등장, V
    // 게임이 진행될수록 강력한 몬스터 등장 V
    // 게임이 진행될수록 몬스터의 종류나 출현 방식 변화 V
    // 특정 시간, 조건에 엘리트나 보스 등장 V
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class DCL_MonsterGenerator : MonoBehaviour
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
            public int MonsterCount;            // 생성 몬스터 개수
            public List<GameObject> MonsterList;
            public Transform[] GeneratePointArr;
        }
        public NSetting Setting = new NSetting();
        #endregion

        #region [NestedClass] Generator Data
        public class NGenData
        {
            public float DelayMinTime;         // 몬스터 생성 딜레이 지정타임
            public float DelayMaxTime;         // 몬스터 생성 딜레이 지정타임
            public float DelayTime;         // 몬스터 생성 딜레이 지정타임
            public float LoopTime;          // 몬스터 생성 딜레이 루프타임
            public int GenerateCount;       // 한번에 생성할 몬스터 개수
            public int DoubleSpeed;         // 배속
            /////////////////////////////////////////////////
            public int MonsterMaxCount;
            public int MonsterMinCount;
            public int MonsterTypeCount;
            public int MonsterLevel;
        }
        public NGenData GenData = new NGenData();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public List<DCL_MonsterBase> MonsterPool = new List<DCL_MonsterBase>();
        //int[] PosArr = { 30, -30 };

        private DCL_StageBase StageBaseScript;
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
        void Start()
        {
            Setting.MonsterCount = 100;
            GenData.DelayTime = 0;          // 첫 딜레이 타임은 2초
            GenData.DoubleSpeed = 1;        // 배속

            StageBaseScript = gameObject.GetComponentInParent<DCL_StageBase>();
            StageBaseScript.SetCallback(Gen_Data_Setting);

            // 젠 데이터 초기화
            Gen_Data_Setting();
        }
        #endregion

        #region [Init] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        void Update()
        {
            GenData.LoopTime += Time.deltaTime * GenData.DoubleSpeed;
            if (GenData.LoopTime >= GenData.DelayTime)
                GenerateMonster();
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. GenerateMonster
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [GenerateMonster]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void GenerateMonster()
        {
            GenData.LoopTime = 0;       // 루프타임 초기화
            GenData.GenerateCount = UnityEngine.Random.Range(GenData.MonsterMinCount, GenData.MonsterMaxCount);
            GenData.DelayTime = UnityEngine.Random.Range(GenData.DelayMinTime, GenData.DelayMaxTime);
            int rndmontype = UnityEngine.Random.Range(0, GenData.MonsterTypeCount);
            for (int i = 0; i < GenData.GenerateCount; i++)
            {
                GameObject InstantMon = Instantiate(Setting.MonsterList[rndmontype], Setting.GeneratePointArr[i]);
                InstantMon.GetComponent<DCL_MonsterBase>().SetMonsterLevel(GenData.MonsterLevel);
            }

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Data Setting
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Data_Setting]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Gen_Data_Setting()
        {
            // 0 : CountUp, 1 : StatUp, 2: TypeUp
            var root = gameObject.GetComponentInParent<DCL_StageBase>();
            int type = root.NowWaveType;
            GenData.MonsterMaxCount = root.WaveMonMaxCount;
            GenData.MonsterMinCount = root.WaveMonMinCount;
            GenData.MonsterTypeCount = root.WaveMonTypeCnt;
            GenData.MonsterLevel = root.WaveMonLevel;
            GenData.DelayMaxTime = root.WaveMaxGenDelay;
            GenData.DelayMinTime = root.WaveMinGenDelay;
        }
        #endregion

    }

    #region [BackUp]
    //int tmp = UnityEngine.Random.Range(0, 1);
    //int ranpos = UnityEngine.Random.Range(0, 3);
    //if (ranpos == 0)
    //	InstantMon.transform.position = new Vector3(30, 0, UnityEngine.Random.Range(-30, 30));
    //else if (ranpos == 1)
    //	InstantMon.transform.position = new Vector3(-30, 0, UnityEngine.Random.Range(-30, 30));
    //else if (ranpos == 2)
    //	InstantMon.transform.position = new Vector3(UnityEngine.Random.Range(-30, 30), 0, 30);
    //else if (ranpos == 3)
    //	InstantMon.transform.position = new Vector3(UnityEngine.Random.Range(-30, 30), 0, -30);
    //MonsterPool.Add(InstantMon.GetComponent<DCL_MonsterBase>());
    #endregion

}
