using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melon : Weapons
{
    public override void Start()
    {
        base.Start();
        anim.Play(animationClip.name);
        StartCoroutine(IEBoom());
    }
}
