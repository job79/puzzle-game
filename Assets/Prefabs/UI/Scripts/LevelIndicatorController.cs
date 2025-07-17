using TMPro;
using UnityEngine;

/// <summary>
/// LevelIndicatorController updates the UI text to show the current level.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelIndicatorController : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    
    private void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _textMeshPro.text = $"Level {TransitionManager.Instance.currentLevel}";
    }
}
