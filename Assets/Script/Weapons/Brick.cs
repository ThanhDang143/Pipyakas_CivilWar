using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Weapons
{
    public override void Start()
    {
        base.Start();
        anim.Play(animationClip.name);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(IEBoom());
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().isStun = true;
            Debug.Log("Stun");
        }
    }
}
