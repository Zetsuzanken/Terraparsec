using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float thrustForce = 5f;
    public float maxSpeed = 10f;
    public float deceleration = 0.5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 0;
        rb.angularDrag = 0;
    }

    void Update()
    {
        HandleThrust();
        HandleRotation();
        HandleDeceleration();
    }

    void HandleThrust()
    {
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            rb.AddForce(new Vector2(input * thrustForce, 0), ForceMode2D.Force);
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
    }

    void HandleRotation()
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

    void HandleDeceleration()
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
