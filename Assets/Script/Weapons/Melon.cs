using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melon : Weapons
{
    [SerializeField] private AnimationClip animationClip;
    public override void Start()
    {
        base.Start();
        anim.Play(animationClip.name);
        StartCoroutine(IEBoom());
    }
}
