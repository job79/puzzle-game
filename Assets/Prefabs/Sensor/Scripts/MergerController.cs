using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// MergerController is a sensor that activates or deactivates
/// when multiple connections are active.
/// </summary>
public class MergerController : Activateable
{
    [Header("Configuration")]
    public Color activatedColor = new(148 / 255f, 209 / 255f, 124 / 255f);
    public Color deactivatedColor = new(219 / 255f, 219 / 255f, 219 / 255f);
    public int connectionCount = 2;
    public Activateable[] activateables;

    private int _activeConnectionCount = 0;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnActivation()
    {
        _activeConnectionCount++;
        if (_activeConnectionCount < connectionCount) return;
        
        _spriteRenderer.color = activatedColor;  
        foreach (Activateable activatable in activateables)
        {
            activatable.SetActivation(true);
        }
    }

    protected override void OnDeactivation()
    {
        _activeConnectionCount = math.max(0, _activeConnectionCount - 1);
        if (_activeConnectionCount >= connectionCount) return;
        
        _spriteRenderer.color = deactivatedColor;
        foreach (Activateable activatable in activateables)
        {
            activatable.SetActivation(false);
        }
    } 
}
