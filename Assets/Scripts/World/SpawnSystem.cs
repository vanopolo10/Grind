using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(RoomManager))]
public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Wallet _wallet;

    private RoomManager _roomManager;
    private bool _isSpawning = false;

    public event Action<Wave> WaveSpawned;
    public int WavesCount => _waves.Count;

    private void Awake()
    {
        _roomManager = GetComponent<RoomManager>();
    }

    public void Stop()
    {
        _isSpawning = false;
    }

    public IEnumerator Spawn(int currentWaveIndex, Character character)
    {
        _isSpawning = true;

        Wave wave = _waves[currentWaveIndex];
        WaitForSeconds spawnDelay = new WaitForSeconds(wave.SpawnDelay);
        int spawnHeightCoefficient = 2;

        for (int i = 0; i < wave.EnemyCount; i++)
        {
            if (!_isSpawning) yield break;

            Vector3 spawnPosition = _spawners[Random.Range(0, _spawners.Count)].EnemySpawnPoint 
                                    + new Vector3(0, wave.EnemyPrefab.transform.localScale.y / spawnHeightCoefficient, 0);

            Enemy enemy = Instantiate(wave.EnemyPrefab, spawnPosition, Quaternion.identity);
            _roomManager.SubscribeEnemy(enemy);
            enemy.StartFight(character);
            _wallet.SubscribeEnemy(enemy);

            yield return spawnDelay;
        }

        if (_isSpawning)
            WaveSpawned?.Invoke(wave);
    }

    [Serializable]
    public struct Wave
    {
        public Enemy EnemyPrefab;
        public int EnemyCount;
        public float SpawnDelay;
        public float NextWaveDelay;
    }
}