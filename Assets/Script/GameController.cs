using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController ins;
    public GameObject endScreen;
    public TextMeshProUGUI txtEnd;

    void Awake()
    {
        ins = this;
        endScreen.SetActive(false);
    }

    public void EndGame()
    {
        endScreen.SetActive(true);
    }
}
