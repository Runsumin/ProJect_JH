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

    public Animator ani;
    protected override Status OnStart()
    {
        ani = this.GameObject.GetComponent<Animator>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !ani.IsInTransition(0))
            return Status.Success;
        else
            return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

