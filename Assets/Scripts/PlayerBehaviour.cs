using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private float thrust;

    private float rotationSpeed;

    public GameObject thruster;

    public Rigidbody2D rb;
    Renderer[] renderers;
    bool isWrappingX = false;
    bool isWrappingY = false;

    public Camera camera;

    private void Update() => Move();
    void Start()
    {
        thrust = 50f;
        rotationSpeed = 400f;
        thruster.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        renderers = GetComponents<Renderer>();
    }

    private bool CheckRenderers()
    {
        foreach(var renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }

        return false;
    }

    private void Move()
    {

        var rotationInput = Input.GetAxis("Horizontal");
        var thrustInput = Input.GetAxis("Vertical");

        if(thrustInput > 0)
        {
            thruster.SetActive(true);
        }
        else
        {
            thruster.SetActive(false);
        }

        // Set acceleration to thrust * deltaTime IF player presses up.
        var acceleration = new Vector2
        {
            y = thrustInput * thrust * Time.deltaTime
        };
        // Add the acceleration to the ship
        rb.AddRelativeForce(acceleration);

        // Set rotation depending on left/right input
        var rotationValue = -rotationInput * rotationSpeed * Time.deltaTime;
        // Rotate the ship
        transform.Rotate(0, 0, rotationValue, Space.Self);

        ScreenWrap();
    }

    private void ScreenWrap()
    {
        // Check is ship is visible
        var isVisible = CheckRenderers();

        // If ship is visible, no wrapping is happening
        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if(isWrappingX && isWrappingY)
        {
            return;
        }

        var viewportPosition = camera.WorldToViewportPoint(transform.position);

        // save current position to new V3
        var newPosition = transform.position;

        // Check if the position is outside the camera
        // Check sides.
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            // If outside, teleport to the opposite side
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }
        // Now check the top and bottom
        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            // If outside, teleport to the opposite side
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }
        // Set the player position to the updated position.
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }
}
