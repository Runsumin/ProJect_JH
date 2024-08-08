using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_MonsterGenerator
    // ���� ������
    // ���̺� ��� ���� V
    // ��� ������ ��������, Ư�� �ð��뿡 Ư�� ���� ���� ���� V
    // ȭ�� ��� ���⿡�� ����, V
    // ������ ����ɼ��� ������ ���� ���� V
    // ������ ����ɼ��� ������ ������ ���� ��� ��ȭ V
    // Ư�� �ð�, ���ǿ� ����Ʈ�� ���� ���� V
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
            public int MonsterCount;            // ���� ���� ����
            public List<GameObject> MonsterList;
            public Transform[] GeneratePointArr;
        }
        public NSetting Setting = new NSetting();
        #endregion

        #region [NestedClass] Generator Data
        public class NGenData
        {
            public float DelayMinTime;         // ���� ���� ������ ����Ÿ��
            public float DelayMaxTime;         // ���� ���� ������ ����Ÿ��
            public float DelayTime;         // ���� ���� ������ ����Ÿ��
            public float LoopTime;          // ���� ���� ������ ����Ÿ��
            public int GenerateCount;       // �ѹ��� ������ ���� ����
            public int DoubleSpeed;         // ���
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
            GenData.DelayTime = 0;          // ù ������ Ÿ���� 2��
            GenData.DoubleSpeed = 1;        // ���

            StageBaseScript = gameObject.GetComponentInParent<DCL_StageBase>();
            StageBaseScript.SetCallback(Gen_Data_Setting);

            // �� ������ �ʱ�ȭ
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
            GenData.LoopTime = 0;       // ����Ÿ�� �ʱ�ȭ
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
