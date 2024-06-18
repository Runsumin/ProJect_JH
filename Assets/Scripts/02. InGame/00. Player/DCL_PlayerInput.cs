using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_PlayerInput
    // �÷��̾� ��ǲ Ŭ����. �÷��̾��� �Է°� ���� Ŭ����
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_PlayerInput : MonoBehaviour
    {
        private Vector3 moveDirection;
        private float moveSpeed = 4f;
        private Vector3 camforward;
        private Vector3 camright;

        public void Start()
        {
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
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }

        #region UNITY_EVENTS
        public void OnMove(InputAction.CallbackContext context)   // Unity Event�� ���� ���
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

