using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Monster_BomberMan
    // 몬스터 - 폭탄마 클래스
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_Monster_BomberMan : DCL_MonsterBase
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
        public GameObject[] BodyMesh;
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
            //for(int i = 0; i < 5; i++)
            //{
            //    DCL_Status statbase = new DCL_Status();
            //    statbase.Attack_Power = 1f;
            //    statbase.Attack_Speed = 1f;
            //    statbase.Cri_Percent = 0;
            //    statbase.Defense= 5f;
            //    statbase.Critical_Damage= 100f;
            //    statbase.HP = 10f;
            //    statbase.HP_Recovery = 0f;
            //    statbase.Gain_Range = 0f;
            //    statbase.Move_Speed = 1f;
            //    Setting.Monster_Status.Add(statbase);
            //}
            //Json_Utility_Extend.FileSaveList<DCL_Status>(Setting.Monster_Status, "Data/Json_Data/Monster/Monster_BomberMan.Json");
            Setting.Monster_Status = Json_Utility_Extend.FileLoadList<DCL_Status>("Data/Json_Data/Monster/Monster_BomberMan.Json");
        }
        #endregion

        #region [Init] start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            // 스텟 임시 초기화
            Setting.Monster_State = MonsterState.MOVING;

            Attack_Range = 10f;
            ChargingTime = 3f;
            ingTime = 0;

            FireEffect.SetActive(false);
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


        #region [FSM] 몬스터 패턴
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
                    if(Charging())
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
            // Y값 보정 - 테스트 위해
            //Vector3 Pos = new Vector3(PlayerPos.position.x, 0, PlayerPos.position.z);

            Vector3 dir = SetDirection(transform.position, PlayerPos.position);

            transform.position += -dir * Mon_Status.Move_Speed * Time.deltaTime;

            transform.LookAt(PlayerPos);
        }
        #endregion

        #region [Move] Monster_Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public bool Charging()
        {
            // Y값 보정 - 테스트 위해
            //Vector3 Pos = new Vector3(PlayerPos.position.x, 0, PlayerPos.position.z);

            Vector3 dir = SetDirection(transform.position, PlayerPos.position);

            transform.position += -dir * Mon_Status.Move_Speed / 2 * Time.deltaTime;

            transform.LookAt(PlayerPos);

            ingTime += Time.deltaTime;
            if (ChargingTime < ingTime)
            {
                ingTime = 0;
                foreach(GameObject data in BodyMesh)
                {
                    data.SetActive(false);
                }
                FireEffect.SetActive(true);
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
            // 이때 충돌처리
            ingTime += Time.deltaTime;
            if (1 < ingTime)
            {
                ingTime = 0;
                Setting.Monster_State = MonsterState.DIE;
                Death();
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Collider
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collider] Monster_Hit
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Hit()
        {

        }
        #endregion

        #region [Collider] OnTriggerEnter
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.name == "Collider" ||
                coll.gameObject.name == "MagicBall")
            {
                Death();
            }
        }
        #endregion

        #region [Collider] 몬스터 겹침 방지
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