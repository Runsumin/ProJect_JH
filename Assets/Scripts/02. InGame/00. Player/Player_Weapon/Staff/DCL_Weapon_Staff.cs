using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// DCL_Weapon_Staff
// 원거리 무기 - 지팡이 클래스
// 
//
//	Level_0	바라보는 방향 에너지볼 발사 -> 에너지볼 개수 증가
//	Level_2 상하좌우 4방향 자동 발사
//	Level_4 바라보는방향 유도탄으로 변경
//	Level_6 유도탄 개수, 사거리 증가, 
//	Level_8 끝 사거리 도달하면 폭발
//	Level_10
//
//
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

namespace HSM.Game
{
    public class DCL_Weapon_Staff : DCL_WeaponBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] StaffSetting
        [Serializable]
        public class NStaffSetting
        {
            public GameObject[] StaffLevelRoot;
            public GameObject MagicBall;
        }
        public NStaffSetting StaffSetting = new NStaffSetting();
        #endregion

        #region [Staff] Level_1
        [Serializable]
        public class NStaff_Level_1
        {
            public float AttcatCoolTime;
            public float IngTime;
            public Transform[] ShootingPoint;
        }
        public NStaff_Level_1 Staff_Lv_1 = new NStaff_Level_1();
        #endregion

        #region [Staff] Level_2
        [Serializable]
        public class NStaff_Level_2
        {
            public float AttcatCoolTime;
            public float IngTime;
            public int MagicBallCount;
            public Transform[] ShootingPoint;
        }
        public NStaff_Level_2 Staff_Lv_2 = new NStaff_Level_2();
        #endregion

        #region [Staff] Level_3
        [Serializable]
        public class NStaff_Level_3
        {
            public float AttcatCoolTime;
            public float IngTime;
            public int MagicBallCount;
            public Transform[] ShootingPoint;
        }
        public NStaff_Level_3 Staff_Lv_3 = new NStaff_Level_3();
        #endregion

        #region [Staff] Level_4
        [Serializable]
        public class NStaff_Level_4
        {
            public float AttcatCoolTime;
            public float IngTime;
            public int MagicBallCount;
            public Transform[] ShootingPoint;
        }
        public NStaff_Level_4 Staff_Lv_4 = new NStaff_Level_4();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Direction;
        public Vector3[] Ball_Direction = { Vector3.forward, Vector3.forward, Vector3.right, Vector3.right, Vector3.left, Vector3.left, Vector3.back, Vector3.back };
        public Transform TempTarget;
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
            Setting.Weapon_AttackType = AttackType.RANGE;
            Setting.Weapon_AttackState = AttackState.COOLTIME;

            // Staff_lv1 초기화
            Staff_Lv_1.AttcatCoolTime = 1;

            // Staff_lv2 초기화
            Staff_Lv_2.AttcatCoolTime = 1;
            Staff_Lv_2.MagicBallCount = 2;

            // Staff_lv3 초기화
            Staff_Lv_3.AttcatCoolTime = 1;
            Staff_Lv_3.MagicBallCount = 8;
            // Staff_lv4 초기화
            Staff_Lv_4.AttcatCoolTime = 0.5f;
            Staff_Lv_4.MagicBallCount = 8;
        }
        #endregion

        #region [Update] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public new void Update()
        {
            base.Update();
            Staff_Level_1();
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Attack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Attack] Staff_Level_1
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_1()
        {
            if (Staff_Lv_1.IngTime > Staff_Lv_1.AttcatCoolTime)
            {
                Staff_Lv_1.IngTime = 0f;
                GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, Staff_Lv_1.ShootingPoint[0].position, PlayerPos.rotation);
                // 매직볼 셋팅
                InstantMagicball.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward);
            }

            Staff_Lv_1.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        #region [Attack] Staff_Level_2
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_2()
        {
            if (Staff_Lv_2.IngTime > Staff_Lv_2.AttcatCoolTime)
            {
                Staff_Lv_2.IngTime = 0f;
                for (int i = 0; i < Staff_Lv_2.MagicBallCount; i++)
                {
                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, Staff_Lv_2.ShootingPoint[i].position, PlayerPos.rotation);
                    // 매직볼 셋팅
                    InstantMagicball.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward);
                }
            }

            Staff_Lv_2.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        #region [Attack] Staff_Level_3
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_3()
        {
            if (Staff_Lv_3.IngTime > Staff_Lv_3.AttcatCoolTime)
            {
                Staff_Lv_3.IngTime = 0f;
                for (int i = 0; i < Staff_Lv_3.MagicBallCount; i++)
                {
                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, Staff_Lv_3.ShootingPoint[i].position, PlayerPos.rotation);
                    // 매직볼 셋팅
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Ball_Direction[i]);
                }
            }

            Staff_Lv_3.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        #region [Attack] Staff_Level_4
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_4()
        {
            if (Staff_Lv_4.IngTime > Staff_Lv_4.AttcatCoolTime)
            {
                Staff_Lv_4.IngTime = 0f;
                // 8방향 발사
                for (int i = 0; i < Staff_Lv_4.MagicBallCount; i++)
                {
                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, Staff_Lv_4.ShootingPoint[i].position, PlayerPos.rotation);
                    // 매직볼 셋팅
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Ball_Direction[i]);
                }
                // 타겟 발사
                GameObject InstantMagicball_target = Instantiate(StaffSetting.MagicBall, PlayerPos.position, PlayerPos.rotation);
                // 매직볼 셋팅
                InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().Set_Target(TempTarget);
            }

            Staff_Lv_4.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        #region [Attack] Staff_Level_5
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_5()
        {
            if (Staff_Lv_4.IngTime > Staff_Lv_4.AttcatCoolTime)
            {
                Staff_Lv_4.IngTime = 0f;
                // 8방향 발사
                for (int i = 0; i < Staff_Lv_4.MagicBallCount; i++)
                {
                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, Staff_Lv_4.ShootingPoint[i].position, PlayerPos.rotation);
                    // 매직볼 셋팅
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Ball_Direction[i]);
                }
                // 타겟 발사
                GameObject InstantMagicball_target = Instantiate(StaffSetting.MagicBall, PlayerPos.position, PlayerPos.rotation);
                // 매직볼 셋팅
                InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().Set_Target(TempTarget);
            }

            Staff_Lv_4.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion
    }

}
