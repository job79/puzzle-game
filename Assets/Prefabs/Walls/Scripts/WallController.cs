using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Animator))]
public class WallController : Activateable
{
    [Header("Configuration")]
    public Color activatedColor = new(44 / 255f, 127 / 255f, 33 / 255f);
    public Color deactivatedColor = new(218 / 255f, 65 / 255f, 60 / 255f);
    
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    protected override void OnActivation()
    {
        _spriteRenderer.color = activatedColor;  
        _collider.enabled = false;
        _animator.SetBool("IsActive", true);
    }

    protected override void OnDeactivation()
    {
        _spriteRenderer.color = deactivatedColor;
        _collider.enabled = true;
        _animator.SetBool("IsActive", false);
    }
}
