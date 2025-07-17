using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// GameManager is a singleton that manages the game state of
/// a single scene.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;
    
    public MapSwitchEvent onMapSwitch;
    public RespawnEvent onRespawn;

    public int currentMapIdx;
    public List<GameObject> maps;
    public List<CubeController> cubes;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public void SwitchMap(int mapIdx)
    {
        Vector3 offset = maps[mapIdx].transform.position - maps[currentMapIdx].transform.position;
        currentMapIdx = mapIdx;
        onMapSwitch?.Invoke(currentMapIdx, offset);
    }
    
    public void ResetCubes()
    {
        foreach (var cube in cubes.Where(x=> x.currentMap == currentMapIdx && x.originMap != x.currentMap))
        {
            cube.Respawn();
        }
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.J)) return;
        SwitchMap((currentMapIdx + 1) % maps.Count);
    }
}

[Serializable]
public class MapSwitchEvent : UnityEvent<int, Vector3>
{
}

[Serializable]
public class RespawnEvent : UnityEvent
{
}
