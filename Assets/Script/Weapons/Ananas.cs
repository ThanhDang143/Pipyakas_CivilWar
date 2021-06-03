using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ananas : Weapons
{

    public override void Start()
    {
        base.Start();
        anim.Play(animationClip.name);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        StartCoroutine(IEAnim());
        StartCoroutine(IEBoom());
    }

    private IEnumerator IEAnim()
    {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), lifeTime / 2).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(lifeTime / 2);
        transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), lifeTime / 2).SetEase(Ease.InOutSine);
    }
}
