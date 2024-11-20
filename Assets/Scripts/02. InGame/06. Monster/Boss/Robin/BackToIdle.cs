using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BackToIdle", story: "Change To [Idle] When Animation End", category: "Action", id: "2d8da4f0ec7a21cb71a0dfb866acdb1d")]
public partial class BackToIdleAction : Action
{
    [SerializeReference] public BlackboardVariable<SelfStatus> Idle;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        bool end = this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().bAniEndTrigger;
        if (end == true)
        {
            Idle.Value = SelfStatus.IDLE;
            end = false;
            return Status.Success;
        }
        else
            return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

