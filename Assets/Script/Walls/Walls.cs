using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Walls : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    [SerializeField] public Sprite[] sprite;
    [SerializeField] public NetworkVariableInt health;

    void Start()
    {
    }

    [ServerRpc]
    public void SetInitWallsServerRpc(int x)
    {
        SetInitWallsClientRpc(x);
    }

    [ClientRpc]
    public void SetInitWallsClientRpc(int x)
    {
        spriteRenderer.sprite = sprite[x];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TakeDmgClientRpc();
    }

    [ServerRpc]
    void TakeDmgServerRpc()
    {
        TakeDmgClientRpc();
    }

    [ClientRpc]
    void TakeDmgClientRpc()
    {
        if (health.Value > 0)
        {
            health.Value -= 1;
            spriteRenderer.sprite = sprite[health.Value];
        }
        if (health.Value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
