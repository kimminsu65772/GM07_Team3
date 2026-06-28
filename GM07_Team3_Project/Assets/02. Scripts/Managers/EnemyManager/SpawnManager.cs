using System;
using UnityEngine;
using UnityEngine.AI; // NavMesh 사용위해 필요
using Random = UnityEngine.Random;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStatController playerStatController;

    [System.Serializable]
    private class WaveSetting
    {
        public EnemyData[] enemyDatas;
        public float spawnInterval;
    }

    [Header("Enemy Data")]
    [SerializeField] private WaveSetting[] waves;

    [Header("Spawn")]
    [SerializeField] private float spawnRadius = 15f; // 플레이어 주변 반지름 15m
    [SerializeField] private float spawnInterval = 1f; // 1초당 생성

    [Header("Wave")]
    [SerializeField] private float waveDuration = 30f;

    // 웨이브 변경 이벤트
    public event Action<int> OnWaveChanged;

    // 현재 웨이브
    public int CurrentWave => currentWave;

    private int currentWave = 1;

    private float spawnTimer;
    private float waveTimer;

    private void Start()
    {
        if (waves.Length > 0)
        {
            spawnInterval = waves[0].spawnInterval;
        }

        // 게임 시작 시 웨이브1 이벤트 알림
        OnWaveChanged?.Invoke(currentWave);
    }

    private void Update()
    {
        // 플레이어가 없을때 예외처리
        if (player == null)
        {
            return;
        }

        UpdateWave();

        SpawnEnemy();
    }

    private void UpdateWave()
    {
        waveTimer += Time.deltaTime;

        if (waveTimer < waveDuration)
        {
            return;
        }

        waveTimer = 0f;

        currentWave++;

        // 웨이브 변경 알림
        OnWaveChanged?.Invoke(currentWave);

        if (currentWave - 1 < waves.Length)
        {
            spawnInterval = waves[currentWave - 1].spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
        {
            return;
        }

        spawnTimer = 0f;

        if (!TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            return;
        }

        int waveIndex = currentWave - 1;

        if (waveIndex >= waves.Length)
        {
            waveIndex = waves.Length - 1;
        }

        WaveSetting currentSetting = waves[waveIndex];

        if (currentSetting.enemyDatas.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, currentSetting.enemyDatas.Length);
        EnemyData data = currentSetting.enemyDatas[randomIndex];

        Enemy enemy = EnemyPoolManager.Instance.GetEnemy(data);

        enemy.SetEnemyData(data);
        enemy.Initialize(player, playerStatController);

        if (!enemy.WarpToNavMesh(spawnPosition))
        {
            EnemyPoolManager.Instance.ReturnEnemy(enemy);
        }
    }

    private bool TryGetSpawnPosition(out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;

        Vector2 randomDirection =
            Random.insideUnitCircle.normalized;

        Vector3 targetPosition = 
            player.position + new Vector3(
                randomDirection.x, 0f, randomDirection.y) 
            * spawnRadius;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(targetPosition,
            out hit, 5f, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
            return true;
        }

        return false;
    }

}
