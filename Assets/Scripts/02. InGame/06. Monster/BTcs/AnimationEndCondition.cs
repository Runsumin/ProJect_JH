using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "AnimationEnd", story: "if NowPlay [Animatior] [Animation] End", category: "Conditions", id: "663007dfee356fec3ad6190910803fda")]
public partial class AnimationEndCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Animator> Animatior;
    [SerializeReference] public BlackboardVariable<string> Animation;

    public override bool IsTrue()
    {
        if (Animatior.Value.GetCurrentAnimatorStateInfo(0).IsName(Animation) &&
            Animatior.Value.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            return true;
        else
            return false;
    }

    //public override void OnStart()
    //{
    //}

    //public override void OnEnd()
    //{
    //}
}
