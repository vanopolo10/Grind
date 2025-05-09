using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

[RequireComponent(typeof(SpawnSystem))]
public class RoomManager : MonoBehaviour
{
    [SerializeField] private Gates _gates;

    private SpawnSystem _spawnSystem;
    private int _waveIndex;
    private Character _character;

    private bool _canLeave;
    private Coroutine _spawnCoroutine;
    private List<Enemy> _roomEnemies = new();
    private Wallet _wallet;

    private bool _canBeginNextWave = true;
    
    public event Action RoomEnded;

    private void Awake()
    {
        _spawnSystem = GetComponent<SpawnSystem>();
    }

    private void OnEnable()
    {
        _gates.HeroEntered += OnHeroLeaved;
        _spawnSystem.WaveSpawned += ChangeWave;
    }

    private void OnDisable()
    {
        _gates.HeroEntered -= OnHeroLeaved;
        _spawnSystem.WaveSpawned -= ChangeWave;
    }

    public void Play(Character character, Wallet wallet)
    {
        _character = character;
        _wallet = wallet;
        _gates.Activate();
        BeginWave(_character);
    }

    public void SubscribeEnemy(Enemy enemy)
    {
        _roomEnemies.Add(enemy);
        enemy.Died += RemoveEnemy;
    }

    public void EndBattle()
    {
        _canLeave = true;
        _spawnSystem.Stop();
        _canBeginNextWave = false;

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        if (_roomEnemies.Count > 0)
        {
            foreach (var enemy in _roomEnemies)
                Destroy(enemy.gameObject);

            _roomEnemies = new List<Enemy>();
        }
        
        TryFinishRoom();
    }

    private void BeginWave(Character character)
    {
        _spawnCoroutine = StartCoroutine(_spawnSystem.Spawn(_waveIndex, character, _wallet));
    }

    private void ChangeWave(SpawnSystem.Wave waveSpawned)
    {
        _waveIndex++;

        if (_waveIndex < _spawnSystem.WavesCount && _canBeginNextWave)
            StartCoroutine(WaitForNextWaveAndBegin(waveSpawned.NextWaveDelay));
        else
            _canLeave = true;
    }

    private IEnumerator WaitForNextWaveAndBegin(float time)
    {
        yield return new WaitForSeconds(time);
        BeginWave(_character);
    }

    private void RemoveEnemy(Enemy enemy, int money)
    {
        enemy.Died -= RemoveEnemy;
        _roomEnemies.Remove(enemy);
        TryFinishRoom();
    }

    private void TryFinishRoom()
    {
        if (_canLeave && _roomEnemies.Count == 0)
            _gates.Deactivate();
    }

    private void OnHeroLeaved()
    {
        RoomEnded?.Invoke();
    }
}
