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
	// 웨이브 기반 생성
    // 어느 정도의 무작위성, 특정 시간대에 특정 패턴 몬스터 등장
	// 화면 모든 방향에서 등장, 
	// 게임이 진행될수록 강력한 몬스터 등장
	// 게임이 진행될수록 몬스터의 종류나 출현 방식 변화
	// 특정 시간, 조건에 엘리트나 보스 등장
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
		}
		public NSetting Setting = new NSetting();
		#endregion



		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		// Variable
		//
		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		#region [Variable] Base
		public List<DCL_MonsterBase> MonsterPool = new List<DCL_MonsterBase>();
		public float DelayTime;			// 몬스터 생성 딜레이 지정타임
		public float LoopTime;			// 몬스터 생성 딜레이 루프타임
		public int GenerateCount;		// 한번에 생성할 몬스터 개수
		public int DoubleSpeed;         // 배속
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
			DelayTime = 0;          // 첫 딜레이 타임은 2초
			DoubleSpeed = 1;		// 배속
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
			LoopTime = 0;		// 루프타임 초기화
			GenerateCount = UnityEngine.Random.Range(1, 4);
			DelayTime = UnityEngine.Random.Range(4, 6);
			
			for (int i = 0; i < GenerateCount; i++)
            {				
				GameObject InstantMon = Instantiate(Setting.MonsterList[0], transform);
			}

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
