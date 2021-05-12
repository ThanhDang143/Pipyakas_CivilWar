using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapons : MonoBehaviour
{
    public int dmg;
    public float dmgZone, throwDistance;
    public Animator anim;
    [HideInInspector] public bool isOverlapPlayer;
    public float overlapRadius;
    public LayerMask overlapMask;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(IEBoom());
    }

    public void Update()
    {
        isOverlapPlayer = Physics2D.OverlapCircle(transform.position, overlapRadius, overlapMask);
        if (!isOverlapPlayer)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            }
            foreach (GameObject teamPod in GameObject.FindGameObjectsWithTag("TeamPod"))
            {
                Physics2D.IgnoreCollision(teamPod.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            }

        }
    }

    public IEnumerator IEBoom()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
