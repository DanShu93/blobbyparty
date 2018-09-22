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

    [Header("Flip")]
    [SerializeField]
    float flipTime = 1f;
    [SerializeField]
    AnimationCurve flipSmoother;

    public bool jumpPressed;
    public bool flipPressed;

    public bool grounded;
    bool gravityDecreased;
    bool turning;
    bool flipping;

    WaitForEndOfFrame wait = new WaitForEndOfFrame();
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!grounded)
        {
            if ((!jumpPressed || rb.velocity.y < 0f) && gravityDecreased)
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

        if(flipPressed && !flipping)
        {
            //Direction (= bool) depends on where the player is in relation to the ball
            StartCoroutine(Flip(true));
        }
    }

    IEnumerator Flip(bool clockWise)
    {
        flipping = true;

        float timer = 0f;
        float percentage = 0f;

        float start = transform.eulerAngles.z;

        float target = start;
        if (clockWise)
        {
            target -= 180f;
        }
        else
        {
            target += 180f;
        }

        while (percentage < 1f)
        {
            timer += Time.deltaTime;
            percentage = timer / flipTime;
            float smoothedPercentage = flipSmoother.Evaluate(percentage);

            transform.eulerAngles = Vector3.forward * Mathf.Lerp(start, target, smoothedPercentage);

            yield return wait;
        }

        transform.eulerAngles = Vector3.forward * target;

        flipping = false;
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
        rb.gravityScale = minGravity;
        gravityDecreased = true;

        grounded = false;
    }

    //Check if Jump is pressed
    public void SynchInput(bool jump, bool flip)
    {
        jumpPressed = jump;
        flipPressed = flip;
    }

    public void ResetGravity()
    {
        rb.gravityScale = maxGravity;
        gravityDecreased = false;
    }

    public void Move(float h)
    {
        if ((h > 0f && rb.velocity.x < maxSpeed) || (h < 0f && rb.velocity.x > -maxSpeed))
        {
            rb.AddForce(Vector3.right * acceleration * Time.deltaTime * h);
        }

        rb.velocity = new Vector2(rb.velocity.x * (1 - horizontalDrag), rb.velocity.y);
    }

    //Check if grounded
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") && !grounded)
        {
            grounded = true;
        }
        else if(other.gameObject.CompareTag("Ball"))
        {
            BallController controller = other.gameObject.GetComponent<BallController>();

            controller.HitBall(10f, (-1)*other.contacts[0].normal.normalized);

            Debug.Log("Ball Hit");
        }
    }
}
