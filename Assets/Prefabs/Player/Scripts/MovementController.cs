using UnityEngine;

/// <summary>
/// PlayerController moves the player based on user input.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")]
    public float movementSpeed = 8f;
    public float jumpForce = 450f;

    [Header("Air Control")]
    public float normalGravityScale = 0.9f;
    public float fastFallGravityScale = 2f;
    public float airControlMultiplier = 0.3f;
    public float airControlLerpStart = 1.2f;
    public float airControlLerpSpeed = 1f;

    [Header("Jump Acceleration Time")]
    public float hangTime = .2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private TrailRenderer _trail;

    private bool _isGrounded;
    private int _isGroundedCounter;
    private float _jumpTimer;

    private bool _isJumping;
    private float _moveHorizontal;

    private float _lastGroundedHorizontalInput;
    private float _airControlLerpTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _trail = GetComponentInChildren<TrailRenderer>();
    }

    private void Update()
    {
        // Get horizontal input
        _moveHorizontal = Input.GetAxis("Horizontal");

        // Update player state
        _animator.SetBool("IsWalking", _moveHorizontal != 0);
        if (_moveHorizontal != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = _moveHorizontal > 0 ? -1 : 1;
            transform.localScale = scale;
        }

        // Get vertical input for jump
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && (_isGrounded || _jumpTimer < coyoteTime))
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            _isJumping = true;
            _lastGroundedHorizontalInput = _moveHorizontal;
        }

        // Variable jump height: if jump key released early, cut upward velocity
        if (_isJumping && (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space)) && _rb.linearVelocity.y > 0f)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * 0.3f);
            _isJumping = false;
        }

        // Fast fall with S
        _rb.gravityScale = Input.GetKey(KeyCode.S) ? fastFallGravityScale : normalGravityScale;

        // Timer countdown
        if (!_isGrounded)
        {
            _jumpTimer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        float input;

        if (_isGrounded)
        {
            input = _moveHorizontal;
        }
        else
        {
            _airControlLerpTimer -= _airControlLerpTimer > 0 ? Time.deltaTime * airControlLerpSpeed : 0f;
            input = (_isJumping && _jumpTimer < hangTime ? _lastGroundedHorizontalInput : _moveHorizontal) *
                    Mathf.Lerp(airControlMultiplier, airControlLerpStart, _airControlLerpTimer);
        }

        _rb.linearVelocity = new Vector2(input * movementSpeed, _rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Unjumpable")) return;

        _isGroundedCounter++;
        _isGrounded = true;
        if (_isGrounded)
        {
            _animator.SetFloat("JumpTime", _jumpTimer);
            _animator.SetBool("IsJumping", false);
            _trail.emitting = false;
            _isJumping = false;
            _jumpTimer = 0f;
            _airControlLerpTimer = 1f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Unjumpable")) return;
        
        _isGroundedCounter = Mathf.Max(0, _isGroundedCounter - 1);
        if (_isGroundedCounter <= 0)
        {
            _isGrounded = false;
            _animator.SetBool("IsJumping", true);
            _trail.emitting = true;
        }
    }
    
    private void OnEnable()
    {
        GameManager manager = GameManager.Instance;
        manager.onMapSwitch.AddListener(OnMapSwitch);
    }
    
    private void OnDisable()
    {
        GameManager manager = GameManager.Instance;
        manager.onMapSwitch.RemoveListener(OnMapSwitch);
    }

    private void OnMapSwitch(int idx, Vector3 o)
    {
        transform.position += o;
        _trail.Clear();
    }
}