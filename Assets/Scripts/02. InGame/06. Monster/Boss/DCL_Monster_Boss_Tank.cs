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
        public enum eTankState { IDLE, RUN, ATTACK, DASH_CHARGE, DASH_START, DASH_END, QUAKE_CHARGE, QUAKE_START, QUAKE_END, JUMP_START, JUMP_END }
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
            public GameObject Model;
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
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class QuakePatternSetting
        {
            public float QuakeTimer;        // 패턴진행시간
            public float QuakeChargeTime;   // 차지 시간
            public float QuakeTime;         // 대쉬 진행 시간
            public float AttackRange;       // 공격 범위
            public float IdleTime;          // 공격 후 대기시간
            public bool QuakeChargeStart;
            public bool QuakeStart;
            public bool Quake_End;
        }
        public QuakePatternSetting QuakePTSetting = new QuakePatternSetting();
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class JumpPatternSetting
        {
            public float JumpTimer;         // 패턴진행시간
            public float JumpStartTime;     // 점프 시간
            public float JumpingTime;           // 패턴 진행 시간
            public float JumpEndTime;       // 점프 진행 시간
            public float AttackRange;       // 공격 범위
            public float IdleTime;          // 공격 후 대기시간
            public bool JumpStart;
            public bool Jumping;
            public bool Jump_End;
        }
        public JumpPatternSetting JumpPTSetting = new JumpPatternSetting();
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
        public int RandomPT;
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
            // Quake Setting
            QuakePTSetting.QuakeTimer = 0;
            QuakePTSetting.QuakeChargeTime = 3;
            QuakePTSetting.QuakeTime = 4;
            QuakePTSetting.IdleTime = 6;
            QuakePTSetting.AttackRange = 10;
            // Jump Setting
            JumpPTSetting.JumpTimer = 0;
            JumpPTSetting.JumpStartTime = 1;
            JumpPTSetting.JumpingTime = 3;
            JumpPTSetting.JumpEndTime = 5;
            JumpPTSetting.IdleTime = 6;
            JumpPTSetting.AttackRange = 10;
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
                            // 랜덤선택
                            new ActionNode(RandomPattern_Dash),
                            // 대쉬 준비
                            new ActionNode(Dash_Charge),
                            // 대쉬
                            new ActionNode(Dashing),
                            // 대쉬 완료
                            new ActionNode(Dash_End),

                        }
                    ),
                    // 지진공격
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            // 랜덤선택
                            new ActionNode(RandomPattern_Quake),
                            // 지진패턴 준비
                            new ActionNode(Quake_Charge),
                            // 지진 공격
                            new ActionNode(Quake),
                            // 완료
                            new ActionNode(Quake_End),

                        }
                    ),
                    // 점프공격
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            // 랜덤선택
                            new ActionNode(RandomPattern_Jump),
                            // 점프 시작
                            new ActionNode(Jump_Start),
                            // 점프 공격
                            new ActionNode(Jumping),
                            // 점프 완료
                            new ActionNode(Jump_End),

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
                    RandomPatternMaker();
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

        #region [BT_Node] Quake Charge
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Quake_Charge()
        {
            if (NowAttack == true)
            {
                // 대쉬 시작
                if (QuakePTSetting.QuakeChargeStart == false)
                {
                    SetStateNAnimation(eTankState.QUAKE_CHARGE);
                    Tank_Setting.Circle_Effect.gameObject.SetActive(true);
                    Nav_Agent.isStopped = true;
                    QuakePTSetting.QuakeChargeStart = true;
                }

                if (QuakePTSetting.QuakeTimer > QuakePTSetting.QuakeChargeTime)
                {
                    return INode.eNodeState.Success;
                }
                else
                {
                    QuakePTSetting.QuakeTimer += Time.deltaTime;
                    Tank_Setting.Circle_Effect.size = new Vector3(QuakePTSetting.QuakeTimer * QuakePTSetting.AttackRange, (float)((double)QuakePTSetting.QuakeTimer * QuakePTSetting.AttackRange * 0.8), 1);
                    return INode.eNodeState.Running;
                }
            }
            return INode.eNodeState.Failure;
        }
        #endregion

        #region [BT_Node] Quake
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Quake()
        {
            // 대쉬 시작
            if (QuakePTSetting.QuakeStart == false)
            {
                SetStateNAnimation(eTankState.QUAKE_START);
                Tank_Setting.Circle_Effect.gameObject.SetActive(false);
                QuakePTSetting.QuakeStart = true;
            }

            if (QuakePTSetting.QuakeTimer > QuakePTSetting.QuakeTime)
            {
                return INode.eNodeState.Success;
            }
            else
            {
                QuakePTSetting.QuakeTimer += Time.deltaTime;
                return INode.eNodeState.Running;
            }

        }
        #endregion

        #region [BT_Node] Quake End
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Quake_End()
        {
            // 대쉬 시작
            if (QuakePTSetting.Quake_End == false)
            {
                SetStateNAnimation(eTankState.IDLE);
                Nav_Agent.isStopped = true;
                QuakePTSetting.Quake_End = true;
            }

            if (QuakePTSetting.QuakeTimer > QuakePTSetting.IdleTime)
            {
                QuakePTSetting.QuakeChargeStart = false;
                QuakePTSetting.QuakeStart = false;
                QuakePTSetting.Quake_End = false;
                NowAttack = false;
                QuakePTSetting.QuakeTimer = 0;

                StartCoroutine(AttackAbleTimeCheck());
                return INode.eNodeState.Failure;
            }
            else
            {
                QuakePTSetting.QuakeTimer += Time.deltaTime;
                return INode.eNodeState.Running;
            }

        }
        #endregion

        #region [BT_Node] Jump Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Jump_Start()
        {
            if (NowAttack == true)
            {
                // 대쉬 시작
                if (JumpPTSetting.JumpStart == false)
                {
                    SetStateNAnimation(eTankState.JUMP_START);
                    Nav_Agent.isStopped = true;
                    JumpPTSetting.JumpStart = true;
                }

                if (JumpPTSetting.JumpTimer > JumpPTSetting.JumpStartTime)
                {
                    return INode.eNodeState.Success;
                }
                else
                {
                    JumpPTSetting.JumpTimer += Time.deltaTime;
                    return INode.eNodeState.Running;
                }
            }
            return INode.eNodeState.Failure;
        }
        #endregion

        #region [BT_Node] Jumping
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Jumping()
        {
            // 대쉬 시작
            if (JumpPTSetting.Jumping == false)
            {
                //SetStateNAnimation(eTankState.JUMP_START);
                Tank_Setting.Circle_Effect.gameObject.SetActive(true);
                Tank_Setting.Model.SetActive(false);
                JumpPTSetting.Jumping = true;
                transform.position = PlayerPos.position;
            }

            if (JumpPTSetting.JumpTimer > JumpPTSetting.JumpingTime)
            {
                return INode.eNodeState.Success;
            }
            else
            {
                JumpPTSetting.JumpTimer += Time.deltaTime;
                Tank_Setting.Circle_Effect.size = new Vector3((JumpPTSetting.JumpTimer - JumpPTSetting.JumpStartTime) * JumpPTSetting.AttackRange, 
                    (float)((double)(JumpPTSetting.JumpTimer - JumpPTSetting.JumpStartTime) * JumpPTSetting.AttackRange * 0.8), 1);
                return INode.eNodeState.Running;
            }
        }
        #endregion

        #region [BT_Node] Jump End
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState Jump_End()
        {
            // 대쉬 시작
            if (JumpPTSetting.Jump_End == false)
            {
                SetStateNAnimation(eTankState.JUMP_END);
                Tank_Setting.Model.SetActive(true);
                Tank_Setting.Circle_Effect.gameObject.SetActive(false);
                Nav_Agent.isStopped = true;
                JumpPTSetting.Jump_End = true;
            }

            if (JumpPTSetting.JumpTimer > JumpPTSetting.JumpEndTime)
            {
                JumpPTSetting.JumpStart = false;
                JumpPTSetting.Jump_End = false;
                JumpPTSetting.Jumping = false;
                NowAttack = false;
                JumpPTSetting.JumpTimer = 0;

                SetStateNAnimation(eTankState.IDLE);
                StartCoroutine(AttackAbleTimeCheck());
                return INode.eNodeState.Failure;
            }
            else
            {
                JumpPTSetting.JumpTimer += Time.deltaTime;
                return INode.eNodeState.Running;
            }

        }
        #endregion

        #region [BT_Node] RandomPattern_Maker
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState RandomPattern_Dash()
        {
            if (RandomPT == 0)
                return INode.eNodeState.Success;
            else
                return INode.eNodeState.Failure;
        }
        #endregion

        #region [BT_Node] RandomPattern_Maker
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState RandomPattern_Quake()
        {
            if (RandomPT == 1)
                return INode.eNodeState.Success;
            else
                return INode.eNodeState.Failure;
        }
        #endregion

        #region [BT_Node] RandomPattern_Maker
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        INode.eNodeState RandomPattern_Jump()
        {
            if (RandomPT == 2)
                return INode.eNodeState.Success;
            else
                return INode.eNodeState.Failure;
        }
        #endregion

        #region [BT_Node] RandomPattern_Maker
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public int RandomPatternMaker()
        {
            RandomPT = UnityEngine.Random.Range(0, 3);
            //RandomPT = 2;
            return RandomPT;
        }
        #endregion

        #region [AttackAbleTimeCheck]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerator AttackAbleTimeCheck()
        {
            AttackAble = true;
            while (true)
            {
                AttackCoolTime += Time.deltaTime;

                if (AttackCoolTime > 2)
                {
                    AttackCoolTime = 0;
                    AttackAble = false;
                    break;
                }
            }
            yield return null;
        }
        #endregion

        #region [Random Maker]
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
