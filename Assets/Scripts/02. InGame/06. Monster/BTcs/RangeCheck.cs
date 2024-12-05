using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RangeCheck", story: "Check Now [Target] And [Self] [Distance]", category: "Action", id: "1e0c310ee210c8bc80383fd882f94d6f")]
public partial class RangeCheckAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Distance;
    protected override Status OnStart()
    {
        Vector3 dirvec = Target.Value.position - Self.Value.transform.position;
        float length = Mathf.Sqrt(Mathf.Pow(dirvec.x, 2) + Mathf.Pow(dirvec.y, 2) + Mathf.Pow(dirvec.z, 2));
        Distance.Value = length;
        return Status.Success;
    }
}

