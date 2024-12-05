using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetSelfStatus", story: "Set [SelfStatus] to [SettingStatus]", category: "Action", id: "de21ea6b42050d2a2e6343f87faeb4e3")]
public partial class SetSelfStatusAction : Action
{
    [SerializeReference] public BlackboardVariable<SelfStatus> SelfStatus;
    [SerializeReference] public BlackboardVariable<SelfStatus> SettingStatus;
    protected override Status OnStart()
    {
        SelfStatus.Value = SettingStatus.Value;
        return Status.Running;
    }

}

