using UnityEngine;

public class PlayerBehaviour : Wrappable
{
    public delegate void PlayerDamage();
    public static event PlayerDamage PlayerHit;
    private float _thrust, _fade;
    private bool _wasThrusting, _isDissolving, _isBecomingVisible, _isInvisible;
    public Transform barrelPoint;
    Material _material;

    public float rotationSpeed;

    public GameObject thruster, MissilePrefab;

    public Rigidbody2D rb;
    Renderer[] renderers;
    
    void Start()
    {
        _fade = 0f;
        _isBecomingVisible = true;
        _isInvisible = false;
        _wasThrusting = false;
        _thrust = 50f;
        rotationSpeed = 400f;
        thruster.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        renderers = GetComponents<Renderer>();
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (_isDissolving)
        {
            Debug.Log("Desolving");
            _fade -= Time.deltaTime;

            if (_fade <= 0f)
            {
                _fade = 0f;
                _isDissolving = false;
                Debug.Log("NOT visible");
            }
        }

        if (_isBecomingVisible)
        {
            Debug.Log("Becoming visible.");
            _fade += Time.deltaTime;

            if (_fade >= 1f)
            {
                _fade = 1f;
                _isBecomingVisible = false;
                Debug.Log("FULLY visible");
            }
        }

        _material.SetFloat("_fade", _fade);

        Move();
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
        {
            Fire();
        }

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

            if(_wasThrusting == false)
            {
                _wasThrusting = true;
                FindObjectOfType<AudioManager>().Play("thruster");
            }

            acceleration.y = thrustInput * _thrust * Time.deltaTime;
        }
        else
        {
            thruster.SetActive(false);

            if (_wasThrusting == true)
            {
                _wasThrusting = false;
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

    public void Dissolve()
    {
        if (!_isDissolving) _isDissolving = true;
    }

    public void MakeVisible()
    {
        if (!_isBecomingVisible) _isBecomingVisible = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MissileController>() != null) return;

        PlayerHit?.Invoke();
    }
}
