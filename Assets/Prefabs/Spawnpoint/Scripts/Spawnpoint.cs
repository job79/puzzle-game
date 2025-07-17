using UnityEngine;

/// <summary>
/// Spawnpoint marks the start location of the player and
/// is responsible for respawning the player when needed.
/// </summary>
public class Spawnpoint : MonoBehaviour
{
    [Header("Configuration")]
    public Vector2 offset = new (0f, 0.5f);
    public int mapIdx;
    
    private GameObject _player;
    
    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        Respawn();
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        Respawn();
    }

    public void Respawn()
    {
        _player.transform.position = new Vector3(
            transform.position.x + offset.x,
            transform.position.y + offset.y,
            _player.transform.position.z
        );
        _player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        GameManager gameManager = GameManager.Instance;
        if (gameManager.currentMapIdx != mapIdx)
        {
            gameManager.currentMapIdx = mapIdx;
            gameManager.SwitchMap(mapIdx);
        }
        gameManager.onRespawn.Invoke();
    }
}
