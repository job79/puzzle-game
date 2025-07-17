using UnityEngine;

/// <summary>
/// ColorChangeController updates the color of a sprite
/// based on its activation state.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ColorChangeController : Activateable
{
    [Header("Configuration")] 
    public Color activatedColor = new(148 / 255f, 209 / 255f, 124 / 255f);
    public Color deactivatedColor = new(219 / 255f, 219 / 255f, 219 / 255f);

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnActivation() => _spriteRenderer.color = activatedColor;
    protected override void OnDeactivation() => _spriteRenderer.color = deactivatedColor;
}