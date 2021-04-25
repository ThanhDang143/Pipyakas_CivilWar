using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MLAPI;

public class ButtonInMenu : MonoBehaviour
{
    [SerializeField] private Transform panel;
    [SerializeField] private GameObject canvasStart;
    [SerializeField] private GameObject[] map;

    void Start()
    {
        for (int i = 0; i < map.Length; i++)
        {
            map[i].SetActive(false);
        }
    }

    public void MoveLeft()
    {
        panel.DOMoveX(panel.position.x + 19.2f, 0.5f);
    }

    public void MoveRight()
    {
        panel.DOMoveX(panel.position.x - 19.2f, 0.5f);
    }

    public void MoveUp()
    {
        panel.DOMoveY(panel.position.y + 10.8f, 0.5f);
    }

    public void MoveDown()
    {
        panel.DOMoveY(panel.position.y - 10.8f, 0.5f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BtnHost()
    {
        NetworkManager.Singleton.StartHost();
        canvasStart.SetActive(false);

        for (int i = 0; i < map.Length; i++)
        {
            map[i].SetActive(true);
        }
    }

    public void BtnJoin()
    {
        NetworkManager.Singleton.StartClient();
        canvasStart.SetActive(false);

        for (int i = 0; i < map.Length; i++)
        {
            map[i].SetActive(true);
        }
    }
}
