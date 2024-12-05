using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DistanceChecking", story: "if [TargetDistance] Lower than [ActionRange]", category: "Action", id: "545557e8c4018fc65658d9f4cd016d1b")]
public partial class DistanceCheckingAction : Action
{
    [SerializeReference] public BlackboardVariable<float> TargetDistance;
    [SerializeReference] public BlackboardVariable<float> ActionRange;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

