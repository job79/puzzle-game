using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// HelpTextManager manages the display of help text messages in the UI.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class HelpTextManager : MonoBehaviour
{
    public static HelpTextManager Instance => _instance;
    private static HelpTextManager _instance;
    
    [Header("Display Settings")]
    public float displayTime = 5f;
    public float fadeDuration = .5f;

    private Queue<string> _helpMessages;
    private TextMeshProUGUI _textMesh;
    private Coroutine _currentCoroutine;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void DisplayHelpText(string[] text)
    {
        _helpMessages = new Queue<string>(text);
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(CycleMessages());
    }
    
    private IEnumerator CycleMessages()
    {
        while (_helpMessages.Count > 0)
        {
            _textMesh.text = _helpMessages.Dequeue();
            _textMesh.alpha = 1f;
            yield return new WaitForSeconds(displayTime);
        }
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float startAlpha = _textMesh.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            _textMesh.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        _textMesh.alpha = 0f;
    }
}
