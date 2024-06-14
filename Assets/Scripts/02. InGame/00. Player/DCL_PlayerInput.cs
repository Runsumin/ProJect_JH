using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float moveZ = 0f;
            float moveX = 0f;
            if (Input.GetKey(KeyCode.W))
            {
                moveX += 1f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                moveX -= 1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveZ -= 1f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                moveZ += 1f;
            }

            transform.Translate(new Vector3(moveX, 0f, moveZ) * 0.1f);

        }
    }

}
