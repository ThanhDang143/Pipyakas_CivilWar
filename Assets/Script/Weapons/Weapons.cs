using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapons : MonoBehaviour
{
    public int dame;
    public float dameZone, throwDistance;
    private Animator anim;
    [SerializeField] private AnimationClip melon;

    public void Start() {
        anim = GetComponent<Animator>();
        anim.Play(melon.name);
        StartCoroutine(IEBoom());
    }

    public IEnumerator IEBoom()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
