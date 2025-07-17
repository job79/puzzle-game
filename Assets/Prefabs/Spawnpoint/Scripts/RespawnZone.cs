using UnityEngine;

/// <summary>
/// RespawnPlane is a trigger zone that respawns the player or cube
/// when they enter the zone.
/// </summary>
public class RespawnPlane : MonoBehaviour
{
    [Header("Configuration")]
    public Spawnpoint spawnpoint;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            spawnpoint.Respawn();
        }
        
        if (other.gameObject.CompareTag("Cube")) {
            other.GetComponent<CubeController>().Respawn();
        }
    }
}
