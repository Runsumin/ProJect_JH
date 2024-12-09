using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "Agent Follow [Target] Speed [float]", category: "Action", id: "de170d7e0077db787732c01a1a917d24")]
public partial class PatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Float;
    protected override Status OnStart()
    {
        //this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().
        //    SetStateNAnimation(HSM.Game.DCL_Monster_Boss_Robin.eRobinState.RUN);

        this.GameObject.GetComponent<Animator>().SetFloat("Speed", Float);
        this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().Nav_Move(Target.Value.position);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().Nav_Move(Target.Value.position);
        return Status.Running;
    }
}

