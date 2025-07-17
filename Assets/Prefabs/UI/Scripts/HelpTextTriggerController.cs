using UnityEngine;

/// <summary>
/// HelpTextTriggerController is responsible for displaying
/// help text when the player enters a trigger area.
/// </summary>
public class HelpTextTriggerController : MonoBehaviour
{
    [Header("Configuration")]
    public string[] helpText;
    public float cooldown = 60f;

    private float _lastTriggerTime = -Mathf.Infinity;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - _lastTriggerTime >= cooldown)
            {
                HelpTextManager.Instance.DisplayHelpText(helpText);
                _lastTriggerTime = Time.time;
            }
        }
    }
}
