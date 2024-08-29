using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// DCL_MonsterBase
// 몬스터 베이스 클래스
//
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

namespace HSM.Game
{
    public class DCL_MonsterBase : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // enum Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum MonsterState { MOVING, CHARGING, ATTACK, DIE }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NSetting
        {
            public List<DCL_Status> Monster_Status = new List<DCL_Status>();     // 몬스터 스텟
            public Collider Monster_OverLapColl;                     // 충돌 방지 
            public MonsterState Monster_State;                       // 몬스터 상태
            public int Monster_Level;                                // 몬스터 레벨	
            public GameObject EXP;                                   // 경험치 프리펩
        }
        public NSetting Setting = new NSetting();
        #endregion

        #region [Nested] NaviMesh
        public class NNavMeshSetting
        {
            public NavMeshAgent Agent;
        }
        public NNavMeshSetting NavSetting = new NNavMeshSetting();
        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public Transform PlayerPos;
        #endregion

        #region [Variable]
        private float NowHP;
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public DCL_Status Mon_Status => Setting.Monster_Status[Setting.Monster_Level];       // 플레이어 스텟
        public NavMeshAgent Nav_Agent => NavSetting.Agent;
        public void SetMonsterLevel(int lv) => Setting.Monster_Level = lv;
        #endregion




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            PlayerPos = GameObject.FindWithTag("Player").transform;
            NavSetting.Agent = this.GetComponent<NavMeshAgent>();
            NowHP = Mon_Status.HP;

        }
        #endregion

        #region [Init] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Update()
        {
            //PlayerPos = GameObject.FindWithTag("Player").transform;
        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Move
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Move] Monster_Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Move()
        {

        }
        #endregion

        #region [Move] SetDestination
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public Vector3 SetDirection(Vector3 me, Vector3 target)
        {
            return (me - target).normalized;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Attack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Move] Monster_Attack
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Attack()
        {

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Hit
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Move] Monster_Hit
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Hit(float att)
        {
            NowHP -= att;
            if (NowHP <= 0)
                Death();
        }
        #endregion

        public void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.name == "Collider" ||
            //    other.gameObject.name == "MagicBall(Clone)")
            //{
            //    Death();
            //}
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Death
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Move] Monster_Death
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public virtual void Death()
        {
            GameObject exp = Instantiate(Setting.EXP, transform.position, Quaternion.identity);
            exp.GetComponent<DCL_Item_EXP>().SetEXP(3, 1);
        }
        #endregion

    }

}
