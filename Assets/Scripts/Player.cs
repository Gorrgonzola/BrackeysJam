using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float climbCost = 1f;
    [SerializeField] private float eatingTime = 4f;
    [SerializeField] private float eatingDistance = 0.3f;

    public Action EatNotification;

    private new Rigidbody2D rigidbody2D;
    private Hunger hunger;
    private new Collider2D collider2D;

    private Animator animator;
    private new SpriteRenderer renderer;

    private AudioSource playerAudio;

    private bool eating = false;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        hunger = GetComponent<Hunger>();
        collider2D = GetComponent<Collider2D>();

        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        playerAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (eating)
            return;

        float moveDirection = Input.GetAxisRaw("Horizontal");
        float climbDirection = Input.GetAxisRaw("Vertical");
        MovementBehaviour(moveDirection);
        AnimationBehaviour(moveDirection);
        ClimbBehaviour(climbDirection);
    }

    private void ClimbBehaviour(float climbDirection)
    {
        if (!collider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.speed = 1f;
            animator.SetBool("Climbing", false);
            rigidbody2D.gravityScale = 1f;
            return;
        }
        rigidbody2D.gravityScale = 0f;
        if (climbDirection == 0)
        {
            animator.speed = 0f;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
            return;
        }
        hunger.AffectHunger(-climbCost * Time.deltaTime);
        rigidbody2D.velocity =
        new Vector2(rigidbody2D.velocity.x, climbDirection * climbSpeed);
        animator.speed = 1f;
        animator.SetBool("Climbing", true);
    }

    private void MovementBehaviour(float direction)
    {
        rigidbody2D.velocity =
        new Vector2(direction * speed, rigidbody2D.velocity.y);
    }


    private void AnimationBehaviour(float direction)
    {
        if (IsPlayerMoving())
        {
            animator.SetBool("Walking", true);
            FlipSprite(direction);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    private void FlipSprite(float direction)
    {
        renderer.flipX = direction < 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact(other);
        }
    }

    public void Interact(Collider2D other)
    {
        var interactable = other.transform.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.Interact(this);
        }
    }

    public void EatCheese(float cheeseAmount)
    {
        playerAudio.Play();
        hunger.AffectHunger(cheeseAmount);
    }

    public void Teleport(Vector3 destination)
    {
        StartCoroutine(DisableControlAndMoveTo(destination));
    }

    IEnumerator DisableControlAndMoveTo(Vector3 destination)
    {
        EatNotification();
        eating = true;
        collider2D.enabled = false;
        renderer.enabled = false;
        rigidbody2D.gravityScale = 0f;
        float currentLerpTime = 0f;
        while (Vector3.Distance(transform.position, destination) > eatingDistance)
        {
            yield return null;
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > eatingTime)
            {
                currentLerpTime = eatingTime;
            }
            transform.position = Vector3.Lerp(transform.position, destination, currentLerpTime / eatingTime);
        }
        EatNotification();
        collider2D.enabled = true;
        renderer.enabled = true;
        rigidbody2D.gravityScale = 1f;
        eating = false;
    }

    private bool IsPlayerMoving() => Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;

}
