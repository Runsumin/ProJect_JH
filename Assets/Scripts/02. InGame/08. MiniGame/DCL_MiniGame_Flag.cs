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


    public class DCL_MiniGame_Flag : DCL_MiniGame_Base
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NMiniGame_FlagSetting
        {
            public Transform Flag_Root;
            public GameObject Flag_Prefab;
        }
        public NMiniGame_FlagSetting NMiniGame_FlagSet = new NMiniGame_FlagSetting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public float InGameTimer;
        public int Item_MaxCount;
        public int Item_NowCount;
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
            Setting.Explanation = "깃발을 따라가세요!";
            Setting.Position = pos;
            Item_MaxCount = 5;   // 나중에 외부로 빼서 난이도 조절에 추가 필요...
            Item_NowCount = 0;

        }
        #endregion

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            Setting.MiniGameEnd = false;
            Setting.MiniGameClear = false;
            NMiniGame_FlagSet.Flag_Root = gameObject.transform;
            ItemSpread(20);
        }
        #endregion

        #region [Init] ItemSpread
        public void ItemSpread(float range)
        {
            // 쓰레기 위치 추후에 변경 필요
            Vector3 pos = UnityEngine.Random.insideUnitSphere * range;
            pos.y = 0;
            GameObject Flag = Instantiate(NMiniGame_FlagSet.Flag_Prefab, pos, Quaternion.identity, NMiniGame_FlagSet.Flag_Root);
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
                if (Item_NowCount < Item_MaxCount)
                {
                    Setting.MiniGameEnd = true;
                    Setting.MiniGameClear = false;
                }
                else
                {
                    Setting.MiniGameEnd = true;
                    Setting.MiniGameClear = true;
                }
            }
            else
            {
                if (Item_NowCount >= Item_MaxCount)
                {
                    Setting.MiniGameEnd = true;
                    Setting.MiniGameClear = true;
                }
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
        // 2. Count
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Count] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ChangeCountData(int add)
        {
            Item_NowCount += add;
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
