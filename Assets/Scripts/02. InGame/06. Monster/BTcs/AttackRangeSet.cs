using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackRangeSet", story: "Set AttackRange To [Target] Type [AttackRangeType]", category: "Action", id: "ad7c14d35b80872172768c95768d6c6e")]
public partial class AttackRangeSetAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<AttackRangeType> AttackRangeType;
    protected override Status OnStart()
    {
        switch (AttackRangeType.Value)
        {
            case global::AttackRangeType.Circle:
                this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().SetCircleAttackRangeAble(true);
                this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().SetCircleAttackRangePosition(Target.Value.position);
                this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().SetCircle_AttackRange(new Vector3(32, 25, 1));
                break;
            case global::AttackRangeType.Box:
                this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().SetBoxAttackRangeAble(true);
                break;
        }
        return Status.Running;
    }
}

