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
        Vector3 targetPosition = player.position + 
            new Vector3(randomDirection.x, 
            0f, randomDirection.y) * spawnRadius;

        // NavMeshHit 결과를 저장하는 변수
        NavMeshHit hit;

        // 타겟 위치 주변 5m 이내 가장 가까운 NavMesh 바닥 찾기
        if (NavMesh.SamplePosition(targetPosition, out hit, 5.0f, NavMesh.AllAreas))
        {
            // 실제 바닥 좌표를 찾았다면 해당 위치로 반환
            return hit.position;
        }

        // 주변에 NavMesh 바닥이 없으면 targetPosition 반환
        return targetPosition;
    }
}
