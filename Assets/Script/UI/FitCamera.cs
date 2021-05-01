using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FitCamera : MonoBehaviour
{
    [SerializeField] SpriteRenderer bG;

    void Awake()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = bG.bounds.size.x / bG.bounds.size.y;
        float differenceInSize = targetRatio / screenRatio;
        Camera.main.orthographicSize = bG.bounds.size.y / 2 * differenceInSize;
        Debug.Log("Camera" + Camera.main.orthographicSize);
    }
}
