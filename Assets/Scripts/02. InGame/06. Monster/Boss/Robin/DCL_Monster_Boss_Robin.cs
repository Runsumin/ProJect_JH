using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Monster_Boss_Robin
    // 보스몬스터 - 로빈
    //
    //  패턴 4종 구현
    //  - 기본공격
    //  - 충전공격
    //  - 화살비
    //  - 함정설치
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class DCL_Monster_Boss_Robin : DCL_MonsterBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Enum] Animation State
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public enum eRobinState { IDLE, RUN, ATTACK, ATTACK_END }
        public enum eRobinPattern { NOMALATTACK, CHARGEATTACK, RAINARROW, TRAP }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class Boss_Robin_Setting
        {
            public DecalProjector Box_Effect;
            public DecalProjector Circle_Effect;
            public GameObject Model;
            public float AttactCoolTime;
            public float AttackRangeSetting;
        }
        public Boss_Robin_Setting Robin_Setting = new Boss_Robin_Setting();
        #endregion

        #region [NestedClass] Effect
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class Boss_Robin_Effect
        {
            public Transform Root;
            public GameObject NormalAttack_Effect;
        }
        public Boss_Robin_Effect Robin_Effect = new Boss_Robin_Effect();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Animation State
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public eRobinState Robin_State;
        #endregion
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        #region [Variable] Animation
        protected Animator RobinAnimation;
        protected eRobinState BeForeRobinState = eRobinState.IDLE;   // 이전 행동
        protected eRobinState RobinState = eRobinState.IDLE;         // 현재 행동
        public bool bAniEndTrigger;
        #endregion

        #region [Variable] BT
        BehaviorTreeRunner _BTRunner = null;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public void SetCircleAttackRangeAble(bool b) => Robin_Setting.Circle_Effect.gameObject.SetActive(b);
        public void SetCircleAttackRangePosition(Vector3 pos) => Robin_Setting.Circle_Effect.transform.position = pos;
        public void SetBoxAttackRangeAble(bool b) => Robin_Setting.Box_Effect.gameObject.SetActive(b);

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Init] Awake
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Awake()
        {
            base.Awake();

            //Json_Utility_Extend.FileSave(Mon_Status, "Data/Json_Data/Monster/Monster_Hero.Json");
            Setting.Monster_Status = Json_Utility_Extend.FileLoadList<DCL_Status>("Data/Json_Data/Monster/Boss_Monster_Tank.Json");
        }
        #endregion

        #region [Init] start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            Robin_State = eRobinState.IDLE;
            RobinAnimation = GetComponent<Animator>();
            Nav_Agent.speed = Mon_Status.Move_Speed;

            Robin_Setting.AttackRangeSetting = 10f;
        }
        #endregion

        #region [Init] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            //_BTRunner.Operate();
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. BT
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [BT] Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Nav_Move(Vector3 pos)
        {
            Nav_Agent.isStopped = false;
            Nav_Agent.SetDestination(pos);
            transform.LookAt(pos);
        }
        #endregion

        #region [BT] Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Nav_Move_Stop()
        {
            Nav_Agent.isStopped = true;
        }
        #endregion


        #region [BT] TargetDistance
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public float GetTargetDistance()
        {
            Vector3 dirvec = PlayerPos.position - transform.position;
            float length = Mathf.Sqrt(Mathf.Pow(dirvec.x, 2) + Mathf.Pow(dirvec.y, 2) + Mathf.Pow(dirvec.z, 2));

            return length;
        }
        #endregion

        #region [BT] AttackRange Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetCircle_AttackRange(Vector3 Size)
        {
            Robin_Setting.Circle_Effect.size = Size;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Attack
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Attack] Create_Arrow
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Create_Arrow(Vector3 Size)
        {
            GameObject InstantMagicball = Instantiate(Robin_Effect.NormalAttack_Effect, Robin_Effect.Root);
            // 매직볼 셋팅
            InstantMagicball.transform.rotation = Quaternion.LookRotation(transform.forward);
            //InstantMagicball.GetComponent<DCL_Staff_MagicBall>().Set_Direction(Vector3.forward, PlayerStatus.Attack_Power);
        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 97. Collider
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collider] Monster_Hit
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Hit(float att)
        {
            base.Hit(att);
        }
        #endregion

        #region [Collision] trigger
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public new void OnTriggerEnter(Collider coll)
        {
            //if (coll.gameObject.name == "Collider" ||
            //    coll.gameObject.name == "MagicBall(Clone)")
            //{
            //    Death();
            //}
        }
        #endregion

        #region [Collider] 몬스터 겹침 방지
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //public override void Hit()
        //{

        //}
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 98. Animation
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Animation] SetTrigger By Animation State
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ChangeAnimationByTrigger(eRobinState robinstate, eRobinState beforerobinstate)
        {
            RobinAnimation.ResetTrigger(beforerobinstate.ToString());
            RobinAnimation.SetTrigger(robinstate.ToString());
        }
        #endregion

        #region [Animation] SetStateAnimation
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetStateNAnimation(eRobinState state)
        {
            BeForeRobinState = RobinState;
            RobinState = state;
            ChangeAnimationByTrigger(RobinState, BeForeRobinState);
        }
        #endregion

        #region [Animation] OnNormalAttackStart
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnNormalAttackStart()
        {

        }
        #endregion

        #region [Animation] OnNormalAttackEnd
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnNormalAttackEnd()
        {

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Death
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collider] Monster_Death
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Death()
        {
            base.Death();
            Destroy(gameObject);
        }
        #endregion


        #region [BackUp]
        /*
         *         #region [BT] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode SettingBT()
        {
            return new SelectorNode
                (
                    new List<INode>()
                    {
                    // 일반 공격
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(NormalAttack),
                            new ActionNode(NormalAttackEnd),
                        }
                    ),
                    // 화살비
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(NormalAttack),
                            new ActionNode(NormalAttackEnd),
                        }
                    ),

                    new ActionNode(RobinMove_Check_AttackRange)

                    }
                );
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. BT Node
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

         *   #region [BT_Node] Robin_Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState RobinMove_Check_AttackRange()
        {
            if (NowAttack == true)
                return INode.eNodeState.Failure;
            else
            {
                SetStateNAnimation(eRobinState.RUN);
                // Ver_2 NavMesh
                Nav_Agent.isStopped = false;
                Nav_Agent.speed = Mon_Status.Move_Speed;
                Nav_Agent.SetDestination(PlayerPos.position);
                transform.LookAt(PlayerPos);

                Vector3 dirvec = transform.position - PlayerPos.position;
                float length = Mathf.Sqrt(Mathf.Pow(dirvec.x, 2) + Mathf.Pow(dirvec.y, 2) + Mathf.Pow(dirvec.z, 2));

                if (Attack_Range > length && NowAttack == false && AttackAble == false)
                {
                    // 사거리 안에 들어오면 공격
                    NowAttack = true;
                    return INode.eNodeState.Success;
                }

                return INode.eNodeState.Running;
            }
        }
        #endregion

        #region [BT_Node] NormalAttack
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState NormalAttack()
        {
            if (NowAttack == true)
            {
                if (NormalAttSetting.AttStart == false)
                {
                    SetStateNAnimation(eRobinState.ATTACK);
                    Nav_Agent.isStopped = true;
                }
                if (NormalAttSetting.AttTimer > NormalAttSetting.IdleTime)
                {
                    return INode.eNodeState.Success;
                }
                else
                {
                    NormalAttSetting.AttTimer += Time.deltaTime;
                    return INode.eNodeState.Running;
                }

            }
            return INode.eNodeState.Failure;
        }
        #endregion

        #region [BT_Node] NormalAttack
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState NormalAttackEnd()
        {
            if (NormalAttSetting.AttEnd == false)
            {
                SetStateNAnimation(eRobinState.IDLE);
                NormalAttSetting.AttStart = false;
                NormalAttSetting.AttTimer = 0;
                NowAttack = false;
                return INode.eNodeState.Running;
            }
            return INode.eNodeState.Failure;
        }
        #endregion
         */
        #endregion

    }

}

