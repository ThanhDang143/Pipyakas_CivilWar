using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using MLAPI;
using UnityEditor;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private Vector2 lastMove;
    private bool wasMovingVertical;
    private float horizontal;
    private float vertical;
    private bool isMovingHorizontal;
    private bool isMovingVertical;
    private bool isLeft;
    private bool isRight;
    private bool isBack;
    private bool isFront;

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
    private bool isOverlap;



    void Start()
    {
        iD = NetworkObjectId;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.Play(idle[0].name);
        ChooseTeamPod();
        isLeft = false;
        isRight = false;
        isBack = false;
        isFront = true;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (IsLocalPlayer)
        {
            Move();
            Attack();
        }
    }

    private void Attack()
    {
        isOverlap = Physics2D.OverlapCircle(transform.position, overlapRadius, overlapMask);
        if (Input.GetKeyDown(KeyCode.Space) && !isOverlap)
        {
            GameObject w = Instantiate(weapon, transform.position, Quaternion.identity);
        }
    }

    private void ChooseTeamPod()
    {
        teamPod.GetComponent<SpriteRenderer>().sprite = teamPodSprite[iD - 1];
    }

    private void SetAnimHorizontal()
    {
        if (horizontal > 0)
        {
            anim.Play(move[3].name);
            if (!isRight)
            {
                isLeft = false;
                isRight = true;
                isBack = false;
                isFront = false;
            }
        }
        else if (horizontal < 0)
        {
            anim.Play(move[2].name);
            if (!isLeft)
            {
                isLeft = true;
                isRight = false;
                isBack = false;
                isFront = false;
            }
        }
    }

    private void SetAnimVertical()
    {
        if (vertical > 0)
        {
            anim.Play(move[1].name);
            if (!isBack)
            {
                isLeft = false;
                isRight = false;
                isBack = true;
                isFront = false;
            }
        }
        else if (vertical < 0)
        {
            anim.Play(move[0].name);
            if (!isFront)
            {
                isLeft = false;
                isRight = false;
                isBack = false;
                isFront = true;
            }
        }
    }

    private void SetAnimIdle()
    {
        if (isBack)
        {
            anim.Play(idle[1].name);
        }
        else if (isFront)
        {
            anim.Play(idle[0].name);
        }
        else if (isLeft)
        {
            anim.Play(idle[2].name);
        }
        else if (isRight)
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
                SetAnimHorizontal();
            }
            else
            {
                rb.velocity = new Vector2(0, vertical * moveSpeed * Time.deltaTime);
                lastMove = new Vector2(0f, vertical);
                SetAnimVertical();
            }
        }
        else if (isMovingHorizontal)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, 0);
            wasMovingVertical = false;
            lastMove = new Vector2(horizontal, 0f);
            SetAnimHorizontal();
        }
        else if (isMovingVertical)
        {
            rb.velocity = new Vector2(0, vertical * moveSpeed * Time.deltaTime);
            wasMovingVertical = true;
            lastMove = new Vector2(0f, vertical);
            SetAnimVertical();
        }
        else
        {
            rb.velocity = Vector2.zero;
            SetAnimIdle();
        }
    }
}
