using UnityEngine;
using System.Collections;

/// <summary>
/// SensorController is a sensor that activates or deactivates
/// based on collisions with a cube or player.
/// </summary>
public class SensorController : MonoBehaviour
{
   private int _collisionEnterCount;
   private SpriteRenderer _spriteRenderer;
   private Coroutine _activationCoroutine;
   
   [Header("Configuration")]
   public Color activatedColor = new (44 / 255f, 127 / 255f, 33/ 255f);
   public Color deactivatedColor = new (218 / 255f, 65 / 255f, 60 / 255f);
   public float activationDelay = 0.2f;
   
   public bool requiresCube;
   public int requiredCubeOriginMap = -1;
   
   public Activateable[] activateables;

   private void Start()
   {
      _spriteRenderer = GetComponent<SpriteRenderer>();
      _spriteRenderer.color = deactivatedColor;
   }
   
   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (!IsMatch(collision)) return;
      _collisionEnterCount++;
      if (_collisionEnterCount != 1) return;
      
      _activationCoroutine ??= StartCoroutine(ActivateAfterDelay());
   }

   private void OnCollisionExit2D(Collision2D collision)
   {
      if (!IsMatch(collision)) return;
      _collisionEnterCount--;
      if (_collisionEnterCount != 0) return;
      
      if (_activationCoroutine != null)
      {
         StopCoroutine(_activationCoroutine);
         _activationCoroutine = null;
      }
      else
      {
         _spriteRenderer.color = deactivatedColor;
         foreach (Activateable activateable in activateables)
         {
            activateable.SetActivation(false);
         }
      }
   }

   private bool IsMatch(Collision2D collision)
   {
      if (requiresCube)
      {
         if (!collision.gameObject.CompareTag("Cube")) return false;
         var originMap = collision.gameObject.GetComponent<CubeController>().originMap;
         if (requiredCubeOriginMap != -1 && originMap != requiredCubeOriginMap) return false;
      }

      return true;
   }
   
   private IEnumerator ActivateAfterDelay()
   {
      yield return new WaitForSeconds(activationDelay);
      _activationCoroutine = null;
      
      _spriteRenderer.color = activatedColor;
      foreach (Activateable activateable in activateables)
      {
         activateable.SetActivation(true);
      }
   }
}