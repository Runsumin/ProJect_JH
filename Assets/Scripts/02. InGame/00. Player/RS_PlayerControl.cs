using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RockSpirits_PlayerInput
    //      플레이어 인풋 관리 클래스
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class RS_PlayerControl : RS_Player
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Move
        private Vector3 moveDirection;      // 이동 방향
        private float moveSpeed = 6f;       // 이동 속도

        private Vector3 camforward;
        private Vector3 camright;

        private Vector3 beforeMoveDir;      // 이동방향 저장
        #endregion

        #region [Varialbe] Dash
        bool IsDashAble = true;             // 플레이어 대쉬 가능 상태
        private float DashCoolTime = 1f;    // 대쉬 쿨타임
        private bool dashfinished = false;
        #endregion

        #region [Variable] Attack
        private Vector3 Attdir;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        #region MyRegion

        #endregion

        private Rigidbody PlayerRigidBody;

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
            PlayerRigidBody = GetComponent<Rigidbody>();

            camforward = Camera.main.transform.forward;
            camforward.y = 0f;

            camforward = Vector3.Normalize(camforward);
            camright = Quaternion.Euler(new Vector3(0, 90, 0)) * camforward;
        }
        #endregion

        #region [Update]
        void Update()
        {
            bool isaction = (moveDirection != Vector3.zero);
            if (isaction /*&& PlayerActionState == RS_Player.ePlayerActionState.WALK*/)
            {
                playerTransform.rotation = Quaternion.LookRotation(moveDirection);
                playerTransform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            }
            //else
            //{
            //    SetStateNAnimation(RS_Player.ePlayerActionState.IDLE);
            //}
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Animation Control
        // 
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [State] SetStateNAnimation
        public void SetStateNAnimation(RS_Player.ePlayerActionState state)
        {
            BeForePlayerActionState = PlayerActionState;
            PlayerActionState = state;
            ChangeAnimationByTrigger(PlayerActionState, BeForePlayerActionState);
        }
        #endregion

        #region [CallBackEvent] EndFrame
        public void AttackEndFrame(string s)
        {
            ComboAttack.ComboAble = true;
            Debug.Log("AttackEndFrame: " + s + " called");
        }
        #endregion

        #region [CallBackEvent] EndFrame
        public void DashEndFrame(string s)
        {
            dashfinished = true;
            Debug.Log("AttackEndFrame: " + s + " called");
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Actions Control
        //
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Actions] Move
        public void OnMove(InputAction.CallbackContext context)
        {
            ComboAttack.ComboAble = true;
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 rightmov = camright * input.x;
            Vector3 forwardmov = camforward * input.y;
            moveDirection = rightmov + forwardmov;

            if (PlayerActionState != RS_Player.ePlayerActionState.DASH)
                SetStateNAnimation(RS_Player.ePlayerActionState.WALK);
        }
        #endregion

        #region [Actions] Idle
        public void OnIdle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SetStateNAnimation(RS_Player.ePlayerActionState.IDLE);
            }
        }
        #endregion

        #region [Actions] Dash
        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (IsDashAble)
                {
                    IsDashAble = false;
                    SetStateNAnimation(RS_Player.ePlayerActionState.DASH);
                    StartCoroutine(SetDashDelay(DashCoolTime));
                }
            }
        }

        IEnumerator SetDashDelay(float CoolTime)
        {
            float inittime = 0;
            dashfinished = false;

            while (true)
            {
                inittime += Time.deltaTime;

                if (dashfinished == true)
                {
                    // 상태 변환
                    if (BeForePlayerActionState == RS_Player.ePlayerActionState.WALK)
                        SetStateNAnimation(RS_Player.ePlayerActionState.WALK);
                    else
                        SetStateNAnimation(RS_Player.ePlayerActionState.IDLE);

                    dashfinished = false;
                }

                if (inittime >= CoolTime)
                {
                    IsDashAble = true;
                    break;
                }

                yield return null;
            }
            yield break;
        }
        #endregion

        #region [Actions] Attack
        public void OnAttack(InputAction.CallbackContext context)
        {
            playerTransform.rotation = Quaternion.LookRotation(Attdir);

            if (context.performed)
            {
                if (ComboAttack.AttackAble == false)
                {
                    ComboAttack.AttackAble = true;
                    if (ComboAttack.ComboCount == 0)
                    {
                        SetStateNAnimation(RS_Player.ePlayerActionState.ATTACK_LIGHT_COMBO_STEP_1);
                    }
                    else if (ComboAttack.ComboCount == 1)
                    {
                        SetStateNAnimation(RS_Player.ePlayerActionState.ATTACK_LIGHT_COMBO_STEP_2);
                    }
                    else if (ComboAttack.ComboCount == 2)
                    {
                        SetStateNAnimation(RS_Player.ePlayerActionState.ATTACK_LIGHT_COMBO_STEP_3);
                    }
                    StartCoroutine(SetLightAttackCombo());
                }
                else
                {
                    ComboAttack.Clicked = true;
                }
            }
        }

        IEnumerator SetLightAttackCombo()
        {
            //float inittime = 0f;
            moveSpeed = 0.5f;
            while (true)
            {
                //inittime += Time.deltaTime;

                if (ComboAttack.ComboAble == true)
                {
                    // 상태 변환
                    moveSpeed = 6f;
                    ComboAttack.AttackAble = false;
                    ComboAttack.ComboAble = false;
                    if(ComboAttack.Clicked == true)
                    {
                        ComboAttack.Clicked = false;
                        if (ComboAttack.ComboCount > 2)
                        {
                            ComboAttack.ComboCount = 0;
                        }
                        if(ComboAttack.ComboCount == 0)
                        {

                        }
                        else if (ComboAttack.ComboCount == 1)
                        {

                        }
                        if (ComboAttack.ComboCount == 2)
                        {

                        }
                        ComboAttack.ComboCount++;
                    }
                    else
                    {
                        ComboAttack.ComboCount = 0;
                        SetStateNAnimation(RS_Player.ePlayerActionState.IDLE);
                    }
                    break;
                }

                yield return null;
            }
            yield break;
        }
        #endregion

        #region [Actions] SetAttackDirection
        public void SetAttackDirection(InputAction.CallbackContext context)
        {
            Vector3 playerscreenpos = Camera.main.WorldToScreenPoint(playerTransform.position);
            Vector2 input = context.ReadValue<Vector2>();

            var mousedir = new Vector3(input.x, input.y);

            var attdir = Vector3.Normalize(mousedir - playerscreenpos);

            Vector3 rightmov = camright * attdir.x;
            Vector3 forwardmov = camforward * attdir.y;

            var final = Vector3.Normalize(rightmov + forwardmov);

            Attdir = final;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Utill
        // 
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Utill] ConvertIsoViewMatrix
        public Vector3 ConvertIsoViewMatrix(Vector3 data)
        {
            return Vector3.zero;
        }
        #endregion
    }

}