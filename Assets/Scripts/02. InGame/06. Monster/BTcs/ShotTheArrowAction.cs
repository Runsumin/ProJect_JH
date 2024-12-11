using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ShotTheArrow", story: "Show the Arrow Type [AttackPattern]", category: "Action", id: "cad72f7ce80287f0c576734605841217")]
public partial class ShotTheArrowAction : Action
{
    [SerializeReference] public BlackboardVariable<AttackPattern> AttackPattern;

    protected override Status OnStart()
    {
        switch (AttackPattern.Value)
        {
            case global::AttackPattern.NormalAttack:
                this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().Create_Arrow(Vector3.zero);
                break;
            case global::AttackPattern.RainArrow:
                break;
            case global::AttackPattern.Trip:
                break;
            case global::AttackPattern.ChargeAttack:
                break;
        }
        return Status.Running;
    }
}

