using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RockSpirits_Player
    // 
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class RS_Player : ObjectBase, RS_Weapon, RS_StatusBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Enum] 플레이어 액션 상태 (애니메이터 파라미터와 이름 동일시 필요)
        public enum ePlayerActionState
        {
            IDLE,
            WALK,
            RUN,
            DASH,
            INTERACTION,
            DASH_ATTACK,
            ATTACK_LIGHT_COMBO_STEP_1,
            ATTACK_LIGHT_COMBO_STEP_1_END,
            ATTACK_LIGHT_COMBO_STEP_2,
            ATTACK_LIGHT_COMBO_STEP_2_END,
            ATTACK_LIGHT_COMBO_STEP_3,
            ATTACK_HEAVY_COMBO_STEP_1,
            ATTACK_HEAVY_COMBO_STEP_2,
            HIT,
            DIE,
        }
        #endregion

        #region [Enum] PlayerDirection  
        public enum ePlayerDirection
        {
            FORWARD,
            BACK,
            LEFT,
            RIGHT,
            FORWARDLEFT,
            FORWARDRIGHT,
            BACKLEFT,
            BACKRIGHT,
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] Combo
        public class NComboAttack
        {
            public int ComboCount;      // 현재 공격 콤보 카운트
            public bool AttackAble;     // 현재 공격 가능한 상태인지
            public bool ComboAble;      // 현재 콤보 연계가 가능한 상태인지
            public bool Clicked;        // 마우스 클릭 상태
        }
        public NComboAttack ComboAttack = new NComboAttack();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] 플레이어 상태
        protected ePlayerActionState BeForePlayerActionState = ePlayerActionState.IDLE;   // 이전 플레이어 행동
        protected ePlayerActionState PlayerActionState = ePlayerActionState.IDLE;         // 현재 플레이어 행동
        protected ePlayerDirection PlayerDirection;             // 플레이어 방향
        protected ePlayerDirection PlayerAttackDirection;       // 플레이어 공격 방향
        #endregion

        #region [Variable] Animation
        protected Animator PlayerAnimation;
        #endregion

        #region [Variable] 플레이어 스텟
        public RS_StatusBase.NStatus PlayerStat = new RS_StatusBase.NStatus();
        #endregion

        #region [Variable] 플레이어 무기
        public RS_Weapon.NWeaponBase PlayerWeapon = new RS_Weapon.NWeaponBase();
        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Transform
        protected Transform playerTransform => transform;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
            PlayerAnimation = GetComponent<Animator>();
            InitStatus();
            SetPlayerWeapon();
        }
        #endregion

        #region [Stat] initStat
        public void SetPlayerWeapon()
        {
            PlayerWeapon.WeaponType = RS_Weapon.eWeaponType.ONE_HAND_SWORD;
            PlayerWeapon.WeaponGrade = RS_Weapon.eWeaponGrade.NORMAL;
            PlayerWeapon.AttackDamage = 10f;
            PlayerWeapon.AttackSpeed = 1f;
        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Animation
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Animation] SetTrigger By Animation State
        public void ChangeAnimationByTrigger(ePlayerActionState playerstate, ePlayerActionState beforeplayerstate)
        {
            PlayerAnimation.ResetTrigger(beforeplayerstate.ToString());
            PlayerAnimation.SetTrigger(playerstate.ToString());
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Direction
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Direction] SetPlayerDirection
        public ePlayerDirection SetPlayerDirection(Vector3 dir)
        {

            return ePlayerDirection.FORWARD;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. RS_Status
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [RS_Status] initStatus
        public void InitStatus()
        {
            PlayerStat.HP = 100;
            PlayerStat.MoveSpeed = 1;
            PlayerStat.CriticalPercent = 10;
            PlayerStat.CriticalDamage = 1.5f;
        }
        #endregion

    }

}