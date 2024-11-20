using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "Agent Follow [Target] When [Range]", category: "Action", id: "de170d7e0077db787732c01a1a917d24")]
public partial class PatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Range;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 dirvec = Target.Value.transform.position - this.GameObject.transform.position;
        float length = Mathf.Sqrt(Mathf.Pow(dirvec.x, 2) + Mathf.Pow(dirvec.y, 2) + Mathf.Pow(dirvec.z, 2));

        if (length < Range)
        {
            this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().Nav_Move_Stop();
            return Status.Success;
        }
        else
        {
            this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().Nav_Move(Target.Value.transform.position);
            return Status.Running;
        }
    }

    protected override void OnEnd()
    {
    }
}

