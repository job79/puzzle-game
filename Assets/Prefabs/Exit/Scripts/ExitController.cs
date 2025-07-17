using UnityEngine;
using System.Collections;

/// <summary>
/// ExitController handles the logic of the exit door.
/// </summary>
[RequireComponent(typeof(Animator))]
public class ExitController : Activateable
{
    [Header("Configuration")]
    public float playerExitDelay = 1f;
    
    private Animator _animator;
    private Coroutine _transitionCoroutine;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            _transitionCoroutine ??= StartCoroutine(WaitAndTransition());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = null;
        }
    }

    private IEnumerator WaitAndTransition()
    {
        yield return new WaitForSeconds(playerExitDelay);
        TransitionManager.Instance.NextLevel();
        _transitionCoroutine = null;
    }
    
    protected override void OnActivation()
    {
        _animator.SetBool("IsActive", true);
    }

    protected override void OnDeactivation()
    {
        _animator.SetBool("IsActive", false);
    }
}
