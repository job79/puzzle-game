using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Animator))]
public class CubeWallController : WallController
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cube"))
        {
            CubeController cube = other.GetComponent<CubeController>();
            cube.Respawn();
        }
    }
}
