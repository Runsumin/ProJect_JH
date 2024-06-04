using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RockSpirits_StatusBase
    //      오브젝트 상태 관련 인터페이스
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public interface RS_StatusBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] Status 기본 스탯
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public class NStatus
        {
            public float HP;                // 체력                   
            public float MoveSpeed;         // 이동속도
            public float CriticalDamage;    // 크리티컬 공격력 증가량
            public float CriticalPercent;   // 크리티컬 확률 - 퍼센트 % 

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

        #region [Base] Init 스텟 초기화
        void InitStatus();
        #endregion
    }

}