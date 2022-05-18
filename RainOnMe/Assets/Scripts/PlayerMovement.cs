using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{

    // movement
    public CharacterController2D characterController;
    private float horizontalMove = 0f;
    public float movementSpeed = 40f;
    private float playerMovementSpeedModifier;

    // jumping
    public float jumpVelocity;
    private int jumpCount = 0;
    public int extraJumps = 1;
    [SerializeField] LayerMask groundLayer; // check what is ground
    [SerializeField] Transform playerBottom;
    public bool isGrounded;
    public float jumpCooldown;
    public float jumpCooldownModifer = 0.2f;
    public float fallingThreshhold = -10f;
    private bool isFalling = false;

    // ??
    bool movementJump = false;
    bool movementCrouch = false;

    // dash mechanics
    public float dashDistance = 15f;
    public float dashDuration = 0.4f;
    public float dashCooldown = 1f;
    public bool isDashing;
    private bool canUseDash = true;
    float doubleTapTime;
    KeyCode lastKeyCode;
    public float dashAnimationDelay;

    public Rigidbody2D rb;
    private PlayerCombat playerCombat;
    private bool isAttacking;

    // Animations
    public Animator animator;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed; // detect horizontal movement 
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetFloat("yVelocity", rb.velocity.y);
        isAttacking = playerCombat.isAttacking;

        // dashing left
        if (Input.GetKeyDown(KeyCode.A) && !isDashing && canUseDash && !isAttacking)
        {
            if(doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
            {
                StartCoroutine(Dash(-1f));
            }
            else
            {
                doubleTapTime = Time.time + 0.4f;
            }
            lastKeyCode = KeyCode.A;
        }

        // dashing right
        if (Input.GetKeyDown(KeyCode.D) && !isDashing && canUseDash && !isAttacking)
        {
            if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
            {
                StartCoroutine(Dash(1f));
            }
            else
            {
                doubleTapTime = Time.time + 0.4f;
            }
            lastKeyCode = KeyCode.D;
        }

        if (Input.GetButtonDown("Jump") && !isDashing)
        {
            Jump();
        }
        // always check after all physics 
        CheckGrounded(); 
    }

    private void FixedUpdate()
    {
        characterController.Move(horizontalMove * Time.fixedDeltaTime, false, false, isGrounded);
        movementJump = false;
    }

    private void Jump()
    {
        if(isGrounded || jumpCount < extraJumps)
        {
            movementJump = true;
            animator.SetBool("IsJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            jumpCount++;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    private void CheckGrounded()
    {
        bool wasGrounded = isGrounded;
        if (Physics2D.OverlapCircle(playerBottom.position, 0.5f, groundLayer))
        {
            isGrounded = true;
            jumpCount = 0;
            jumpCooldown = Time.time + jumpCooldownModifer;
        } 
        //else if (Time.time < jumpCooldown)
        //{
        //    isGrounded = true;
        //}
        else
        {
            isGrounded = false;
        }
    }

    IEnumerator Dash(float direction)
    {
        animator.SetInteger("IsDashing", 1);
        canUseDash = false;
        isDashing = true;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;

        yield return new WaitForSeconds(dashDuration);  
        
        rb.gravityScale = gravity;

        StartCoroutine(StartDashCooldown());
        yield return new WaitForSeconds(dashAnimationDelay);

        animator.SetInteger("IsDashing", 0);

        StartCoroutine(IsDashingDelay());
    }

    IEnumerator StartDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canUseDash = true;
    }

    IEnumerator IsDashingDelay()
    {
        yield return new WaitForSeconds(0f);
        isDashing = false;
    }


}
