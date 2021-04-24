using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveScreen : MonoBehaviour
{
    [SerializeField] private Transform panel;
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
}
