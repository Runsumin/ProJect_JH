using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_MiniGame_Cleaning
    // 미니게임 - 청소
    // - 제한시간동안 정해진 목표물 청소하기
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class DCL_MiniGame_Cleaning : DCL_MiniGame_Base
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NMiniGame_CleaningSetting
        {
            public Collider HitCollider;
            public Transform Trash_Root;
            public GameObject Trash_Prefab;
            public List<GameObject> Item_Trasharr;
            public float Item_MaxCount;
            public float Item_NowCount;
        }
        public NMiniGame_CleaningSetting NMiniGame_CleaningSet = new NMiniGame_CleaningSetting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public float InGameTimer;        
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

        #region [Init] Initialize
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Initialize(float time, float GameTime, Vector3 pos)
        {
            Setting.GameStatus = MiniGameStatus.Start;
            Setting.GameType = MiniGameType.Flag;
            Setting.EventStartTime = time;
            Setting.GameIngTime = GameTime;
            Setting.EventEndTime = Setting.EventStartTime + Setting.GameIngTime;
            Setting.Explanation = "제한시간 안에 쓰레기를 청소하세요!";
            Setting.Position = pos;
            NMiniGame_CleaningSet.Item_MaxCount = 10;   // 나중에 외부로 빼서 난이도 조절에 추가 필요...
            NMiniGame_CleaningSet.Item_NowCount = 0;

        }
        #endregion

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            Setting.MiniGameEnd = false;
            Setting.MiniGameClear = false;
            ItemSpread(NMiniGame_CleaningSet.Item_MaxCount, 30);
        }
        #endregion

        #region [Init] ItemSpread
        public void ItemSpread(float count, float range)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = UnityEngine.Random.insideUnitSphere * range;
                pos.y = 0;
                GameObject trash = Instantiate(NMiniGame_CleaningSet.Trash_Prefab, pos, Quaternion.identity, NMiniGame_CleaningSet.Trash_Root);
                NMiniGame_CleaningSet.Item_Trasharr.Add(trash);
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Update
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Update] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Update()
        {
            if (MiniGameTimer() == true)
            {
                Setting.MiniGameEnd = true;
                Setting.MiniGameClear = true;
            }
        }
        #endregion

        #region [Update] GameTimer
        public bool MiniGameTimer()
        {
            Setting.GameIngTime -= Time.deltaTime;
            if (Setting.GameIngTime < 0)
                return true;
            else
                return false;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Collider
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collider] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnTriggerExit(Collider other)
        {

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Clear
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Clear] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Clear()
        {

        }
        #endregion

        #region [Clear] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Destroy()
        {
            Destroy(gameObject);
        }
        #endregion
    }

}
