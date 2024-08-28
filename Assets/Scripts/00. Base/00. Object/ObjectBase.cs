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
