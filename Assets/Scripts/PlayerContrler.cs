using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContrler : MonoBehaviour
{
     Rigidbody2D rb;
     BoxCollider2D coll;
    [Header("移动参数")]
    public float speed=8f;
    public float CrouchSpeedDivisor=3f;
    [Header("跳跃")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float CrouchJumpBoost = 2.5f;
    float jumpTime;
    [Header("状态")]
    public bool isCrouch;
    public bool isGround;
    public bool isJump;
    [Header("环境监测")]
    float xVelocity;
    public LayerMask groundLayer;

    public bool jumpPressed;
    public bool jumpHeld;
    public bool crouchHeld;

    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x,coll.offset.y/2f);
    }

    // Update is called once per frame
    void Update()
    {
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
    }
    private void FixedUpdate()
    {
        PhysicsCheck();
        BasicMovement();
        Jump();
    }
    void PhysicsCheck()
    {
        if (coll.IsTouchingLayers(groundLayer))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
    void BasicMovement() {
        if (crouchHeld&&!isCrouch&&isGround)
        {
            Crouch();
        }
        else if (!crouchHeld && isCrouch)
        {
            StandUp();
        }
        else if (!isGround&&isCrouch)
        {
            StandUp();

        }
        if (isCrouch)
        {
            xVelocity /= CrouchSpeedDivisor;
        }
        xVelocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        if (xVelocity<0)       
            transform.localScale = new Vector2(-1, 1);       
        if (xVelocity>0)       
            transform.localScale = new Vector2(1, 1);      
    }
    void Jump()
    {
        if (jumpPressed&&isGround&&!isJump)
        {
            if (isCrouch&&isGround)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, CrouchJumpBoost), ForceMode2D.Impulse);
            }
            isGround = false;
            isJump = true;
            jumpTime = Time.time + jumpHoldDuration;
            //突然给力
            rb.AddForce(new Vector2(0f,jumpForce),ForceMode2D.Impulse);
        }
        else if (isJump)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if (jumpTime<Time.time)
            {
                isJump = false;
            }
        }

    }
    void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
        
    }
    void StandUp() { 
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }
}
