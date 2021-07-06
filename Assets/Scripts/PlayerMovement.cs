using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SECOND MOVEMENT TEMPLATE
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public LayerMask groundLayer; // which layer should we consider ground (for jumping)

    [Header("Speeds")]
    [SerializeField] private float movespeed;
    [SerializeField] private float jumpspeed;
    [SerializeField] private float short_jumpspeed; // jump speed if jump button only tapped

    [Space]

    [Header("Ground Check")]
    [SerializeField] private Vector2 grounded_offset; // where should our circle detecting ground be located?
    [SerializeField] private float grounded_radius = 0.25f; // ground check circle size

    private Vector2 direction; // movement direction
    private int jump_count = 2; // how many times can we jump
    private bool grounded = false; // is the character touching ground?
    private bool try_jump = false; // is the player trying to jump?
    private bool jump_cancelled = false; // did player cancel jump?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {   // detect input in Update() so we don't miss any inputs

        float x_component = Input.GetAxisRaw("Horizontal");

        direction = new Vector2(x_component * movespeed, rb.velocity.y);

        // TASK #1
        if (IsGrounded() == true);
        {
            grounded = true;
        }
        // IMPLEMENT code here to check if our player is grounded here, set our 'grounded' variable appropriately
        // Jumps should be reset upon touching ground

        if (Input.GetButtonDown("Jump"))
        {
            // user presses jump

            if (grounded)
            {
                // TASK #2
                Jump();
                jump_count--;
                if(jump_count == 0)
                {
                    grounded = false;
                }
                /* If our character is grounded and we pressed jump, we need to
                 execute the jump function. We should also modify the jump_count variable here
                 to keep track of how many jumps we have left. Implement this code.*/
            }
            else if (!grounded && (jump_count == 2))
            {
                // TASK #3
                Jump();
                jump_count--;

                /* This conditional says that we are not on the ground yet we still have 2 jumps left.
                This must mean that we fell off a cliff somewhere. In this situation where the user presses 
                jump, we should allow them to jump, but only ONCE. Implement this code. */

            }
            else if (jump_count > 0)
            {
                // TASK #4
                Jump();
                jump_count--;

                // Here, the player is in the air but still has jumps. Implement this code.
            }

        }

        if (Input.GetButtonUp("Jump") && !grounded)
        {
            // If the player cancelled the jump in mid-air, don't jump as high
            jump_cancelled = true;
        }

    }
    private void FixedUpdate()
    {
        Move(direction);
        if (try_jump)
        {
            Jump();
        }

        if (jump_cancelled)
        {
            JumpCancel();
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle((Vector2)transform.position + grounded_offset, grounded_radius, groundLayer);
        // this long line of code creates an adjustable circle. If this circle overlaps anything on the ground layer
        // the function will return True.
    }

    private void Move(Vector2 direction)
    {
        rb.velocity = direction;
        // Create a velocity in a specific direction
    }

    private void Jump()
    {
        rb.velocity = new Vector2(0, jumpspeed);
        try_jump = false;
    }

    private void JumpCancel()
    {

        // TASK #5
        if (rb.velocity.y > short_jumpspeed)
        {
            rb.velocity = new Vector2(0, short_jumpspeed);
        }
        /* In this section of code, the player cancelled the jump.
         When the jump is cancelled, we want the character to have a smaller jump.
         We do this by checking if the character's current velocity is bigger than
         the short jumpspeed. If it is, we should correct it by setting the velocity
         of our rigid body to a new vector that represents the short jumpspeed. Implement this.
         Hint: The x-component of the new vector shouldn't change, just the y-component.
        */

        jump_cancelled = false;
    }

    private void OnDrawGizmos()
    {
        // Draw the circle that detects where ground is
        // Helpful for setting up and debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + grounded_offset, grounded_radius);
    }

}