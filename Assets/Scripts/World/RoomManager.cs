using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpawnManager))]
public class RoomManager : MonoBehaviour
{
    [SerializeField] private Vector2 _leftTopCorner;
    [SerializeField] private Vector2 _rightBottomCorner;
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private Door _door;
    [SerializeField] private Counter _counter;

    private SpawnManager _spawnManager;
    private int _waveIndex = 0;
    
    public event Action RoomStarted;
    public event Action RoomEnded;

    private void Awake()
    {
        _spawnManager = GetComponent<SpawnManager>();
    }

    private void OnEnable()
    {
        _counter.TimeEnded += ActivateDoor;
        _door.HeroEntered += OnHeroEntered;
        _spawnManager.WaveSpawned += ChangeWave;
    }
    
    private void OnDisable()
    {
        _counter.TimeEnded -= ActivateDoor;
        _door.HeroEntered -= OnHeroEntered;
        _spawnManager.WaveSpawned -= ChangeWave;
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
        
        BeginWave();
    }

    private void BeginWave()
    {
        StartCoroutine(_spawnManager.Spawn(_waveIndex));
    }

    private void ChangeWave(SpawnManager.Wave waveSpawned)
    {
        _waveIndex++;

        if (_waveIndex < _spawnManager.WavesCount)
            StartCoroutine(WaitForNextWaveAndBegin(waveSpawned.NextWaveDelay));
    }

    private IEnumerator WaitForNextWaveAndBegin(float time)
    {
        yield return new WaitForSeconds(time);
        BeginWave();
    }
    
    private void ActivateDoor()
    {
        _door.Activate();
    }

    private void OnHeroEntered()
    {
        RoomEnded?.Invoke();
    }
}
