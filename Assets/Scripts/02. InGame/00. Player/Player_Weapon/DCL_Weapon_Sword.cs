using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// DCL_WeaponBase
// 근접무기 - 검 클래스
// 
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

namespace HSM.Game
{
    public class DCL_Weapon_Sword : DCL_WeaponBase
    {

		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		// Nested Class
		//
		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		// Variable
		//
		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		#region [Variable] Sword SwingBase
		public Transform _target;
		public Transform _p1, _p2, _p3;
		#endregion

		#region [Variable] Rotation
		private Quaternion InitRotation;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
		{
			base.Start();
			Setting.Weapon_AttackType = AttackType.MELEE;
			Setting.Weapon_AttackState = AttackState.ATTACKING;
			StartCoroutine(ColliderMove());
			InitRotation = transform.localRotation;
		}
		#endregion

		#region [Update] 
		//------------------------------------------------------------------------------------------------------------------------------------------------------
		public new void Update()
        {
			transform.localRotation = InitRotation;
		}
		#endregion

		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		// 1. Attack
		//
		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		#region [Attack] Collider Move
		//------------------------------------------------------------------------------------------------------------------------------------------------------
		IEnumerator ColliderMove(float duration = 1.0f)
		{
			float time = 0f;

			while (true)
			{
				if (time > 1)
				{
					ChangeAttackState(AttackState.COOLTIME);
					if(time > 1 + Setting.AttackCoolTime)
                    {
						time = 0f;
						ChangeAttackState(AttackState.ATTACKING);
					}
				}

				Vector3 p4 = Vector3.Lerp(_p1.position, _p2.position, time);
				Vector3 p5 = Vector3.Lerp(_p2.position, _p3.position, time);
				_target.position = Vector3.Lerp(p4, p5, time);

				time += Time.deltaTime * Setting.AttackSpeed;

				yield return null;
			}
		}
		#endregion
	}

}
