using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // ObjectBase
    // 인게임내 생성되는 모든 오브젝트 베이스 클래스
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public abstract class ObjectBase : MonoBehaviour
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] ObjectBase

        #endregion

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        #region [Nested] Status
        [Serializable]
        public class DCL_Status
        {
            public float Attack_Power;      // 공격력
            public float Attack_Speed;      // 공격속도
            public int Cri_Percent;         // 치명타확률
            public float Critical_Damage;   // 치명타데미지
            public float Move_Speed;        // 이동속도
            public float Defense;           // 방어력
            public float HP;                // 체력
            public float HP_Recovery;       // 체력재생량
            public float Cleaning_Speed;    // 청소속도
            public float Gain_Range;        // 획득범위

            public DCL_Status(float att_pwr, float att_spd, int cri_per, float cri_dmg, float mov_spd, float def, float hp, float hp_re, float clean_spd, float gain_rng)
            {
                this.Attack_Power = att_pwr;
                this.Attack_Speed = att_spd;
                this.Cri_Percent = cri_per;
                this.Critical_Damage = cri_dmg;
                this.Move_Speed = mov_spd;
                this.Defense = def;
                this.HP = hp;
                this.HP_Recovery = hp_re;
                this.Cleaning_Speed = clean_spd;
                this.Gain_Range = gain_rng;
            }

            public static DCL_Status operator +(DCL_Status p1, DCL_Status p2)
            {
                return new DCL_Status(p1.Attack_Power + p2.Attack_Power, 
                    p1.Attack_Speed + p2.Attack_Speed , 
                    p1.Cri_Percent + p2.Cri_Percent,
                    p1.Critical_Damage + p2.Critical_Damage,
                    p1.Move_Speed + p2.Move_Speed,
                    p1.Defense + p2.Defense,
                    p1.HP + p2.HP,
                    p1.HP_Recovery + p2.HP_Recovery,
                    p1.Cleaning_Speed + p2.Cleaning_Speed,
                    p1.Gain_Range + p2.Gain_Range);
            }
        }
        #endregion

        #region [const] Coll Layer
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public const int PLAYERCOLLIDER = 6;
        public const int MONSTERCOLLIDER = 7;
        public const int MONSTERWEAPONCOLLIDER = 8;
        public const int PLAYERWEAPONCOLLIDER = 9;
        public const int PLAYERINTERACTIONCOLLIDER = 10;
        public const int ITEM = 11;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] TransForm
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #region [Property] [Transform] Rotation
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual float LocalRotationY
        {
            get => LocalRotation.eulerAngles.y;
            set => LocalRotation = Quaternion.Euler(0f, value, 0f);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual Quaternion LocalRotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual float RotationY
        {
            get => Rotation.eulerAngles.y;
            set => Rotation = Quaternion.Euler(0f, value, 0f);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual Vector3 ForwardDirection
        {
            get => transform.forward;
            set => transform.rotation = Quaternion.LookRotation(value);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual Vector3 LocalForwardDirection
        {
            get => transform.worldToLocalMatrix.MultiplyVector(transform.forward);
            set => transform.localRotation = Quaternion.LookRotation(value);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual Vector3 RightDirection => transform.right;
        public virtual Vector3 UpDirection => transform.up;
        #endregion

        #region [Property] Visible
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Show() => SetVisible(true);
        public virtual void Hide() => SetVisible(false);
        public virtual void SetVisible(bool visible) => gameObject.SetActive(visible);
        public virtual bool IsVisible => gameObject.activeSelf;
        public virtual bool IsActive => gameObject.activeSelf;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Awake() { }
        public virtual void Start() { }
        #endregion


        #region [Init] Enable & Disable 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        #endregion

        #region [Init] Destroy
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Destroy() { }
        #endregion

        #region [Init] Reset
        public virtual void Reset() { }
        #endregion
    }

}
