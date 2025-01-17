using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Vector2 _leftTopCorner;
    [SerializeField] private Vector2 _rightBottomCorner;
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private Door _door;
    [SerializeField] private Counter _counter;
    
    private bool _isActive = false;

    public event Action RoomStarted;
    public event Action RoomEnded;
    
    private void OnEnable()
    {
        _counter.TimeEnded += ActivateDoor;
        _door.HeroEntered += OnHeroEntered;
        _isActive = true;
    }
    
    private void OnDisable()
    {
        _counter.TimeEnded -= ActivateDoor;
        _door.HeroEntered -= OnHeroEntered;
        _isActive = false;
    }

    public (Vector2 leftTopCorner, Vector2 rightBottomCorner) GetCorners()
    {
        return (_leftTopCorner, _rightBottomCorner);
    }

    public Counter GetCounter()
    {
        return _counter;
    }
    
    public void Play(TouchInput player)
    {
        RoomStarted?.Invoke();
        _door.Deactivate();

        player.transform.position = new Vector3(_spawnpoint.position.x, _spawnpoint.position.y + player.transform.localScale.y, _spawnpoint.position.z);
        
        foreach (var spawner in _spawners)
            spawner.BeginSpawn();
    }
    
    private void ActivateDoor()
    {
        _door.Activate();
    }

    private void OnHeroEntered()
    {
        foreach (var spawner in _spawners)
            spawner.enabled = false;
        
        RoomEnded?.Invoke();
    }
}
