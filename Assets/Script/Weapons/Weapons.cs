using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapons : MonoBehaviour
{
    public int dmg;
    public float dmgZone, throwDistance;
    public Animator anim;
    public Collider2D colli;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        colli = GetComponent<Collider2D>();
        colli.isTrigger = true;
        StartCoroutine(IEBoom());
    }

    public IEnumerator IEBoom()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
