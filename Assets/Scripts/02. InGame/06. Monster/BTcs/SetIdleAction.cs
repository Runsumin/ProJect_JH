using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetIdle", story: "Set Idle", category: "Action", id: "b6eea1aed9f82b677e893f61515af1de")]
public partial class SetIdleAction : Action
{

    protected override Status OnStart()
    {
        this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().Nav_Move_Stop();
        return Status.Running;
    }

}

