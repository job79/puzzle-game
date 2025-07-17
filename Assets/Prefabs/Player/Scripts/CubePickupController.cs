using UnityEngine;

/// <summary>
/// CubePickupController implments the logic for picking up and moving cubes in the game.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CubePickupController : MonoBehaviour
{
    public float followSpeed = 250f;
    public float maxDistance = 3f;
    public Vector2 offset = new (0.5f, 1f);
    public float pickupAngularDamping = 2f;
    
    private Rigidbody2D _pickedUpCube;
    private float _originalAngularDamping;
    private Collider2D _playerCollider;
    private Vector2 _mapSwitchOffset = Vector2.zero;
    
    private void Awake()
    {
        _playerCollider = GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        if (_pickedUpCube != null) DropCube();
        else PickUpCube();
    }

    private void FixedUpdate()
    {
        if (_pickedUpCube != null) MoveCube();
    }

    private void DropCube()
    {
        Collider2D blockCollider = _pickedUpCube.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(blockCollider, _playerCollider, false);
        _pickedUpCube.angularDamping = _originalAngularDamping;
        _pickedUpCube = null;
    }
    
    private void PickUpCube()
    {
        foreach (var collider in Physics2D.OverlapCircleAll(transform.position, 1f))
        {
            if (collider.CompareTag("Cube"))
            {
                _pickedUpCube = collider.GetComponent<Rigidbody2D>();
                _originalAngularDamping = _pickedUpCube.angularDamping;
                _pickedUpCube.angularDamping = pickupAngularDamping;
                Physics2D.IgnoreCollision(collider, _playerCollider, true);
                return;
            }
        }
    }
    
    private void MoveCube()
    {
        if (_mapSwitchOffset != Vector2.zero)
        {
            _pickedUpCube.position += _mapSwitchOffset;
            _mapSwitchOffset = Vector2.zero;
            return;
        }
        
        if (Vector2.Distance(_pickedUpCube.position, transform.position) > maxDistance)
        {
            DropCube();
            return;
        }
        
        Vector2 targetPosition = (Vector2)transform.position + offset;
        _pickedUpCube.linearVelocity = (targetPosition - _pickedUpCube.position) * (followSpeed * Time.fixedDeltaTime);
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
        if (_pickedUpCube != null)
        {
            _mapSwitchOffset = o;
            _pickedUpCube.GetComponent<CubeController>().currentMap = idx;
        }
    }
}
