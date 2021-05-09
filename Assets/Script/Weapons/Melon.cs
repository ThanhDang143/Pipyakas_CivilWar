using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melon : Weapons
{
    [SerializeField] private AnimationClip melon;
    public override void Start()
    {
        base.Start();
        anim.Play(melon.name);
    }

}
