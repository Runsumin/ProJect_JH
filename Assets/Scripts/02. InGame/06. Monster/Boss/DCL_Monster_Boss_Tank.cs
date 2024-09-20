using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Monster_Boss_Tank
    // 보스몬스터 - 탱크
    //
    //  패턴 3종 구현
    //  - 돌진
    //  - 낙하
    //  - 지진
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_Monster_Boss_Tank : DCL_MonsterBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Enum] Animation State
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public enum eTankState { IDLE, RUN, ATTACK, DASH_CHARGE, DASH_START, DASH_END }
        public enum eTankPattern { DASH, FALLDOWN, EARTHQUAKE }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class Boss_Tank_Setting
        {
            public DecalProjector Box_Effect;
            public DecalProjector Circle_Effect;
        }
        public Boss_Tank_Setting Tank_Setting = new Boss_Tank_Setting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Animation State
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public eTankState Tank_State;
        #region [Variable] Animation
        protected Animator TankAnimation;
        protected eTankState BeForeTankState = eTankState.IDLE;   // 이전 행동
        protected eTankState TankState = eTankState.IDLE;         // 현재 행동
        #endregion
        #endregion

        #region [Variable] Attack
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public float Attack_Range;
        public eTankPattern TankPattern;
        public bool NowAttack;
        #endregion

        #region [Variable] BT
        BehaviorTreeRunner _BTRunner = null;
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

            //Json_Utility_Extend.FileSave(Mon_Status, "Data/Json_Data/Monster/Monster_Hero.Json");
            Setting.Monster_Status = Json_Utility_Extend.FileLoadList<DCL_Status>("Data/Json_Data/Monster/Boss_Monster_Tank.Json");
        }
        #endregion

        #region [Init] start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            Attack_Range = 10f;
            Tank_State = eTankState.IDLE;
            TankAnimation = GetComponent<Animator>();

            _BTRunner = new BehaviorTreeRunner(SettingBT());

        }
        #endregion

        #region [Init] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            _BTRunner.Operate();
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Move
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Move] Monster_Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState TankMove()
        {
            SetStateNAnimation(eTankState.RUN);
            // Ver_2 NavMesh
            Nav_Agent.isStopped = false;
            Nav_Agent.speed = Mon_Status.Move_Speed;
            Nav_Agent.SetDestination(PlayerPos.position);
            transform.LookAt(PlayerPos);

            return INode.eNodeState.Running;
        }
        #endregion

        #region [Move] Idle
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Idle()
        {
            SetStateNAnimation(eTankState.IDLE);
            // Ver_2 NavMesh
            Nav_Agent.isStopped = true;
            transform.LookAt(PlayerPos);
            return INode.eNodeState.Running;
        }
        #endregion


        #region [Move] Check AttackRange
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Check_AttackRange()
        {
            Vector3 dirvec = transform.position - PlayerPos.position;
            float length = Mathf.Sqrt(Mathf.Pow(dirvec.x, 2) + Mathf.Pow(dirvec.y, 2) + Mathf.Pow(dirvec.z, 2));

            if (Attack_Range < length)
                return INode.eNodeState.Failure;
            else
                return INode.eNodeState.Success;
        }
        #endregion        

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. BT
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        INode SettingBT()
        {
            return new SelectorNode
                (
                    new List<INode>()
                    {
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(Check_AttackRange),
                            new ActionNode(Idle),
                        }
                    ),
                    new ActionNode(TankMove),
                    }
                );
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Collider
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
        public void ChangeAnimationByTrigger(eTankState tankstate, eTankState beforetankstate)
        {
            TankAnimation.ResetTrigger(beforetankstate.ToString());
            TankAnimation.SetTrigger(tankstate.ToString());
        }
        #endregion

        #region [Animation] SetStateAnimation
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetStateNAnimation(eTankState state)
        {
            BeForeTankState = TankState;
            TankState = state;
            ChangeAnimationByTrigger(TankState, BeForeTankState);
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


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 00. BackUp
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [BackUp] FSM
        /*
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. FSM
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [FSM] 몬스터 패턴
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Tank_FSM()
        {
            switch (Tank_State)
            {
                case eTankState.IDLE:
                    if (Check_AttackRange())
                    {
                        Tank_State = eTankState.IDLE;
                    }
                    else
                    {
                        Tank_State = eTankState.RUN;
                    }
                    Idle();
                    break;
                case eTankState.RUN:
                    if (Check_AttackRange())
                    {
                        Tank_State = eTankState.DASH_CHARGE;
                    }
                    else
                    {
                        Tank_State = eTankState.RUN;
                    }
                    Move();
                    break;
                case eTankState.ATTACK:
                    if (NowAttack == false)
                    {
                        switch (SetTankPattern())
                        {
                            case eTankPattern.DASH:
                                StartCoroutine(Pattern_Dash());
                                break;
                            case eTankPattern.FALLDOWN:
                                break;
                            case eTankPattern.EARTHQUAKE:
                                break;
                        }
                    }
                    break;
            }

            SetStateNAnimation(Tank_State);

        }
        #endregion

        #region [FSM] 패턴 정하기
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public eTankPattern SetTankPattern()
        {
            TankPattern = eTankPattern.DASH;
            return TankPattern;
        }
        #endregion

        #region [Attack] Monster_Attack
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Attack()
        {

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Attack Pattern
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Pattern] Dash
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public IEnumerator Pattern_Dash()
        {
            // 1. Find Target - Set Destination
            // 2. Charge
            // 3. Dash
            NowAttack = true;

            Vector3 DashDirection = transform.position - PlayerPos.position;
            float DashTime = 0;
            float ChargeTime = 2;
            float DashEndTime = 4;
            while (NowAttack)
            {
                DashTime += Time.deltaTime;
                if (DashTime < ChargeTime)
                    Tank_State = eTankState.DASH_CHARGE;
                else if (DashTime > ChargeTime && DashTime < DashEndTime)
                    Tank_State = eTankState.DASH_START;
                else if (DashTime > DashEndTime)
                {
                    Tank_State = eTankState.DASH_END;
                    NowAttack = false;
                }
                yield return null;
            }
        }
        #endregion
        */
        #endregion
    }

}
