using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float thrustForce = 4f;
    public float maxSpeed = 8f;
    public float deceleration = 20f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 0;
        rb.angularDrag = 0;
    }

    private void Update()
    {
        HandleThrust();
        HandleRotation();
        HandleDeceleration();
    }

    private void HandleThrust()
    {
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            rb.AddForce(new Vector2(input * thrustForce, 0), ForceMode2D.Force);
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
    }

    private void HandleRotation()
    {
        if (rb.velocity.x > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (rb.velocity.x < -0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void HandleDeceleration()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f)
        {
            float decelerationFactor = deceleration * Time.deltaTime;

            rb.velocity = new Vector2(
                Mathf.MoveTowards(rb.velocity.x, 0, decelerationFactor),
                rb.velocity.y
            );
        }
    }
}
