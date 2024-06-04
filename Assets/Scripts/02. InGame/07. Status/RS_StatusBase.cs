using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RockSpirits_StatusBase
    //      ������Ʈ ���� ���� �������̽�
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public interface RS_StatusBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] Status �⺻ ����
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public class NStatus
        {
            public float HP;                // ü��                   
            public float MoveSpeed;         // �̵��ӵ�
            public float CriticalDamage;    // ũ��Ƽ�� ���ݷ� ������
            public float CriticalPercent;   // ũ��Ƽ�� Ȯ�� - �ۼ�Ʈ % 

            public NStatus(/*float hp, float movespeed, float cridamage, float criper*/)
            {
                //hp = HP;
                //attackdamage = AttackDamage;
                //attackspeed = AttackSpeed;
                //movespeed = MoveSpeed;
                //cridamage = CriticalDamage;
                //criper = CriticalPercent;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Base] Init ���� �ʱ�ȭ
        void InitStatus();
        #endregion
    }

}