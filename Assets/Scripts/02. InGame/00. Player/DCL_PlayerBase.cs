using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_PlayerBase
    // 플레이어 베이스 클래스 , 플레이어의 상태를 관리한다
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class DCL_PlayerBase : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum PlayerState { IDLE, RUN, INTERACTION, DEATH };

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NSetting
        {
            //public DCL_Status Player_Status = new DCL_Status();                         
            public List<NPlayerLevle> Player_Status_Level = new List<NPlayerLevle>();   // 플레이어 스텟 
            public float NowEXP;
        }
        public NSetting Setting = new NSetting();
        #endregion

        #region [InterAction] 상호작용
        [Serializable]
        public class NInterAction
        {
            public float InterAction_Range;
            public float InterAction_Count;
            public float InterAction_Time;
        }
        public NInterAction Player_InterAction = new NInterAction();
        #endregion

        #region [Level] 플레이어 레벨
        [Serializable]
        public class NPlayerLevle
        {
            public DCL_Status Pl_Status;
            public int Level;
            public float MaxEXP;

            public NPlayerLevle(DCL_Status status, int lv)
            {
                this.Pl_Status = status;
                this.Level = lv;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Parsing
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Parsing] PlayerStatus_Level
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class PlayerStatus_Level
        {
            public List<NPlayerLevle> Player_Status_Level;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] HP
        public float NowHP;
        private float HPRecoveryTime;
        private float HPRecoveryCoolTime;
        #endregion

        #region [Variable] Hit
        private float NowHitCoolTime;  // 히트 쿨타임 -> 무적시간
        private float HitCoolTime;  // 히트 쿨타임 -> 무적시간
        private bool HitAble;
        #endregion

        #region [Variable] Level
        public int NowPlayerLevel;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public DCL_Status PL_Status => Setting.Player_Status_Level[NowPlayerLevel].Pl_Status;       // 플레이어 스텟
        #endregion




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Awake
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            //Json_Utility_Extend.FileSaveList(Setting.Player_Status_Level, "Data/Json_Data/Player/Player_Status.Json");
            Setting.Player_Status_Level = Json_Utility_Extend.FileLoadList<NPlayerLevle>("Data/Json_Data/Player/Player_Status.Json");
        }
        #endregion

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            NowHP = PL_Status.HP;
            HitCoolTime = 2;
            HitAble = true;
        }
        #endregion

        #region [Update]    
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            Auto_HP_Recovery();
            Debug.Log(NowHP);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Status_Change
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Status_Change] HP Recovery
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Auto_HP_Recovery()
        {
            HPRecoveryTime += Time.deltaTime;
            if (HPRecoveryTime > 3)
            {
                if (NowHP < PL_Status.HP)
                {
                    NowHP += PL_Status.HP_Recovery;
                    HPRecoveryTime = 0;
                }
            }
        }
        #endregion

        #region [Status_Change] Hit
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Hit_HP_Reduce(float monatt)
        {
            NowHP -= monatt;
            StartCoroutine(HitCoolTimeChange());
        }
        #endregion

        #region [Status_Change] HitCoolTime
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerator HitCoolTimeChange()
        {
            float cooltime = 0;
            while(true)
            {
                cooltime += Time.deltaTime;
                if (cooltime > HitCoolTime)
                {
                    HitAble = true;
                    break;
                }
            }
            yield return null;
        }
        #endregion

        //#region [Status_Change] Level_Up
        ////------------------------------------------------------------------------------------------------------------------------------------------------------
        //public void LevelUp()
        //{
        //    if(Setting.NowEXP >= Setting.Player_Status_Level[NowPlayerLevel].MaxEXP)
        //    {
        //        NowPlayerLevel++;
        //        Setting.NowEXP = 0;
        //    }
        //}
        //#endregion

        #region [Status_Change] Add_EXP
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Add_EXP(int amount)
        {
            Setting.NowEXP += amount;
            if (Setting.NowEXP >= Setting.Player_Status_Level[NowPlayerLevel].MaxEXP)
            {
                NowPlayerLevel++;
                Setting.NowEXP = 0;
            }
        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Move
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Attack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 4. InterAction
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [InterAction] - Collision - MonsterBody
        public void OnTriggerEnter(Collider coll)
        {
            // Layer = 7 : monstercoll, 8 = MonsterWeaponcoll
            if(HitAble && coll.gameObject.layer == MONSTERCOLLIDER)
            {
                HitAble = false;
                float att = coll.gameObject.GetComponent<DCL_MonsterBase>().Mon_Status.Attack_Power;
                Hit_HP_Reduce(att);
            }

        }
        #endregion

        #region [InterAction] - Collision - EXP
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Death
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    }

}
