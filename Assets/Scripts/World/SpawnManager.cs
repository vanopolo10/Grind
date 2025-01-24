using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private TouchInput _player;

    public event Action<Wave> WaveSpawned;

    public int WavesCount => _waves.Count;

    public IEnumerator Spawn(int currentWaveIndex)
    {
        Wave wave = _waves[currentWaveIndex];
        
        WaitForSeconds spawnDelay = new WaitForSeconds(wave.SpawnDelay);
        
        int spawnHeightCoefficient = 2;

        for (int i = 0; i < wave.EnemyCount; i++)
        {
            Enemy enemy = Instantiate(wave.EnemyPrefab);
            
            Vector3 spawnPosition = _spawners[Random.Range(0, _waves.Count)].EnemySpawnPoint + new Vector3(0, enemy.transform.localScale.y / spawnHeightCoefficient, 0);
            enemy.transform.position = spawnPosition;
            
            enemy.StartFight(_player.transform);

            yield return spawnDelay;
        }
        
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
