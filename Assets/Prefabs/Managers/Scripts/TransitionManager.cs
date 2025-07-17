using System.Collections;
using UnityEngine;

/// <summary>
/// TransitionManager is a singleton that manages the transitions
/// between levels in the game.
/// </summary>
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance => _instance;
    private static TransitionManager _instance;
    
    public int currentLevel;
    
    private Animator _animator;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        _animator = GetComponentInChildren<Animator>();
    }

    public void NextLevel()
    {
        ToLevel(currentLevel + 1);
    }
    
    public void ToLevel(int level)
    {
        currentLevel = level;
        var scene = $"Level {level}";
        if (Application.CanStreamedLevelBeLoaded(scene)) 
            StartCoroutine(LoadLevel(scene));
        else
        {
            currentLevel = 0;
            StartCoroutine(LoadLevel("Menu"));
        }
    }

    private IEnumerator LoadLevel(string levelName)
    {
        _animator.SetTrigger("Start");
        yield return new WaitForSeconds(.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
}
