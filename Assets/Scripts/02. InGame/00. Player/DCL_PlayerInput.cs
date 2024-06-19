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

    public class DCL_PlayerInput : DCL_PlayerBase
    {
        private Vector3 moveDirection;
        private Vector3 camforward;
        private Vector3 camright;

        public override void Start()
        {
            base.Start();

            camforward = Camera.main.transform.forward;
            camforward.y = 0f;

            camforward = Vector3.Normalize(camforward);
            camright = Quaternion.Euler(new Vector3(0, 90, 0)) * camforward;
        }

        public void Update()
        {
            bool hasControl = (moveDirection != Vector3.zero);
            if (hasControl)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
                transform.Translate(Vector3.forward * Player_Status.Move_Speed * Time.deltaTime);
            }
        }

        #region UNITY_EVENTS
        public void OnMove(InputAction.CallbackContext context)   // Unity Event로 받을 경우
        {
            Vector2 input = context.ReadValue<Vector2>();
            if (input != null)
            {
                Vector3 rightmov = camright * input.x;
                Vector3 forwardmov = camforward * input.y;
                moveDirection = rightmov + forwardmov;
                Debug.Log($"UNITY_EVENTS : {input.magnitude}");
            }
        }
        #endregion
    }

}

