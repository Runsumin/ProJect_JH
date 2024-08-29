using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// DCL_WeaponBase
// 인게임 모든 무기 베이스 클래스
// 
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

namespace HSM.Game
{
    public class DCL_WeaponBase : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum AttackType { MELEE, RANGE }
        public enum AttackState { ATTACKING, COOLTIME }
        public enum WeaponLevel { LEVEL_1, LEVEL_2, LEVEL_3, LEVEL_4, LEVEL_5, LEVEL_6, LEVEL_7, LEVEL_8, LEVEL_9, LEVEL_10 }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        [Serializable]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public class NSetting
        {
            public GameObject HitBox;                   // 콜라이더 박스
            public GameObject Effect;                   // 이펙트 -> 이펙트에 맞춰 조절 기능 추가
            public float AttackSpeed;                   // 초당 공격속도
            public float AttackCoolTime;                // 공격 쿨타임
            public float AttackRange;                   // 공격범위
            public AttackType Weapon_AttackType;        // 공격타입
            public AttackState Weapon_AttackState;      // 공격상태

            public WeaponLevel NowWeaponLevel;                  // 무기 레벨
        }
        public NSetting Setting = new NSetting();
        #endregion

        #region [NestedClass] AttackPoint
        [Serializable]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public class AttackPointSetting
        {
            public Transform ATTransform;

        }
        public AttackPointSetting ATSetting = new AttackPointSetting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] AttackDirection
        public Vector3 AttackPos;
        public Vector3 AttackDirection;
        #endregion


        #region [Variable] PlayerData
        public Transform PlayerPos;
        public Vector3 PlayerAttackDirection;
        public DCL_Status PlayerStatus;
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

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            SetPlayerData();
        }
        #endregion

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Update()
        {
            SetAttackPoint();
            SetPlayerData();
        }
        #endregion


        #region [Init] SetPlayerData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetPlayerData()
        {
            GameObject pl = GameObject.FindWithTag("Player");
            PlayerPos = pl.transform;
            PlayerAttackDirection = pl.GetComponent<DCL_PlayerInput>().AttackDirection;
            PlayerStatus = pl.GetComponent<DCL_PlayerBase>().PL_Status;
            SetAttackPoint();
        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Attack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Attack] Collider Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void ColliderMove()
        {

        }
        #endregion

        #region [Attack] Change AttackState
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ChangeAttackState(AttackState state)
        {
            switch (state)
            {
                case AttackState.ATTACKING:
                    Setting.HitBox.SetActive(true);
                    break;
                case AttackState.COOLTIME:
                    Setting.HitBox.SetActive(false);
                    break;
            }
        }
        #endregion

        #region [Attack] SetAttackPoint
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetAttackPoint()
        {
            ATSetting.ATTransform.position = transform.position + PlayerAttackDirection;
            //ATSetting.ATTransform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. etc
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    }

}

