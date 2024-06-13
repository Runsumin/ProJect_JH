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
        #region [Enum] �÷��̾� �׼� ���� (�ִϸ����� �Ķ���Ϳ� �̸� ���Ͻ� �ʿ�)
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
            public int ComboCount;      // ���� ���� �޺� ī��Ʈ
            public bool AttackAble;     // ���� ���� ������ ��������
            public bool ComboAble;      // ���� �޺� ���谡 ������ ��������
            public bool Clicked;        // ���콺 Ŭ�� ����
        }
        public NComboAttack ComboAttack = new NComboAttack();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] �÷��̾� ����
        protected ePlayerActionState BeForePlayerActionState = ePlayerActionState.IDLE;   // ���� �÷��̾� �ൿ
        protected ePlayerActionState PlayerActionState = ePlayerActionState.IDLE;         // ���� �÷��̾� �ൿ
        protected ePlayerDirection PlayerDirection;             // �÷��̾� ����
        protected ePlayerDirection PlayerAttackDirection;       // �÷��̾� ���� ����
        #endregion

        #region [Variable] Animation
        protected Animator PlayerAnimation;
        #endregion

        #region [Variable] �÷��̾� ����
        public RS_StatusBase.NStatus PlayerStat = new RS_StatusBase.NStatus();
        #endregion

        #region [Variable] �÷��̾� ����
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