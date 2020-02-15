using UnityEngine;

public class PlayerBehaviour : Wrappable
{
    private float thrust;

    public float rotationSpeed;

    public GameObject thruster;

    public Rigidbody2D rb;
    Renderer[] renderers;

    private void Update() => Move();
    void Start()
    {
        thrust = 50f;
        rotationSpeed = 400f;
        thruster.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        renderers = GetComponents<Renderer>();
        //camera = GetComponent<Camera>();

    }

    private void Move()
    {

        var rotationInput = Input.GetAxis("Horizontal");
        var thrustInput = Input.GetAxis("Vertical");

        var acceleration = new Vector2();

        if (thrustInput > 0)
        {
            thruster.SetActive(true);

            // Set acceleration to thrust * deltaTime IF player presses up.

            acceleration.y = thrustInput * thrust * Time.deltaTime;
            
        }
        else
        {
            thruster.SetActive(false);
        }

        // Add the acceleration to the ship
        rb.AddRelativeForce(acceleration);


        // Set rotation depending on left/right input
        var rotationValue = -rotationInput * rotationSpeed * Time.deltaTime;
        // Rotate the ship
        transform.Rotate(0, 0, rotationValue, Space.Self);

        transform.position = WrappingBehaviour.WrappingUpdate(this);
        //ScreenWrap();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }
}
