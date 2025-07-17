using UnityEngine;

/// <summary>
/// MenuManager manages the transitions from the menu screen to the game levels.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public void ToLevel(int level)
    {
        TransitionManager.Instance.ToLevel(level);
    }
}
