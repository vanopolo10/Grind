using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpawnSystem))]
public class RoomManager : MonoBehaviour
{
    [SerializeField] private Vector2 _leftTopCorner;
    [SerializeField] private Vector2 _rightBottomCorner;
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private Door _door;
    [SerializeField] private Counter _counter;

    private SpawnSystem _spawnSystem;
    private int _waveIndex = 0;
    private Character _character;
    
    public event Action RoomStarted;
    public event Action RoomEnded;

    private void Awake()
    {
        _spawnSystem = GetComponent<SpawnSystem>();
    }

    private void OnEnable()
    {
        _counter.TimeEnded += ActivateDoor;
        _door.HeroEntered += OnHeroEntered;
        _spawnSystem.WaveSpawned += ChangeWave;
    }
    
    private void OnDisable()
    {
        _counter.TimeEnded -= ActivateDoor;
        _door.HeroEntered -= OnHeroEntered;
        _spawnSystem.WaveSpawned -= ChangeWave;
    }

    public (Vector2 leftTopCorner, Vector2 rightBottomCorner) GetCorners()
    {
        return (_leftTopCorner, _rightBottomCorner);
    }

    public Counter GetCounter()
    {
        return _counter;
    }
    
    public void Play(Character character)
    {
        _character = character;
        
        RoomStarted?.Invoke();
        _door.Deactivate();

        character.transform.position = new Vector3(_spawnpoint.position.x, _spawnpoint.position.y + character.transform.localScale.y, _spawnpoint.position.z);
        
        BeginWave(_character);
    }

    private void BeginWave(Character character)
    {
        StartCoroutine(_spawnSystem.Spawn(_waveIndex, character));
    }

    private void ChangeWave(SpawnSystem.Wave waveSpawned)
    {
        _waveIndex++;

        if (_waveIndex < _spawnSystem.WavesCount)
            StartCoroutine(WaitForNextWaveAndBegin(waveSpawned.NextWaveDelay));
    }

    private IEnumerator WaitForNextWaveAndBegin(float time)
    {
        yield return new WaitForSeconds(time);
        BeginWave(_character);
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
