using System.Collections;
using UnityEngine;

public class PlayerBehaviour : Wrappable
{
    public delegate void PlayerDamage();
    public delegate void MissileFire();
    public static event PlayerDamage PlayerHit;
    public static event MissileFire MissileFired;

    private float _thrust, _fade;
    private bool _wasThrusting, _isDissolving, _isBecomingVisible, _isMovable, _isDamageable;
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
            _isDamageable = false;
            _fade -= Time.deltaTime;

            if (_fade <= 0f)
            {
                _fade = 0f;
                _isDissolving = false;
            }
        }

        if (_isBecomingVisible)
        {
            Debug.Log("Becoming visible.");
            _fade += Time.deltaTime / 2;

            if (_fade >= 1f)
            {
                _fade = 1f;
                _isBecomingVisible = false;
                StartCoroutine(DamageImunity());
            }
        }

        _material.SetFloat("_fade", _fade);

        Move();
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
        {
            Fire();
        }

    }

    IEnumerator DamageImunity()
    {
        _isDamageable = false;
        yield return new WaitForSeconds(3f);
        _isDamageable = true;
    }

    void Fire()
    {
        Instantiate(MissilePrefab, barrelPoint.position, barrelPoint.rotation);
        MissileFired?.Invoke();
    }

    private void Move()
    {
        float rotationInput = 0f;
        float thrustInput = 0f;

        if (_isMovable)
        {
            rotationInput = Input.GetAxis("Horizontal");
            thrustInput = Input.GetAxis("Vertical");
        }

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

    public void MovementEnabled(bool enable)
    {
        _isMovable = enable;
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
        // Ignore missile collisions
        if (collision.GetComponent<MissileController>() != null) return;

        if (_isDamageable)
        {
            PlayerHit?.Invoke();
        }
    }
}
