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
            public float AttcatCoolTime;
            public float IngTime;
            public int MagicBallCount;
            public int GuidedMagicBallCount;
            public int Level;
            public SphereCollider TargetCheckColl;
        }
        public NStaffSetting StaffSetting = new NStaffSetting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Direction;
        public Vector3[] Ball_Direction;
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

            Ball_Direction = new Vector3[]
            {
                transform.right,
                transform.right,
                transform.forward,
                transform.forward,
                -transform.forward,
                -transform.forward,
                -transform.right,
                -transform.right,
            };
        }
        #endregion

        #region [Update] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public new void Update()
        {
            base.Update();
            SwitchStaffLevel_Status(Setting.NowWeaponLevel);
            SwitchStaffLevel(Setting.NowWeaponLevel);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Attack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Attack] Switch Level
        public void SwitchStaffLevel(WeaponLevel lv)
        {
            switch (lv)
            {
                case WeaponLevel.LEVEL_1:
                    Staff_Level_1();
                    break;
                case WeaponLevel.LEVEL_2:
                    Staff_Level_1();
                    break;
                case WeaponLevel.LEVEL_3:
                    Staff_Level_2();
                    break;
                case WeaponLevel.LEVEL_4:
                    Staff_Level_2();
                    break;
                case WeaponLevel.LEVEL_5:
                    Staff_Level_3();
                    break;
                case WeaponLevel.LEVEL_6:
                    Staff_Level_3();
                    break;
                case WeaponLevel.LEVEL_7:
                    Staff_Level_4();
                    break;
                case WeaponLevel.LEVEL_8:
                    Staff_Level_4();
                    break;
                case WeaponLevel.LEVEL_9:
                    Staff_Level_5();
                    break;
                case WeaponLevel.LEVEL_10:
                    Staff_Level_5();
                    break;
            }
        }
        #endregion

        #region [Attack] Switch Level
        public void SwitchStaffLevel_Status(WeaponLevel lv)
        {
            switch (lv)
            {
                case WeaponLevel.LEVEL_1:
                    // 바라보는 방향 구체 발사
                    StaffSetting.AttcatCoolTime = 1;
                    break;
                case WeaponLevel.LEVEL_2:
                    // 공격력 증가
                    break;
                case WeaponLevel.LEVEL_3:
                    // 구체 개수 증가
                    StaffSetting.AttcatCoolTime = 1;
                    StaffSetting.MagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_4:
                    // 공격 속도 증가
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_5:
                    // 유도탄 추가
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 2;
                    StaffSetting.GuidedMagicBallCount = 1;
                    break;
                case WeaponLevel.LEVEL_6:
                    // 유도탄 개수 증가
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 2;
                    StaffSetting.GuidedMagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_7:
                    // 발사 방향, 크기 증가
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 4;
                    StaffSetting.GuidedMagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_8:
                    // 발사 방향증가
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 8;
                    StaffSetting.GuidedMagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_9:
                    // 유도탄 끝사거리 폭발
                    break;
                case WeaponLevel.LEVEL_10:
                    // 폭발 반경 증가
                    break;
            }
        }
        #endregion

        #region [Attack] Staff_Level_1
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_1()
        {
            if (StaffSetting.IngTime > StaffSetting.AttcatCoolTime)
            {
                StaffSetting.IngTime = 0f;
                GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, ATSetting.ATTransform.position, PlayerPos.rotation);
                // 매직볼 셋팅
                InstantMagicball.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
            }

            StaffSetting.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        #region [Attack] Staff_Level_2
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_2()
        {
            if (StaffSetting.IngTime > StaffSetting.AttcatCoolTime)
            {
                float separationDistance = 0.5f; // 총알 간의 거리
                Vector3[] offset = new Vector3[2];
                offset[0] = transform.right * separationDistance / 2;
                offset[1] = transform.right * -separationDistance / 2;

                StaffSetting.IngTime = 0f;
                for (int i = 0; i < StaffSetting.MagicBallCount; i++)
                {
                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, ATSetting.ATTransform.position + offset[i], PlayerPos.rotation);
                    // 매직볼 셋팅
                    InstantMagicball.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                }
            }

            StaffSetting.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        #region [Attack] Staff_Level_3
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_3()
        {
            if (StaffSetting.IngTime > StaffSetting.AttcatCoolTime)
            {
                float separationDistance = 0.5f; // 총알 간의 거리
                Vector3[] offset = new Vector3[2];
                offset[0] = transform.right * separationDistance / 2;
                offset[1] = transform.right * -separationDistance / 2;

                StaffSetting.IngTime = 0f;
                for (int i = 0; i < StaffSetting.MagicBallCount; i++)
                {
                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, ATSetting.ATTransform.position + offset[i], PlayerPos.rotation);
                    // 매직볼 셋팅
                    InstantMagicball.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                }

                for (int i = 0; i < StaffSetting.GuidedMagicBallCount; i++)
                {
                    // 타겟 발사
                    GameObject InstantMagicball_target = Instantiate(StaffSetting.MagicBall, PlayerPos.position, PlayerPos.rotation);
                    TempTarget = FindEnemy();
                    if (TempTarget == null)
                    {
                        InstantMagicball_target.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                        InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                    }
                    else
                        InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().Set_Target(TempTarget, PlayerStatus.Attack_Power);
                }
            }
            StaffSetting.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        #region [Attack] Staff_Level_4
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_4()
        {
            if (StaffSetting.IngTime > StaffSetting.AttcatCoolTime)
            {
                Vector3[] offset = new Vector3[StaffSetting.MagicBallCount];
                float separationDistance = 0.5f; // 총알 간의 거리
                StaffSetting.IngTime = 0f;
                Vector3[] dirction = new Vector3[]
                {
                    Quaternion.Euler(0, 0, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 0, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 180, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 180, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 90, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 90, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, -90, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, -90, 0) * PlayerAttackDirection
                };

                for (int i = 0; i < StaffSetting.MagicBallCount; i++)
                {
                    if (i % 2 == 0)
                        offset[i] = Ball_Direction[i] * separationDistance / 2;
                    else
                        offset[i] = Ball_Direction[i] * -separationDistance / 2;

                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, ATSetting.ATTransform.position + offset[i], PlayerPos.rotation);
                    // 매직볼 셋팅
                    InstantMagicball.transform.rotation = Quaternion.LookRotation(dirction[i]);
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                }

                for (int i = 0; i < StaffSetting.GuidedMagicBallCount; i++)
                {
                    // 타겟 발사
                    GameObject InstantMagicball_target = Instantiate(StaffSetting.MagicBall, PlayerPos.position, PlayerPos.rotation);
                    TempTarget = FindEnemy();
                    if (TempTarget == null)
                    {
                        InstantMagicball_target.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                        InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                    }
                    else
                        InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().Set_Target(TempTarget, PlayerStatus.Attack_Power);
                }
            }

            StaffSetting.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        #region [Attack] Staff_Level_5
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Staff_Level_5()
        {
            if (StaffSetting.IngTime > StaffSetting.AttcatCoolTime)
            {
                Vector3[] offset = new Vector3[StaffSetting.MagicBallCount];
                float separationDistance = 0.5f; // 총알 간의 거리
                StaffSetting.IngTime = 0f;
                Vector3[] dirction = new Vector3[]
                {
                    Quaternion.Euler(0, 0, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 0, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 180, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 180, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 90, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, 90, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, -90, 0) * PlayerAttackDirection,
                    Quaternion.Euler(0, -90, 0) * PlayerAttackDirection
                };

                for (int i = 0; i < StaffSetting.MagicBallCount; i++)
                {
                    if (i % 2 == 0)
                        offset[i] = Ball_Direction[i] * separationDistance / 2;
                    else
                        offset[i] = Ball_Direction[i] * -separationDistance / 2;

                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, ATSetting.ATTransform.position + offset[i], PlayerPos.rotation);
                    // 매직볼 셋팅
                    InstantMagicball.transform.rotation = Quaternion.LookRotation(dirction[i]);
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                }

                for (int i = 0; i < StaffSetting.GuidedMagicBallCount; i++)
                {
                    // 타겟 발사
                    GameObject InstantMagicball_target = Instantiate(StaffSetting.MagicBall, PlayerPos.position, PlayerPos.rotation);
                    TempTarget = FindEnemy();
                    if (TempTarget == null)
                    {
                        InstantMagicball_target.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                        InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                    }
                    else
                    {
                        InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().Set_Target(TempTarget, PlayerStatus.Attack_Power);
                        InstantMagicball_target.GetComponent<DCL_Staff_MagicBall>().SetBomb(true);
                    }
                }
            }

            StaffSetting.IngTime += Time.deltaTime * Setting.AttackSpeed;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Find Enemy
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Find]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public Transform FindEnemy()
        {
            Collider[] hitColliders = Physics.OverlapSphere(StaffSetting.TargetCheckColl.bounds.center, StaffSetting.TargetCheckColl.radius);

            List<Transform> targetdata = new List<Transform>();
            foreach (Collider data in hitColliders)
            {
                if (data.transform.GetComponent<DCL_MonsterBase>() != null)
                {
                    targetdata.Add(data.transform);
                }
            }

            if (targetdata.Capacity == 0)
                return null;
            else
            {
                Transform closestTransform = null;
                float closestDistance = Mathf.Infinity;

                foreach (Transform target in targetdata)
                {
                    float distance = Vector3.Distance(transform.position, target.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTransform = target;
                    }
                }

                return closestTransform;
            }
        }
        #endregion

    }

}
