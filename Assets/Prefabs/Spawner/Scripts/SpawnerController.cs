using System.Collections;
using UnityEngine;

/// <summary>
/// SpawnerController manages the spawning of cubes in the game.
/// </summary>
[RequireComponent(typeof(Animator))]
public class SpawnerController : Activateable
{
    [Header("Configuration")]
    public GameObject prefabCube;
    public Vector3 spawnOffset = new (0, -.8f, 0);
    
    private bool _isSpawning;
    private CubeController _cube;
    private Animator _animator;

    private void Awake()
    {   
        _animator = GetComponent<Animator>();
    }

    protected override void OnActivation()
    {
        if (_cube != null)
        {
            _cube.Respawn();
            _cube = null;
        }
        else SpawnCube();
    }
    
    protected override void OnDeactivation()
    {
    }

    public void SpawnCube()
    {
        if (isActive && !_isSpawning) StartCoroutine(SpawnCubeCoroutine());
    }
    
    private IEnumerator SpawnCubeCoroutine()
    {
        _isSpawning = true;
        yield return new WaitForSeconds(.5f);
        _animator.SetTrigger("Spawn");
        yield return new WaitForSeconds(.3f);
        
        GameObject gameObject = Instantiate(prefabCube, transform.position + spawnOffset, Quaternion.identity);
        _cube = gameObject.GetComponent<CubeController>();
        _cube.spawnerController = this;
        _isSpawning = false;
    }
}
