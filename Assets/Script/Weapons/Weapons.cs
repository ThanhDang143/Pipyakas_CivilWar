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
    [HideInInspector] public bool isOverlap;
    public float overlapRadius;
    public LayerMask overlapMask;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        colli = GetComponent<Collider2D>();
        colli.enabled = false;
        // StartCoroutine(IEBoom());
    }

    public void Update()
    {
        if (colli.enabled == false)
        {
            // isOverlap = Physics2D.OverlapCircle(transform.position, overlapRadius, overlapMask);
            // if (!isOverlap)
            // {
            //     colli.enabled = true;
            // }
        }
    }

    public IEnumerator IEBoom()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
