using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RockSpirits_PlayerWeapon
    //      플레이어 무기 관련 베이스 클래스
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public interface RS_Weapon
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum eWeaponType
        {
            TWO_HAND_HAMMER,
            ONE_HAND_SWORD,
        }

        public enum eWeaponGrade
        {
            NORMAL,
            EPIC,
            REGENDARY,
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Nested] NWeaponBase
        public class NWeaponBase
        {
            public eWeaponType WeaponType;      // 무기 타입
            public eWeaponGrade WeaponGrade;    // 무기 등급
            public float AttackSpeed;           // 공격 속도
            public float AttackDamage;          // 공격 데미지
            public bool HitAble;                // 무기가 충돌 처리 가능한지.
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] HitDitection
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Hit Detection
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [CallBackEvent] HitDetectionStart
        public virtual void HitDetectionStart(string s)
        {
            Debug.Log("HitDetectionStart: " + s + " called");
        }
        #endregion

        #region [CallBackEvent] HitDetectionEnd
        public virtual void HitDetectionEnd(string s)
        {
            Debug.Log("HitDetectionEnd: " + s + " called");
        }
        #endregion

    }

}