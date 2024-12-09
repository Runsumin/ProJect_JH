using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackRangeUnSet", story: "UnSet AttackRange", category: "Action", id: "043b26b2c6e32db5cf209af6a7552248")]
public partial class AttackRangeUnSetAction : Action
{
    protected override Status OnStart()
    {
        this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().SetCircleAttackRangeAble(false);
        this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().SetBoxAttackRangeAble(false);
        return Status.Running;
    }
}

