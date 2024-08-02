using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// DCL_Weapon_Sword
// 근접무기 - 검 클래스
// 
//
//	Level_0	좌 우 공격
//	Level_2 좌 우 동시공격
//	Level_4 회전
//	Level_6 회전 범위 증가, 검 2개 회전
//	Level_8 검기 발사
//	Level_10
//
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

        #region [Nested] SwordSetting
        [Serializable]
        public class NSwordSetting
        {
            public GameObject[] SwordLevelRoot;
        }
        public NSwordSetting SwordSetting = new NSwordSetting();
        #endregion

        #region [Sword] Level_1
        [Serializable]
        public class NSword_Level_1
        {
            public Transform _target;
            public Transform _p1, _p2, _p3;
        }
        public NSword_Level_1 Sword_Lv1 = new NSword_Level_1();
        #endregion

        #region [Sword] Level_2
        [Serializable]
        public class NSword_Level_2
        {
            public Transform _target_1, _target_2;
            public Transform _p1, _p2_1, _p2_2, _p3;
        }
        public NSword_Level_2 Sword_Lv2 = new NSword_Level_2();
        #endregion

        #region [Sword] Level_3
        [Serializable]
        public class NSword_Level_3
        {
            public Transform[] _target;
            public float Speed;
            public float Radius; //반지름
            public float Degree; //각도
        }
        public NSword_Level_3 Sword_Lv3 = new NSword_Level_3();
        #endregion


        #region [Sword] Level_4
        [Serializable]
        public class NSword_Level_4
        {
            public Transform[] _target;
            public float Speed;
            public float InsideRad, OutsideRad; //반지름
            public float Degree; //각도
        }
        public NSword_Level_4 Sword_Lv4 = new NSword_Level_4();
        #endregion

        #region [Sword] Level_5
        [Serializable]
        public class NSword_Level_5
        {
            // 회전
            public Transform[] _target;
            public float Speed;
            public float InsideRad, OutsideRad; //반지름
            public float Degree; //각도

            // 검기
            public GameObject SwordAura;
        }
        public NSword_Level_5 Sword_Lv5 = new NSword_Level_5();
        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] PlayerTransform
        #endregion

        #region [Variable] Rotation
        public Quaternion InitRotation;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ChangeNowWeaponLevel(WeaponLevel level) => Setting.NowWeaponLevel = level;
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
            InitRotation = transform.localRotation;

            // 무기 레벨 1로 초기화
            Setting.NowWeaponLevel = WeaponLevel.LEVEL_1;
            StartCoroutine(Sword_Level_1());
            //StartCoroutine(Sword_Level_2());
            //StartCoroutine(Sword_Level_3());
            //StartCoroutine(Sword_Level_4());
            //StartCoroutine(Sword_Level_5());


        }
        #endregion

        #region [Update] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public new void Update()
        {
            PlayerPos = GameObject.FindWithTag("Player").transform;

            transform.localRotation = InitRotation;
            transform.rotation = InitRotation;
            //transform.position = PlayerPos.position;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Attack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Attack] ChangeWeapon_SwordForm
        public void ChangeWeapon_SwordForm()
        {
            switch (Setting.NowWeaponLevel)
            {
                case WeaponLevel.LEVEL_1:
                    break;
                case WeaponLevel.LEVEL_2:
                    break;
                case WeaponLevel.LEVEL_3:
                    break;
                case WeaponLevel.LEVEL_4:
                    break;
                case WeaponLevel.LEVEL_5:
                    break;
                case WeaponLevel.LEVEL_6:
                    break;
                case WeaponLevel.LEVEL_7:
                    break;
                case WeaponLevel.LEVEL_8:
                    break;
                case WeaponLevel.LEVEL_9:
                    break;
                case WeaponLevel.LEVEL_10:
                    break;
            }
        }

        #endregion

        #region [Attack] Sword_Level_1
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerator Sword_Level_1(float duration = 1.0f)
        {
            float time = 0f;
            bool flip = false;
            Vector3 p2localpos = Sword_Lv1._p2.localPosition;

            while (true)
            {
                if (time > 1)
                {
                    //ChangeAttackState(AttackState.COOLTIME);
                    if (time > 1 + Setting.AttackCoolTime)
                    {
                        time = 0f;
                       // ChangeAttackState(AttackState.ATTACKING);
                        if (flip == false)
                        {
                            flip = true;
                            Sword_Lv1._p2.localPosition = new Vector3(p2localpos.x * -1, p2localpos.y, p2localpos.z);
                        }   
                        else
                        {
                            flip = false;
                            Sword_Lv1._p2.localPosition = new Vector3(p2localpos.x, p2localpos.y, p2localpos.z);
                        }
                    }
                }

                Vector3 p4 = Vector3.Lerp(Sword_Lv1._p1.position, Sword_Lv1._p2.position, time);
                Vector3 p5 = Vector3.Lerp(Sword_Lv1._p2.position, Sword_Lv1._p3.position, time);
                Sword_Lv1._target.position = Vector3.Lerp(p4, p5, time);

                time += Time.deltaTime * Setting.AttackSpeed;

                yield return null;
            }
        }
        #endregion

        #region [Attack] Sword_Level_2
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerator Sword_Level_2(float duration = 1.0f)
        {
            float time = 0f;

            while (true)
            {
                if (time > 1)
                {
                    ChangeAttackState(AttackState.COOLTIME);
                    if (time > 1 + Setting.AttackCoolTime)
                    {
                        time = 0f;
                        ChangeAttackState(AttackState.ATTACKING);
                    }
                }

                Vector3 p4 = Vector3.Lerp(Sword_Lv2._p1.position, Sword_Lv2._p2_1.position, time);
                Vector3 p5 = Vector3.Lerp(Sword_Lv2._p2_1.position, Sword_Lv2._p3.position, time);
                Vector3 p6 = Vector3.Lerp(Sword_Lv2._p3.position, Sword_Lv2._p2_2.position, time);
                Vector3 p7 = Vector3.Lerp(Sword_Lv2._p2_2.position, Sword_Lv2._p1.position, time);
                Sword_Lv2._target_1.position = Vector3.Lerp(p4, p5, time);
                Sword_Lv2._target_2.position = Vector3.Lerp(p6, p7, time);

                time += Time.deltaTime * Setting.AttackSpeed;

                yield return null;
            }
        }
        #endregion

        #region [Attack] Sword_Level_3
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerator Sword_Level_3(float duration = 1.0f)
        {
            //float time = 0f;

            while (true)
            {
                Sword_Lv3.Degree += Time.deltaTime * Sword_Lv3.Speed;
                if (Sword_Lv3.Degree < 360)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var rad = Mathf.Deg2Rad * (Sword_Lv3.Degree + i * (360 / 3));
                        var x = Sword_Lv3.Radius * Mathf.Sin(rad);
                        var z = Sword_Lv3.Radius * Mathf.Cos(rad);
                        Sword_Lv3._target[i].position = transform.position + new Vector3(x, 0, z);
                        Sword_Lv3._target[i].rotation = Quaternion.Euler(0, 0, ((Sword_Lv3.Degree + i * (360 / 3))) * -1); //가운데를 바라보게 각도 조절
                    }
                }
                else
                {
                    Sword_Lv3.Degree = 0;
                }

                yield return null;
            }
        }
        #endregion

        #region [Attack] Sword_Level_4
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerator Sword_Level_4(float duration = 1.0f)
        {
            //float time = 0f;

            while (true)
            {
                Sword_Lv4.Degree += Time.deltaTime * Sword_Lv4.Speed;
                if (Sword_Lv4.Degree < 360)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var rad = Mathf.Deg2Rad * (Sword_Lv4.Degree + i * (360 / 6));
                        if (i % 2 == 0)
                        {
                            var x = Sword_Lv4.InsideRad * Mathf.Sin(rad);
                            var z = Sword_Lv4.InsideRad * Mathf.Cos(rad);
                            Sword_Lv4._target[i].position = transform.position + new Vector3(x, 0, z);
                            Sword_Lv4._target[i].rotation = Quaternion.Euler(0, 0, ((Sword_Lv4.Degree + i * (360 / 6))) * -1); //가운데를 바라보게 각도 조절
                        }
                        else
                        {
                            var x = Sword_Lv4.OutsideRad * Mathf.Sin(rad);
                            var z = Sword_Lv4.OutsideRad * Mathf.Cos(rad);
                            Sword_Lv4._target[i].position = transform.position + new Vector3(z, 0, x);
                            Sword_Lv4._target[i].rotation = Quaternion.Euler(0, 0, ((Sword_Lv4.Degree + i * (360 / 6))) * -1); //가운데를 바라보게 각도 조절
                        }
                    }
                }
                else
                {
                    Sword_Lv4.Degree = 0;
                }

                yield return null;
            }
        }
        #endregion

        #region [Attack] Sword_Level_5
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerator Sword_Level_5(float duration = 1.0f)
        {
            float time = 0f;

            while (true)
            {
                // 플레이어 바라보는방향 검기 발사
                if (time > 2)
                {
                    ChangeAttackState(AttackState.COOLTIME);
                    if (time > 2 + Setting.AttackCoolTime)
                    {
                        time = 0f;
                        ChangeAttackState(AttackState.ATTACKING);
                        GameObject InstantMon = Instantiate(Sword_Lv5.SwordAura, PlayerPos.position, PlayerPos.rotation);
                    }
                }

                time += Time.deltaTime * Setting.AttackSpeed;

                // 검 회전 공격
                Sword_Lv5.Degree += Time.deltaTime * Sword_Lv5.Speed;
                if (Sword_Lv5.Degree < 360)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var rad = Mathf.Deg2Rad * (Sword_Lv5.Degree + i * (360 / 6));
                        if (i % 2 == 0)
                        {
                            var x = Sword_Lv5.InsideRad * Mathf.Sin(rad);
                            var z = Sword_Lv5.InsideRad * Mathf.Cos(rad);
                            Sword_Lv5._target[i].position = transform.position + new Vector3(x, 0, z);
                            Sword_Lv5._target[i].rotation = Quaternion.Euler(0, 0, ((Sword_Lv5.Degree + i * (360 / 6))) * -1); //가운데를 바라보게 각도 조절
                        }
                        else
                        {
                            var x = Sword_Lv5.OutsideRad * Mathf.Sin(rad);
                            var z = Sword_Lv5.OutsideRad * Mathf.Cos(rad);
                            Sword_Lv5._target[i].position = transform.position + new Vector3(z, 0, x);
                            Sword_Lv5._target[i].rotation = Quaternion.Euler(0, 0, ((Sword_Lv5.Degree + i * (360 / 6))) * -1); //가운데를 바라보게 각도 조절
                        }
                    }
                }
                else
                {
                    Sword_Lv5.Degree = 0;
                }

                yield return null;
            }
        }
        #endregion

    }

}
