using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEditor;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Spawning;

public class PlayerController : NetworkBehaviour
{
    [Header("Player Info")]
    [SerializeField] private NetworkVariableInt maxHealth = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly });
    [SerializeField] private NetworkVariableInt currentHealth = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly });
    [SerializeField] private float moveSpeed;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject vCam;
    private Rigidbody2D rb;
    private Vector2 lastMove;
    private bool wasMovingVertical;
    private float horizontal;
    private float vertical;
    private bool isMovingHorizontal;
    private bool isMovingVertical;
    private NetworkVariableBool isLeft = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, false);
    private NetworkVariableBool isRight = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, false);
    private NetworkVariableBool isBack = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, false);
    private NetworkVariableBool isFront = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, false);

    [Header("Animation")]
    [SerializeField] private AnimationClip[] idle; //Front //Back //Left //Right
    [SerializeField] private AnimationClip[] move; //Front //Back //Left //Right
    private Animator anim;

    [Header("Identifier")]
    [SerializeField] ulong iD;
    [SerializeField] private GameObject teamPod;
    [SerializeField] private Sprite[] teamPodSprite;

    [Header("Weapons & Overlap Detection")]
    [SerializeField] private GameObject weapon;
    [SerializeField] private float overlapRadius;
    [SerializeField] private LayerMask overlapMask;
    private bool isOverlapWaepons;

    void Start()
    {
        //Setup
        iD = NetworkObjectId;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (!IsLocalPlayer)
        {
            vCam.gameObject.SetActive(false);
        }
        ChooseTeamPod();
        //Setup for health
        maxHealth.Value = 100;
        healthBar.SetMaxHealth(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
        //Setup for Anim
        anim.Play(idle[0].name);
        isLeft.Value = false;
        isRight.Value = false;
        isBack.Value = false;
        isFront.Value = true;
    }

    void Update()
    {
        if (!IsLocalPlayer) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Move();

        isOverlapWaepons = Physics2D.OverlapCircle(transform.position, overlapRadius, overlapMask);
        if (Input.GetKeyDown(KeyCode.Space) && !isOverlapWaepons)
        {
            AttackServerRpc();
        }
    }

    // Take dmg
    private void OnTriggerEnter2D(Collider2D other)
    {
        TakeDmgServerRpc(other.GetComponent<Weapons>().GetComponent<Weapons>().dmg);
    }

    [ServerRpc]
    public void TakeDmgServerRpc(int dmgOfWeapons)
    {
        TakeDmgClientRpc(dmgOfWeapons);
    }

    [ClientRpc]
    public void TakeDmgClientRpc(int dmgOfWeapons)
    {
        currentHealth.Value -= dmgOfWeapons;
        healthBar.SetHealth(currentHealth.Value);
    }

    [ServerRpc]
    public void AttackServerRpc()
    {
        GameObject w = Instantiate(weapon, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(w.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        ThrowWeapons(w);
        w.GetComponent<NetworkObject>().Spawn();
        AttackClientRpc();
    }

    [ClientRpc]
    public void AttackClientRpc()
    {
    }

    private void ThrowWeapons(GameObject weapon)
    {
        if (isLeft.Value)
        {
            weapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(-weapon.GetComponent<Weapons>().throwPower, 0));
            return;
        }
        if (isRight.Value)
        {
            weapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(weapon.GetComponent<Weapons>().throwPower, 0));
            return;
        }
        if (isFront.Value)
        {
            weapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -weapon.GetComponent<Weapons>().throwPower));
            return;
        }
        if (isBack.Value)
        {
            weapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, weapon.GetComponent<Weapons>().throwPower));
            return;
        }
    }

    private void ChooseTeamPod()
    {
        teamPod.GetComponent<SpriteRenderer>().sprite = teamPodSprite[Random.Range(0, teamPodSprite.Length)];
    }

    [ServerRpc]
    public void SetAnimHorizontalServerRpc()
    {
        SetAnimHorizontalClientRpc();
    }

    [ClientRpc]
    public void SetAnimHorizontalClientRpc()
    {
        if (horizontal > 0)
        {
            anim.Play(move[3].name);
            if (!isRight.Value)
            {
                isLeft.Value = false;
                isRight.Value = true;
                isBack.Value = false;
                isFront.Value = false;
            }
        }
        else if (horizontal < 0)
        {
            anim.Play(move[2].name);
            if (!isLeft.Value)
            {
                isLeft.Value = true;
                isRight.Value = false;
                isBack.Value = false;
                isFront.Value = false;
            }
        }
    }

    [ServerRpc]
    private void SetAnimVerticalServerRpc()
    {
        SetAnimVerticalClientRpc();
    }

    [ClientRpc]
    public void SetAnimVerticalClientRpc()
    {
        if (vertical > 0)
        {
            anim.Play(move[1].name);
            if (!isBack.Value)
            {
                isLeft.Value = false;
                isRight.Value = false;
                isBack.Value = true;
                isFront.Value = false;
            }
        }
        else if (vertical < 0)
        {
            anim.Play(move[0].name);
            if (!isFront.Value)
            {
                isLeft.Value = false;
                isRight.Value = false;
                isBack.Value = false;
                isFront.Value = true;
            }
        }
    }

    private void SetAnimIdle()
    {
        if (isBack.Value)
        {
            anim.Play(idle[1].name);
        }
        else if (isFront.Value)
        {
            anim.Play(idle[0].name);
        }
        else if (isLeft.Value)
        {
            anim.Play(idle[2].name);
        }
        else if (isRight.Value)
        {
            anim.Play(idle[3].name);
        }
    }

    private void Move()
    {
        isMovingHorizontal = Mathf.Abs(horizontal) == 1f;
        isMovingVertical = Mathf.Abs(vertical) == 1f;

        if (isMovingVertical && isMovingHorizontal)
        {
            if (wasMovingVertical)
            {
                rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, 0);
                lastMove = new Vector2(horizontal, 0f);
                SetAnimHorizontalServerRpc();
            }
            else
            {
                rb.velocity = new Vector2(0, vertical * moveSpeed * Time.deltaTime);
                lastMove = new Vector2(0f, vertical);
                SetAnimVerticalServerRpc();
            }
        }
        else if (isMovingHorizontal)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, 0);
            wasMovingVertical = false;
            lastMove = new Vector2(horizontal, 0f);
            SetAnimHorizontalServerRpc();
        }
        else if (isMovingVertical)
        {
            rb.velocity = new Vector2(0, vertical * moveSpeed * Time.deltaTime);
            wasMovingVertical = true;
            lastMove = new Vector2(0f, vertical);
            SetAnimVerticalServerRpc();
        }
        else
        {
            rb.velocity = Vector2.zero;
            SetAnimIdle();
        }
    }
}
