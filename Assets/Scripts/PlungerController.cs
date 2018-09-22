using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float maxSpeed = 10f;
    [SerializeField]
    float acceleration = 10f;
    [SerializeField]
    float horizontalDrag = 0.05f;


    [Header("Gravity")]
    [SerializeField]
    float minGravity = 1f;
    [SerializeField]
    float maxGravity = 3f;
    [SerializeField]
    [Tooltip("How much the gravity increses per second after jumping (up to maxGravity)")]
    float gravityIncrease = 2f;
    [SerializeField]
    float jumpForce = 10f;

    

    bool jumpPressed;

    bool grounded;
    bool gravityDecreased;

    Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!grounded)
        {
            if((!jumpPressed || rb.velocity.y < 0f) && gravityDecreased)
            {
                ResetGravity();
            }
            else if (rb.gravityScale <= maxGravity)
            {
                rb.gravityScale += gravityIncrease * Time.deltaTime;
            }
            else
            {
                ResetGravity();
            }
        }
        else
        {
            if (jumpPressed)
            {
                Jump();
            }
        }
    }


    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
        rb.gravityScale = minGravity;
        gravityDecreased = true;

        grounded = false;
    }

    //Check if Jump is pressed
    public void SetJump(bool i)
    {
        jumpPressed = i;
    }

    public void ResetGravity()
    {
        rb.gravityScale = maxGravity;
        gravityDecreased = false;
    }

    public void Move(float h)
    {
        if((h > 0f && rb.velocity.x < maxSpeed) || (h < 0f && rb.velocity.x > -maxSpeed))
        {
            rb.AddForce(Vector3.right * acceleration * Time.deltaTime * h);
        }

        rb.velocity = new Vector2(rb.velocity.x * (1 - horizontalDrag), rb.velocity.y);
        //rb.AddForce(Vector3.right * -rb.velocity.x * Time.deltaTime * horizontalDrag);
    }

    //Check if grounded
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") && !grounded)
        {
            grounded = true;
        }
    }
}
