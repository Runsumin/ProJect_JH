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

    public class DCL_PlayerInput : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region[Variable] Move
        public Vector3 moveDirection;
        private Vector3 camforward;
        private Vector3 camright;
        #endregion

        #region[Variable] Player Data
        private DCL_Status Player_Status; 
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Base Methods] Start
        public override void Start()
        {
            Player_Status = this.GetComponent<DCL_PlayerBase>().PL_Status;

            camforward = Camera.main.transform.forward;
            camforward.y = 0f;

            camforward = Vector3.Normalize(camforward);
            camright = Quaternion.Euler(new Vector3(0, 90, 0)) * camforward;
        }
        #endregion

        #region [Base Methods] Update
        public void Update()
        {
            // �ϴ� �̷������� �޾ƿ���... ���Ŀ� �ٸ� ��� ��� �ʿ��غ���
            Player_Status = this.GetComponent<DCL_PlayerBase>().PL_Status;

            moveDirection = SetDirction(moveDirection); 

            bool hasControl = (moveDirection != Vector3.zero);
            if (hasControl)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
                transform.Translate(Vector3.forward * Player_Status.Move_Speed * Time.deltaTime);
            }
        }
        #endregion

        #region [Base Methods] Update
        public void FixedUpdate()
        {
            //// �ϴ� �̷������� �޾ƿ���... ���Ŀ� �ٸ� ��� ��� �ʿ��غ���
            //Player_Status = this.GetComponent<DCL_PlayerBase>().PL_Status;

            //bool hasControl = (moveDirection != Vector3.zero);
            //if (hasControl)
            //{
            //    transform.rotation = Quaternion.LookRotation(moveDirection);
            //    transform.Translate(Vector3.forward * Player_Status.Move_Speed * Time.deltaTime);
            //}
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Input Control
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Input Control] Move - InputAction
        public void OnMove(InputAction.CallbackContext context)   // Unity Event�� ���� ���
        {
            Vector2 input = context.ReadValue<Vector2>();
            if (input != null)
            {
                //Vector3 rightmov = camright * input.x;
                //Vector3 forwardmov = camforward * input.y;
                //moveDirection = rightmov + forwardmov;
            }
        }
        #endregion

        #region [Input Control] Move - Legacy
        public Vector3 SetDirction(Vector3 dir)   // Unity Event�� ���� ���
        {
            float h = Input.GetAxis("Horizontal");  
            float v = Input.GetAxis("Vertical");

            Vector3 rightmov = camright * h;
            Vector3 forwardmov = camforward * v;
            dir = rightmov + forwardmov;

            return dir;
        }
        #endregion

    }

}

