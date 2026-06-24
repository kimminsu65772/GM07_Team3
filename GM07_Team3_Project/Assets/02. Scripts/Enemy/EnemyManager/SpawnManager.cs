using UnityEngine;
using UnityEngine.AI; // NavMesh 사용위해 필요

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 15f; // 플레이어 주변 반지름 15m
    [SerializeField] private float spawnInterval = 1f; // 1초당 생성

    private float spawnTimer;

    private void Update()
    {
        // 플레이어가 없을때 예외처리
        if (player == null) return;

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        spawnTimer += Time.deltaTime; // 매 프레임 시간 누적

        if (spawnTimer < spawnInterval)
        {
            return;
        }

        spawnTimer = 0f;

        MeleeEnemy enemy =
            EnemyPoolManager.Instance.GetMeleeEnemy();

        enemy.transform.position = GetSpawnPosition();
    }

    private Vector3 GetSpawnPosition()
    {
        // 반지름 15m 원 내부에서 랜덤 좌표 생성
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // 플레이어 주변 적 생성 위치 계산
        return player.position + 
            new Vector3(randomDirection.x, 
            0f, randomDirection.y) * spawnRadius;
    }
}
