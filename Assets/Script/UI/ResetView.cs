using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetView : MonoBehaviour
{


    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Clouds"))
            {
                transform.GetChild(i).transform.position -= new Vector3(Time.deltaTime / 16, 0f, 0f);
            }
            if (transform.GetChild(i).CompareTag("Hills"))
            {
                transform.GetChild(i).transform.position -= new Vector3(Time.deltaTime / 8, 0f, 0f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Clouds"))
        {
            other.transform.position = new Vector3(35f, Random.Range(15f, 20f), 0f);
        }
        if (other.CompareTag("Hills"))
        {
            other.transform.position = new Vector3(40f, other.transform.position.y, 0f);
        }
    }
}
