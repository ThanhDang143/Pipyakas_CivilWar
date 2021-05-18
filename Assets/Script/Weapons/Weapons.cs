using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapons : MonoBehaviour
{
    public int dmg;
    public float dmgZone, throwPower;
    [HideInInspector] public CircleCollider2D colli;
    [HideInInspector] public Animator anim;
    [HideInInspector] public bool isOverlapPlayer;
    public float overlapRadius;
    public LayerMask overlapMask;
    public ParticleSystem explosion;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        colli = GetComponent<CircleCollider2D>();
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
        yield return new WaitForSeconds(2f);
        explosion.Play();
        StartCoroutine(ShakeCamera.ins.IEShake(2.5f, 0.1f));
        GetComponent<SpriteRenderer>().enabled = false;
        colli.isTrigger = true;
        float stepTime = explosion.main.duration / 18;
        float disDmgZone = Mathf.Abs(dmgZone - colli.radius);
        // IgnoreCollision with player = false;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        }
        foreach (GameObject teamPod in GameObject.FindGameObjectsWithTag("TeamPod"))
        {
            Physics2D.IgnoreCollision(teamPod.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        }
        // Scale Up dmgZone;
        for (int i = 0; i < 6; i++)
        {
            colli.radius += disDmgZone / 6;
            yield return new WaitForSeconds(stepTime);
        }
        //Scale Down dmgZone
        for (int i = 0; i < 12; i++)
        {
            colli.radius -= disDmgZone / 12;
            yield return new WaitForSeconds(stepTime);
        }
        Destroy(gameObject);
    }
}
