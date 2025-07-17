using UnityEngine;

/// <summary>
/// Activateable is a base class for objects that can
/// be activated or deactivated by a sensor.
/// </summary>
public abstract class Activateable : MonoBehaviour
{
    [Header("Activateable Configuration")]
    public bool inverted;
    public bool isActive;
    
    protected abstract void OnActivation();
    protected abstract void OnDeactivation();
    
    public void SetActivation(bool state)
    {
        isActive = inverted ? !state : state;
        if (isActive) OnActivation();
        else OnDeactivation();
    }

    protected virtual void Start()
    {
        if (isActive) OnActivation();
        else OnDeactivation();
    }
}