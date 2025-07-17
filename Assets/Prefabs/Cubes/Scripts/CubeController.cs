using System.Collections;
using UnityEngine;

/// <summary>
/// CubeController manages the behavior of cubes in the game.
/// </summary>
[RequireComponent(typeof(Animator))]
public class CubeController : MonoBehaviour
{
    [Header("Configuration")]
    public float movementThreshold = 0.01f;
    
    public SpawnerController spawnerController;
    public int originMap;
    public int currentMap;
    
    private Animator _animator;
    private Vector3 _lastPosition;
    private bool _cubesAreReset;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _lastPosition = transform.position;
       currentMap = originMap;
    }

    private void Update()
    {
        if (Vector3.Distance(_lastPosition, transform.position) < movementThreshold) return;
        _lastPosition = transform.position;

        if (!_cubesAreReset && GameManager.Instance.currentMapIdx == originMap)
        {
            _cubesAreReset = true;
            GameManager.Instance.ResetCubes();
        }
    }
    
    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        _animator.SetTrigger("Respawn");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        spawnerController.SpawnCube();
    }
    
    private void OnEnable()
    {
        GameManager manager = GameManager.Instance;
        manager.onMapSwitch.AddListener(OnMapSwitch);
        manager.cubes.Add(this);
    }

    private void OnDisable()
    {
        GameManager manager = GameManager.Instance;
        manager.onMapSwitch.RemoveListener(OnMapSwitch);
        manager.cubes.Remove(this);
    }
    
    private void OnMapSwitch(int idx, Vector3 o) => _cubesAreReset = false;
}
