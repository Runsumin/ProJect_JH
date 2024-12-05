using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DistanceCheck", story: "if [TargetDistance] Lower than NowDistance", category: "Action", id: "c2966721f1af73c8d42e37f0dc523911")]
public partial class DistanceCheckModifier : Modifier
{
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

