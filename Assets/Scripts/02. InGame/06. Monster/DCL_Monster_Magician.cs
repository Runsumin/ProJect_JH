using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Monster_BomberMan
    // ���� - ��ź�� Ŭ����
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_Monster_Magician : DCL_MonsterBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public float Attack_Range;
        public float ChargingTime;
        public float ingTime;
        public GameObject FireEffect;
        public Transform AttackPoint;
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

        #region [Init] Awake
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            //Json_Utility_Extend.FileSave(Mon_Status, "Data/Json_Data/Monster/Monster_Magicion.Json");
            Setting.Monster_Status = Json_Utility_Extend.FileLoadList<DCL_Status>("Data/Json_Data/Monster/Monster_Magicion.Json");
        }
        #endregion

        #region [Init] start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            // ���� �ӽ� �ʱ�ȭ
            Setting.Monster_State = MonsterState.MOVING;

            Attack_Range = 10f;
            ChargingTime = 1f;
            ingTime = 0;
        }
        #endregion

        #region [Init] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            Mon_BomberMan_FSM();
        }
        #endregion


        #region [FSM] ���� ����
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Mon_BomberMan_FSM()
        {
            switch (Setting.Monster_State)
            {
                case MonsterState.MOVING:
                    if (Check_AttackRange())
                        Setting.Monster_State = MonsterState.CHARGING;
                    else
                        Move();
                    break;
                case MonsterState.CHARGING:
                    if (Charging())
                        Setting.Monster_State = MonsterState.ATTACK;
                    break;
                case MonsterState.ATTACK:
                    Attack();
                    break;
                case MonsterState.DIE:
                    break;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Move
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Move] Check AttackRange
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public bool Check_AttackRange()
        {
            Vector3 dirvec = transform.position - PlayerPos.position;
            float length = Mathf.Sqrt(Mathf.Pow(dirvec.x, 2) + Mathf.Pow(dirvec.y, 2) + Mathf.Pow(dirvec.z, 2));

            if (Attack_Range < length)
                return false;
            else
                return true;
        }
        #endregion

        #region [Move] Monster_Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Move()
        {
            //Ver_1 Default
            // Y�� ���� - �׽�Ʈ ����
            //Vector3 Pos = new Vector3(PlayerPos.position.x, 0, PlayerPos.position.z);

            //Vector3 dir = SetDirection(transform.position, PlayerPos.position);

            //transform.position += -dir * Mon_Status.Move_Speed * Time.deltaTime;

            // Ver_2 NavMesh
            Nav_Agent.speed = Mon_Status.Move_Speed;
            Nav_Agent.SetDestination(PlayerPos.position);
            transform.LookAt(PlayerPos);
        }
        #endregion

        #region [Move] Monster_Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public bool Charging()
        {
            //transform.LookAt(PlayerPos);
            ingTime += Time.deltaTime;
            if (ChargingTime < ingTime)
            {
                ingTime = 0;
                return true;
            }
            else
                return false;
        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Attack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Attack] Monster_Attack
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Attack()
        {
            // �̶� �浹ó��
            Vector3 dir = SetDirection(PlayerPos.position, transform.position);

            GameObject InstantMagicball = Instantiate(FireEffect, AttackPoint.position, AttackPoint.rotation);
            // ������ ����
            InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward);
            Setting.Monster_State = MonsterState.MOVING;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Collider
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collider] Monster_Hit
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Hit(float att)
        {
            base.Hit(att);
        }
        #endregion

        public new void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.name == "Collider" ||
                coll.gameObject.name == "MagicBall")
            {
                Death();
            }
        }

        #region [Collider] ���� ��ħ ����
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //public override void Hit()
        //{

        //}
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Death
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collider] Monster_Death
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Death()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}
