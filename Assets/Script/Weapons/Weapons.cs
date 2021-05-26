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
        }
    }

    public IEnumerator IEBoom()
    {
        yield return new WaitForSeconds(2f);
        explosion.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        colli.isTrigger = true;
        float stepTime = explosion.main.duration / 18;
        float disDmgZone = Mathf.Abs(dmgZone - colli.radius);
        // IgnoreCollision with player = false;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        }
        // Scale Up dmgZone;
        for (int i = 0; i < 5; i++)
        {
            colli.radius += disDmgZone / 5;
            yield return new WaitForSeconds(stepTime);
        }
        yield return new WaitForSeconds(13 * stepTime);
        Destroy(gameObject);
    }
}
