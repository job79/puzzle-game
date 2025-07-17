using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MapIndicatorController updates the UI to show the current map.
/// </summary>
[RequireComponent(typeof(Image))]
public class MapIndicatorController : MonoBehaviour
{
    [Header("Configuration")] 
    public Color[] colors =
    {
        new(34f / 255, 139f / 255, 230f / 255),
        new(250f / 255, 82f / 255, 82f / 255),
        new(64f / 255, 192f / 255, 87f / 255)
    };

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.color = colors[GameManager.Instance.currentMapIdx];
    }
    
    private void OnEnable()
    {
        GameManager manager = GameManager.Instance;
        manager.onMapSwitch.AddListener(OnMapSwitch);
    }

    private void OnDisable()
    {
        GameManager manager = GameManager.Instance;
        manager.onMapSwitch.RemoveListener(OnMapSwitch);
    }
    
    private void OnMapSwitch(int idx, Vector3 o)
    {
        _image.color = colors[idx];
    }
}