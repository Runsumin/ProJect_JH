using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetActionState", story: "Set [AttackState] Random", category: "Action", id: "c759e08f12201779b7b3e6537a8bbb10")]
public partial class SetActionStateAction : Action
{
    [SerializeReference] public BlackboardVariable<AttackPattern> AttackState;

    float[] Pattern_1 = { 100, 0, 0, 0 };
    float[] Pattern_2 = { 0, 30, 30, 30 };

    protected override Status OnStart()
    {
        float grade = HSM.Game.RandomMaker.Choose(Pattern_1);
        AttackState.Value = (AttackPattern)grade;
        return Status.Success;
    }

    //protected override Status OnUpdate()
    //{
    //    return Status.Success;
    //}

    //protected override void OnEnd()
    //{
    //}
}

