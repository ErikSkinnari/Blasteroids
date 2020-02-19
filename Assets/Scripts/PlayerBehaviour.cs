using UnityEngine;

public class PlayerBehaviour : Wrappable
{
    public delegate void PlayerDamage();
    public static event PlayerDamage PlayerHit;
    private float thrust;
    bool wasThrusting;
    public Transform barrelPoint;

    public float rotationSpeed;

    public GameObject thruster, MissilePrefab;

    public Rigidbody2D rb;
    Renderer[] renderers;

    private void Update()
    {
        Move();
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }
    void Start()
    {
        wasThrusting = false;
        thrust = 50f;
        rotationSpeed = 400f;
        thruster.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        renderers = GetComponents<Renderer>();
    }

    void Fire()
    {
        Instantiate(MissilePrefab, barrelPoint.position, barrelPoint.rotation);
    }

    private void Move()
    {

        var rotationInput = Input.GetAxis("Horizontal");
        var thrustInput = Input.GetAxis("Vertical");

        var acceleration = new Vector2();

        if (thrustInput > 0)
        {
            thruster.SetActive(true);

            if(wasThrusting == false)
            {
                wasThrusting = true;
                FindObjectOfType<AudioManager>().Play("thruster");
            }

            acceleration.y = thrustInput * thrust * Time.deltaTime;
        }
        else
        {
            thruster.SetActive(false);

            if (wasThrusting == true)
            {
                wasThrusting = false;
                FindObjectOfType<AudioManager>().Stop("thruster");
            }
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
        if (collision.GetComponent<MissileController>() != null) return;

        PlayerHit?.Invoke();
    }
}
