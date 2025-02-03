using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private List<Wave> _waves;

    public event Action<Wave> WaveSpawned;

    public int WavesCount => _waves.Count;

    public IEnumerator Spawn(int currentWaveIndex, Character character)
    {
        Wave wave = _waves[currentWaveIndex];
        
        WaitForSeconds spawnDelay = new WaitForSeconds(wave.SpawnDelay);
        
        int spawnHeightCoefficient = 2;

        for (int i = 0; i < wave.EnemyCount; i++)
        {
            
            Vector3 spawnPosition = _spawners[Random.Range(0, _waves.Count)].EnemySpawnPoint + new Vector3(0, wave.EnemyPrefab.transform.localScale.y / spawnHeightCoefficient, 0);
            
            Enemy enemy = Instantiate(wave.EnemyPrefab, spawnPosition, new Quaternion());
            
            enemy.StartFight(character);

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
