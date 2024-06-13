using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RockSpirits_PlayerWeapon
    //      �÷��̾� ���� ���� ���̽� Ŭ����
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
            public eWeaponType WeaponType;      // ���� Ÿ��
            public eWeaponGrade WeaponGrade;    // ���� ���
            public float AttackSpeed;           // ���� �ӵ�
            public float AttackDamage;          // ���� ������
            public bool HitAble;                // ���Ⱑ �浹 ó�� ��������.
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