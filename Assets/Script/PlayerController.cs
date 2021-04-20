using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using MLAPI;

public class PlayerController : NetworkBehaviour
{
    // Move player
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
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
    private Animator anim;
    [SerializeField] private AnimationClip idle;
    [SerializeField] private AnimationClip idleBack;
    [SerializeField] private AnimationClip idleLeft;
    [SerializeField] private AnimationClip idleRight;
    [SerializeField] private AnimationClip moveRight;
    [SerializeField] private AnimationClip moveLeft;
    [SerializeField] private AnimationClip moveFront;
    [SerializeField] private AnimationClip moveBack;
    [SerializeField] private GameObject teamPod;
    [SerializeField] private Sprite[] teamPodSprite;
    [SerializeField] ulong iD;
    [SerializeField] GameObject weapon;


    void Start()
    {

        iD = NetworkObjectId;
        anim = GetComponent<Animator>();
        anim.Play(idle.name);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject w = Instantiate(weapon, transform.position, Quaternion.identity);
        }
    }

    private void ChooseTeamPod()
    {
        GameObject tP = Instantiate(teamPod, transform);
        tP.transform.position = transform.position + new Vector3(0f, -0.2f, 0f);
        tP.GetComponent<SpriteRenderer>().sprite = teamPodSprite[iD - 1];
    }

    private void SetAnimHorizontal()
    {
        if (horizontal > 0)
        {
            anim.Play(moveRight.name);
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
            anim.Play(moveLeft.name);
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
            anim.Play(moveBack.name);
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
            anim.Play(moveFront.name);
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
            anim.Play(idleBack.name);
        }
        else if (isFront)
        {
            anim.Play(idle.name);
        }
        else if (isLeft)
        {
            anim.Play(idleLeft.name);
        }
        else if (isRight)
        {
            anim.Play(idleRight.name);
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
