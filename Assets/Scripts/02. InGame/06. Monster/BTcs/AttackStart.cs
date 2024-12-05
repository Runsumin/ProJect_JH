using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackStart", story: "[Attack] Start in [Animator]", category: "Action", id: "1ee51aa4765c292f43cff63daf3eccb2")]
public partial class AttackStartAction : Action
{
    [SerializeReference] public BlackboardVariable<string> Attack;
    [SerializeReference] public BlackboardVariable<Animator> Animator;

    protected override Status OnStart()
    {
        Animator.Value.SetTrigger(Attack);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //if (Animator.Value.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        //    return Status.Running;
        //else
        //{
            return Status.Success;
        //}
    }

    protected override void OnEnd()
    {
    }
}

