using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// DCL_Weapon_Staff
// ���Ÿ� ���� - ������ Ŭ����
// 
//
//	Level_0	�ٶ󺸴� ���� �������� �߻� -> �������� ���� ����
//	Level_2 �����¿� 4���� �ڵ� �߻�
//	Level_4 �ٶ󺸴¹��� ����ź���� ����
//	Level_6 ����ź ����, ��Ÿ� ����, 
//	Level_8 �� ��Ÿ� �����ϸ� ����
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
                    // �ٶ󺸴� ���� ��ü �߻�
                    StaffSetting.AttcatCoolTime = 1;
                    break;
                case WeaponLevel.LEVEL_2:
                    // ���ݷ� ����
                    break;
                case WeaponLevel.LEVEL_3:
                    // ��ü ���� ����
                    StaffSetting.AttcatCoolTime = 1;
                    StaffSetting.MagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_4:
                    // ���� �ӵ� ����
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_5:
                    // ����ź �߰�
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 2;
                    StaffSetting.GuidedMagicBallCount = 1;
                    break;
                case WeaponLevel.LEVEL_6:
                    // ����ź ���� ����
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 2;
                    StaffSetting.GuidedMagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_7:
                    // �߻� ����, ũ�� ����
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 4;
                    StaffSetting.GuidedMagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_8:
                    // �߻� ��������
                    StaffSetting.AttcatCoolTime = 0.8f;
                    StaffSetting.MagicBallCount = 8;
                    StaffSetting.GuidedMagicBallCount = 2;
                    break;
                case WeaponLevel.LEVEL_9:
                    // ����ź ����Ÿ� ����
                    break;
                case WeaponLevel.LEVEL_10:
                    // ���� �ݰ� ����
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
                // ������ ����
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
                float separationDistance = 0.5f; // �Ѿ� ���� �Ÿ�
                Vector3[] offset = new Vector3[2];
                offset[0] = transform.right * separationDistance / 2;
                offset[1] = transform.right * -separationDistance / 2;

                StaffSetting.IngTime = 0f;
                for (int i = 0; i < StaffSetting.MagicBallCount; i++)
                {
                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, ATSetting.ATTransform.position + offset[i], PlayerPos.rotation);
                    // ������ ����
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
                float separationDistance = 0.5f; // �Ѿ� ���� �Ÿ�
                Vector3[] offset = new Vector3[2];
                offset[0] = transform.right * separationDistance / 2;
                offset[1] = transform.right * -separationDistance / 2;

                StaffSetting.IngTime = 0f;
                for (int i = 0; i < StaffSetting.MagicBallCount; i++)
                {
                    GameObject InstantMagicball = Instantiate(StaffSetting.MagicBall, ATSetting.ATTransform.position + offset[i], PlayerPos.rotation);
                    // ������ ����
                    InstantMagicball.transform.rotation = Quaternion.LookRotation(PlayerAttackDirection);
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                }

                for (int i = 0; i < StaffSetting.GuidedMagicBallCount; i++)
                {
                    // Ÿ�� �߻�
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
                float separationDistance = 0.5f; // �Ѿ� ���� �Ÿ�
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
                    // ������ ����
                    InstantMagicball.transform.rotation = Quaternion.LookRotation(dirction[i]);
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                }

                for (int i = 0; i < StaffSetting.GuidedMagicBallCount; i++)
                {
                    // Ÿ�� �߻�
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
                float separationDistance = 0.5f; // �Ѿ� ���� �Ÿ�
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
                    // ������ ����
                    InstantMagicball.transform.rotation = Quaternion.LookRotation(dirction[i]);
                    InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
                }

                for (int i = 0; i < StaffSetting.GuidedMagicBallCount; i++)
                {
                    // Ÿ�� �߻�
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
