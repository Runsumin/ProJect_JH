using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UnFollowTarget", story: "[Self] UnFollow [Target]", category: "Action", id: "4f089f298ae1db27d276fd92c6a98be1")]
public partial class UnFollowTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    protected override Status OnStart()
    {
        this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().Nav_Move_Stop();
        //this.GameObject.GetComponent<HSM.Game.DCL_Monster_Boss_Robin>().
        //    SetStateNAnimation(HSM.Game.DCL_Monster_Boss_Robin.eRobinState.IDLE);

        this.GameObject.GetComponent<Animator>().SetFloat("Speed", 0);
        return Status.Success;
    }   
}

