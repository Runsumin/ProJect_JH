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
            public float AttactCoolTime;
        }
        public Boss_Tank_Setting Tank_Setting = new Boss_Tank_Setting();
        #endregion

        #region [class] InGamePatternSet
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class DashPatternSetting
        {
            public float DashTimer;         // 패턴진행시간
            public float DashChargeTime;    // 차지 시간
            public float DashIngTime;       // 대쉬 진행 시간
            public float AttackRange;       // 공격 범위
            public float IdleTime;          // 공격 후 대기시간
            public bool ChargeStart;
            public bool DashStart;
            public bool Dash_End;
        }
        public DashPatternSetting DashPTSetting = new DashPatternSetting();

        [Serializable]
        public class QuakePatternSetting
        {
            public float QuakeTimer;        // 패턴진행시간
            public float QuakeChargeTime;   // 차지 시간
            public float QuakeTime;       // 대쉬 진행 시간
            public float AttackRange;       // 공격 범위
            public float IdleTime;          // 공격 후 대기시간
            public bool QuakeChargeStart;
            public bool QuakeStart;
            public bool Quake_End;
        }
        public QuakePatternSetting QuakePTSetting = new QuakePatternSetting();
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public class IdleSetting
        {
            public float IdleTimer;         // 패턴진행시간
        }
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

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public bool NowAttack;
        public bool AttackEnd;
        public float AttackCoolTime;
        public bool AttackAble;
        //------------------------------------------------------------------------------------------------------------------------------------------------------
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

            // Dash Setting
            DashPTSetting.DashTimer = 0;
            DashPTSetting.DashChargeTime = 2;
            DashPTSetting.DashIngTime = 4;
            DashPTSetting.IdleTime = 6;
            DashPTSetting.AttackRange = 10;
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
        // 2. BT
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [BT] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode SettingBT()
        {
            return new SelectorNode
                (
                    new List<INode>()
                    {
                    // 대시공격
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            // 대쉬 준비
                            new ActionNode(Dash_Charge),
                            // 대쉬
                            new ActionNode(Dashing),
                            // 대쉬 완료
                            new ActionNode(Dash_End),

                        }
                    ),
                    new ActionNode(TankMove_Check_AttackRange)

                    }
                );
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. BT Node
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [BT_Node] Monster_Move
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState TankMove_Check_AttackRange()
        {
            if (NowAttack == true)
                return INode.eNodeState.Failure;
            else
            {
                SetStateNAnimation(eTankState.RUN);
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

        #region [BT_Node] Check AttackRange
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Check_AttackRange()
        {
            Vector3 dirvec = transform.position - PlayerPos.position;
            float length = Mathf.Sqrt(Mathf.Pow(dirvec.x, 2) + Mathf.Pow(dirvec.y, 2) + Mathf.Pow(dirvec.z, 2));

            if (Attack_Range > length && NowAttack == false)
            {
                // 사거리 안에 들어오면 공격
                NowAttack = true;
                return INode.eNodeState.Success;
            }
            else
            {
                return INode.eNodeState.Failure;
            }
        }
        #endregion

        #region [BT_Node] Dash charge
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Dash_Charge()
        {
            if (NowAttack == true)
            {
                // 대쉬 시작
                if (DashPTSetting.ChargeStart == false)
                {
                    SetStateNAnimation(eTankState.DASH_CHARGE);
                    Tank_Setting.Box_Effect.gameObject.SetActive(true);
                    Nav_Agent.isStopped = true;
                    DashPTSetting.ChargeStart = true;
                }

                if (DashPTSetting.DashTimer > DashPTSetting.DashChargeTime)
                {
                    return INode.eNodeState.Success;
                }
                else
                {
                    DashPTSetting.DashTimer += Time.deltaTime;
                    Tank_Setting.Box_Effect.size = new Vector3(15, 1, DashPTSetting.DashTimer * DashPTSetting.AttackRange);
                    Tank_Setting.Box_Effect.pivot = new Vector3(0, 0, DashPTSetting.DashTimer * DashPTSetting.AttackRange / 2);
                    return INode.eNodeState.Running;
                }
            }
            return INode.eNodeState.Failure;
        }
        #endregion

        #region [BT_Node] Dashing
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Dashing()
        {
            // 대쉬 시작
            if (DashPTSetting.DashStart == false)
            {
                SetStateNAnimation(eTankState.DASH_START);
                Tank_Setting.Box_Effect.gameObject.SetActive(false);
                DashPTSetting.DashStart = true;
            }

            if (DashPTSetting.DashTimer > DashPTSetting.DashIngTime)
            {
                return INode.eNodeState.Success;
            }
            else
            {
                DashPTSetting.DashTimer += Time.deltaTime;
                transform.position += Time.deltaTime * transform.forward * DashPTSetting.AttackRange;
                return INode.eNodeState.Running;
            }

        }
        #endregion

        #region [BT_Node] Dash_End
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Dash_End()
        {
            // 대쉬 시작
            if (DashPTSetting.Dash_End == false)
            {
                SetStateNAnimation(eTankState.IDLE);
                Nav_Agent.isStopped = true;
                DashPTSetting.Dash_End = true;
            }

            if (DashPTSetting.DashTimer > DashPTSetting.IdleTime)
            {
                DashPTSetting.Dash_End = false;
                DashPTSetting.ChargeStart = false;
                DashPTSetting.DashStart = false;
                NowAttack = false;
                DashPTSetting.DashTimer = 0;

                StartCoroutine(AttackAbleTimeCheck());
                return INode.eNodeState.Failure;
            }
            else
            {
                DashPTSetting.DashTimer += Time.deltaTime;
                return INode.eNodeState.Running;
            }

        }
        #endregion

        #region [BT_Node] Idle
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Idle()
        {

        }
        #endregion

        #region
        IEnumerator AttackAbleTimeCheck()
        {
            AttackAble = true;
            while (true)
            {
                AttackCoolTime += Time.deltaTime;

                if(AttackCoolTime > 2)
                {
                    AttackCoolTime = 0;
                    AttackAble = false;
                    break;
                }
            }
            yield return null;
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
