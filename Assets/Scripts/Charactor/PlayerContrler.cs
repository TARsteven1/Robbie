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
    public float hangingJumpForce = 15f;
    [Header("状态")]
    public bool isCrouching;
    public bool isOnGround;
    public bool isJumping;
    public bool isHeadBlocked;
    public bool isHanging;
    [Header("环境监测")]
    public float footOffset;
    public float headClearance=0.5f;
    public float groundDistance = 0.2f;
    public float xVelocity;
    public LayerMask groundLayer;
    [Header("悬挂检测")]
    float playerHeight;
    public float eyeHeight=1.5f;
    public float grabDistance=0.4f;
    public float reachOffset = 0.7f;

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
        playerHeight= coll.size.y;
        footOffset = coll.size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver())
        {
            return;
        }

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
        //脚部检测射线
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset,0f),Vector2.down,groundDistance,groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if (leftCheck|| rightCheck)
            isOnGround = true;
        else
            isOnGround = false;

        //头部检测射线
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);
        if (headCheck)
            isHeadBlocked = true;
        else
            isHeadBlocked = false;
        //悬挂射线
        float direction = transform.localScale.x;
        Vector2 graDir = new Vector2(direction,0f);
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset*direction, playerHeight), graDir, grabDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), graDir, grabDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);
        if (!isOnGround&&rb.velocity.y<0f&&ledgeCheck&&wallCheck&&!blockedCheck)
        {
            Vector3 pos = transform.position;
            pos.x += wallCheck.distance-0.05f * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }
    void BasicMovement() {
        if (isHanging)
            return;

        if (crouchHeld&&!isCrouching&&isOnGround)
        {
            Crouch();
        }
        else if (!crouchHeld && isCrouching&&!isHeadBlocked)
        {
            StandUp();
        }
        else if (!isOnGround&&isCrouching)
        {
            StandUp();

        }
        if (isCrouching)
        {
            xVelocity /= CrouchSpeedDivisor;
        }
        xVelocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        if (xVelocity<0)       
            transform.localScale = new Vector3(-1, 1,1);       
        if (xVelocity>0)       
            transform.localScale = new Vector3(1, 1,1);      
    }
    void Jump()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
            if (Input.GetButtonDown("Crouch"))
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                //rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
        }
        if (jumpPressed&&isOnGround&&!isJumping && !isHeadBlocked)
        {
            if (isCrouching&&isOnGround)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, CrouchJumpBoost), ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJumping = true;
            jumpTime = Time.time + jumpHoldDuration;
            //突然给力
            rb.AddForce(new Vector2(0f,jumpForce),ForceMode2D.Impulse);

            AudioManager.PlayJumpAudio();
        }
        else if (isJumping)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if (jumpTime<Time.time)
            {
                isJumping = false;
            }
        }

    }
    void Crouch()
    {
        isCrouching = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
        
    }
    void StandUp() { 
        isCrouching = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }
    //射线判断接触
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDistance, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDistance, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos+offset,rayDistance*length);
        return hit;
    }

}
