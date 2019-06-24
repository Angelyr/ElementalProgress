using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //PUBLIC
    public Animator animator;
    public float runSpeed = 40f;
    public float movementSmoothing = .05f;
    public float jumpForce = 400f;
    public LayerMask whatIsGround;
    public Transform groundCheck;

    //PRIVATE
    private Rigidbody2D myRigidbody2D;
    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;
    private bool facingRight = true;
    private bool grounded = false;
    private const float groundedRadius = .2f;
    private Vector3 velocity = Vector3.zero;

    //Initialize
    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    //Every frame
    void Update()
    {
        //movement input
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        //jump input
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
    }

    //Every physics update
    private void FixedUpdate()
    {
        SetGrounded();
        Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    //continuously get if character is grounded
    private void SetGrounded()
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    animator.SetBool("IsJumping", false);
            }
        }
    }

    //move character after input
    private void Move(float move, bool crouch, bool jump)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = Vector3.SmoothDamp(myRigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

        if (move > 0 && !facingRight) Flip();
        else if (move < 0 && facingRight) Flip();

        if (grounded && jump)
        {
            grounded = false;
            myRigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    //flip character when facing the wrong direction
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
