using UnityEngine;

/// <summary>
/// CameraController lets the camera follow the player smoothly.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Configuration")]
    public float smoothTime = 0.15f;
    public Vector3 offset = new (0f, 1f, -10f);

    private GameObject _player;
    private Vector3 _velocity;
    private Vector3 _mapSwitchOffset;
    private bool _respawned;
    
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void LateUpdate()
    {
        if (_mapSwitchOffset != Vector3.zero)
        {
            // When switching maps, teleport the camera to the new position.
            transform.position += _mapSwitchOffset;
            _mapSwitchOffset = Vector2.zero;
            return;
        }

        if (_respawned)
        {
            // If the player has respawned, teleport the camera to the new position.
            transform.position = _player.transform.position + offset;
            _respawned = false;
            return;
        }

        Vector3 targetPosition = _player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }

    private void OnEnable()
    {
        GameManager manager = GameManager.Instance;
        manager.onMapSwitch.AddListener(OnMapSwitch);
        manager.onRespawn.AddListener(OnRespawn);
    }

    private void OnDisable()
    {
        GameManager manager = GameManager.Instance;
        manager.onMapSwitch.RemoveListener(OnMapSwitch);
        manager.onRespawn.RemoveListener(OnRespawn);
    }
    
    private void OnMapSwitch(int idx, Vector3 o) => _mapSwitchOffset = o;
    private void OnRespawn() => _respawned = true;
}