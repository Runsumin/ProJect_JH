using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_PlayerInput
    // 플레이어 인풋 클래스. 플레이어의 입력값 관리 클래스
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_PlayerInput : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum ePlayerActionState
        {
            IDLE,
            RUN,
            DEATH
        };

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        [Header("Asset")]
        public InputActionAsset actionAsset;

        [Header("Action")]
        public InputActionReference MoveAction;
        public InputActionReference IdleAction;
        public InputActionReference AttackDirectionAction;

        #region[Variable] Move
        public Vector3 moveDirection;
        private Vector3 camforward;
        private Vector3 camright;
        #endregion

        #region[Variable] Attack
        public Vector3 AttackDirection;
        #endregion

        #region[Variable] Player Data
        private DCL_Status Player_Status;
        #endregion

        #region [Variable] Animation
        protected Animator PlayerAnimation;
        protected ePlayerActionState BeForePlayerActionState = ePlayerActionState.IDLE;   // 이전 플레이어 행동
        protected ePlayerActionState PlayerActionState = ePlayerActionState.IDLE;         // 현재 플레이어 행동
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Base Methods] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            Player_Status = this.GetComponent<DCL_PlayerBase>().PL_Status;

            camforward = Camera.main.transform.forward;
            camforward.y = 0f;

            camforward = Vector3.Normalize(camforward);
            camright = Quaternion.Euler(new Vector3(0, 90, 0)) * camforward;

            PlayerAnimation = GetComponent<Animator>();
        }
        #endregion

        #region [Base Methods] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            // 일단 이런식으로 받아오기... 추후에 다른 방법 고려 필요해보임
            Player_Status = this.GetComponent<DCL_PlayerBase>().PL_Status;

            //moveDirection = SetDirction(moveDirection);

            bool hasControl = (moveDirection != Vector3.zero);
            if (hasControl)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
                transform.Translate(Vector3.forward * Player_Status.Move_Speed * Time.deltaTime);
            }

        }
        #endregion

        #region [Base Methods] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void FixedUpdate()
        {
            //// 일단 이런식으로 받아오기... 추후에 다른 방법 고려 필요해보임
            //Player_Status = this.GetComponent<DCL_PlayerBase>().PL_Status;

            //bool hasControl = (moveDirection != Vector3.zero);
            //if (hasControl)
            //{
            //    transform.rotation = Quaternion.LookRotation(moveDirection);
            //    transform.Translate(Vector3.forward * Player_Status.Move_Speed * Time.deltaTime);
            //}
        }
        #endregion

        #region [Base Methods] OnEnable
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private new void OnEnable()
        {
            actionAsset.Enable();

            MoveAction.action.performed += OnMove;
            IdleAction.action.performed += OnIdle;
            AttackDirectionAction.action.performed += OnAttDIr;
            //MoveAction.action.canceled += OnMove;
        }
        #endregion

        #region [Base Methods] OnDisable
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private new void OnDisable()
        {
            actionAsset.Disable();

            MoveAction.action.performed -= OnMove;
            IdleAction.action.performed -= OnIdle;
            AttackDirectionAction.action.performed -= OnAttDIr;
            //MoveAction.action.canceled -= OnMove;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Input Control
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Actions] Idle
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnIdle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SetStateNAnimation(ePlayerActionState.IDLE);
            }
        }
        #endregion

        #region [Input Control] Move - InputAction
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnMove(InputAction.CallbackContext context)   // Unity Event로 받을 경우
        {
            Vector2 input = context.ReadValue<Vector2>();

            if (input != null)
            {
                Vector3 rightmov = camright * input.x;
                Vector3 forwardmov = camforward * input.y;
                moveDirection = rightmov + forwardmov;
            }
            SetStateNAnimation(ePlayerActionState.RUN);
        }
        #endregion

        #region [Input Control] AttackDir - InputAction
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnAttDIr(InputAction.CallbackContext context)   // Unity Event로 받을 경우
        {
            //Vector2 input = context.ReadValue<Vector2>();
            //if (input != null)
            //{
            //    Vector3 WorldMousePosition = Camera.main.ScreenToWorldPoint(/*new Vector3(input.x,0,input.y)*/input);
            //    Vector3 plpos = this.GetComponent<DCL_PlayerBase>().transform.position;
            //    AttackDirection = (plpos - WorldMousePosition).normalized;
            //    AttackDirection.y = 0;
            //    Debug.Log(input);
            //}
            Vector3 plpos = this.GetComponent<DCL_PlayerBase>().transform.position;

            Vector3 playerscreenpos = Camera.main.WorldToScreenPoint(plpos);
            Vector2 input = context.ReadValue<Vector2>();

            var mousedir = new Vector3(input.x, input.y);

            var attdir = Vector3.Normalize(mousedir - playerscreenpos);

            Vector3 rightmov = camright * attdir.x;
            Vector3 forwardmov = camforward * attdir.y;

            var final = Vector3.Normalize(rightmov + forwardmov);

            AttackDirection = final;
        }
        #endregion

        #region [Input Control] Move - Legacy
        public Vector3 SetDirction(Vector3 dir)   // Unity Event로 받을 경우
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 rightmov = camright * h;
            Vector3 forwardmov = camforward * v;
            dir = rightmov + forwardmov;

            return dir;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Animation
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Animation] SetTrigger By Animation State
        public void ChangeAnimationByTrigger(ePlayerActionState playerstate, ePlayerActionState beforeplayerstate)
        {
            PlayerAnimation.ResetTrigger(beforeplayerstate.ToString());
            PlayerAnimation.SetTrigger(playerstate.ToString());
        }
        #endregion

        #region [Animation] SetStateAnimation
        public void SetStateNAnimation(ePlayerActionState state)
        {
            BeForePlayerActionState = PlayerActionState;
            PlayerActionState = state;
            ChangeAnimationByTrigger(PlayerActionState, BeForePlayerActionState);
        }
        #endregion

    }

}

