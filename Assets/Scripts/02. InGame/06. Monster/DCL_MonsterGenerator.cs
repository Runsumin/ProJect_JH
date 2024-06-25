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
		}
		public NSetting Setting = new NSetting();
		#endregion



		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		// Variable
		//
		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		#region [Variable] Base
		public List<DCL_MonsterBase> MonsterPool = new List<DCL_MonsterBase>();
		public float DelayTime;			// ���� ���� ������ ����Ÿ��
		public float LoopTime;			// ���� ���� ������ ����Ÿ��
		public int GenerateCount;		// �ѹ��� ������ ���� ����
		public int DoubleSpeed;         // ���
		int[] PosArr = { 30, -30 };
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
			DelayTime = 0;          // ù ������ Ÿ���� 2��
			DoubleSpeed = 1;		// ���
		}
		#endregion

		#region [Init] Update
		//------------------------------------------------------------------------------------------------------------------------------------------------------
		void Update()
		{
			LoopTime += Time.deltaTime * DoubleSpeed;
			if(LoopTime >= DelayTime)
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
			LoopTime = 0;		// ����Ÿ�� �ʱ�ȭ
			GenerateCount = UnityEngine.Random.Range(1, 4);
			DelayTime = UnityEngine.Random.Range(2, 4);

			for(int i = 0; i < GenerateCount; i++)
            {
				int ranpos = UnityEngine.Random.Range(0, 3);
				GameObject InstantMon = Instantiate(Setting.MonsterList[0], transform);
				if(ranpos == 0)
					InstantMon.transform.position = new Vector3(30, 0, UnityEngine.Random.Range(-30, 30));
				else if (ranpos == 1)
					InstantMon.transform.position = new Vector3(-30, 0, UnityEngine.Random.Range(-30, 30));
				else if (ranpos == 2)
					InstantMon.transform.position = new Vector3(UnityEngine.Random.Range(-30, 30), 0, 30);
				else if (ranpos == 3)
					InstantMon.transform.position = new Vector3(UnityEngine.Random.Range(-30, 30), 0, -30);

				MonsterPool.Add(InstantMon.GetComponent<DCL_MonsterBase>());
			}

		}
        #endregion
    }

}
