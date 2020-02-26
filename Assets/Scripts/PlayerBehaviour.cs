using System.Collections;
using UnityEngine;

public class PlayerBehaviour : Wrappable
{
    public delegate void PlayerDamage();
    public delegate void MissileFire();
    public static event PlayerDamage PlayerHit;
    public static event MissileFire MissileFired;

    private float _thrust, _fade, _shieldThickness;
    private bool _wasThrusting, _isDissolving, _isBecomingVisible, _isMovable, _isDamageable, _shieldShrinking;
    public Transform barrelPoint;
    public Renderer renderer;
    public Material material;
    public float rotationSpeed;
    public GameObject thruster, MissilePrefab;
    public Rigidbody2D rb;
    
    void Start()
    {        
        _fade = 0f;
        _isBecomingVisible = true;
        _wasThrusting = false;
        _thrust = 50f;
        _shieldThickness = 2f;
        rotationSpeed = 400f;
        thruster.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        material.SetFloat("_shieldThickness", _shieldThickness);
        material.SetFloat("_shieldEnabled", 1);
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
            }
        }

        if(_shieldShrinking)
        {
            _shieldThickness -= Time.deltaTime * 2;
            if(_shieldThickness <= 0) 
            {
                _shieldShrinking = false;
                Debug.Log("Shield shrunk finished");
                _shieldThickness = 2;
            }
        }

        material.SetFloat("_visibility", _fade);
        material.SetFloat("_shieldThickness", _shieldThickness);

        Move();
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
        {
            Fire();
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0f,0f,0f);
        transform.rotation = Quaternion.identity;
        rb.velocity = new Vector2(0,0);
        StartCoroutine(DamageImunity());
    }

    IEnumerator DamageImunity()
    {
        material.SetFloat("_shieldEnabled", 1);
        _isDamageable = false;
        _shieldShrinking = true;
        
        yield return new WaitForSeconds(4f);
        
        _isDamageable = true;
        material.SetFloat("_shieldEnabled", 0);
        
    }

    IEnumerator TakeDamage()
    {
        material.SetFloat("_damageEnabled", 1);
        yield return new WaitForSeconds(0.1f);
        material.SetFloat("_damageEnabled", 0);
    }

    void Fire()
    {
        if (_isMovable)
        {
            Instantiate(MissilePrefab, barrelPoint.position, barrelPoint.rotation);
            MissileFired?.Invoke();
        }
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
        // rb.AddTorque(rotationValue);
        transform.Rotate(0, 0, rotationValue, Space.Self);

        transform.position = WrappingBehaviour.WrappingUpdate(this);
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
        StartCoroutine(DamageImunity());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore missile collisions
        if (collision.GetComponent<MissileController>() != null) return;

        if (_isDamageable)
        {
            StartCoroutine(TakeDamage());
            PlayerHit?.Invoke();
        }
    }
}
