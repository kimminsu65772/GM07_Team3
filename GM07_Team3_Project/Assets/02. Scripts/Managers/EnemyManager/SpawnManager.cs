using UnityEngine;
using UnityEngine.AI; // NavMesh 사용위해 필요

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStatController playerStatController;

    [SerializeField] private float spawnRadius = 15f; // 플레이어 주변 반지름 15m
    [SerializeField] private float spawnInterval = 1f; // 1초당 생성


    private float spawnTimer;

    private void Update()
    {
        // 플레이어가 없을때 예외처리
        if (player == null)
        {
            return;
        }

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

        // 계산 좌표를 spawnPosition에 저장
        Vector3 spawnPosition = GetSpawnPosition();

        if (Random.value < 0.7f)
        {
            MeleeEnemy enemy =
            EnemyPoolManager.Instance.GetMeleeEnemy();

            enemy.Initialize(player, playerStatController);

            // 회전값을 (0, 0, 0)으로 강제 초기화 (모델 드르렁 방지)
            enemy.transform.rotation = Quaternion.identity;

            // NavMeshAgent의 위치를 Warp로 안전하게 순간이동
            if (enemy.TryGetComponent<NavMeshAgent>(out var agent))
            {
                agent.Warp(spawnPosition);
            }
            else
            {
                // 에이전트가 없을 때를 대비한 예외처리 유지
                enemy.transform.position = spawnPosition;
            }
        }
        else
        {
            RangedEnemy enemy =
                EnemyPoolManager.Instance.GetRangedEnemy();

            enemy.Initialize(player, playerStatController);

            // 회전값을 (0, 0, 0)으로 강제 초기화 (모델 드르렁 방지)
            enemy.transform.rotation = Quaternion.identity;

            if (enemy.TryGetComponent<NavMeshAgent>(out var agent))
            {
                agent.Warp(spawnPosition);
            }
            else
            {
                enemy.transform.position = spawnPosition;
            }

        }
    }

    // Vector3 좌표 계산
    private Vector3 GetSpawnPosition()
    {
        // 반지름 15m 원 내부에서 랜덤 좌표 생성
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 targetPosition =
            player.position +
            new Vector3(
                randomDirection.x,
                0f,
                randomDirection.y) * spawnRadius;

        
        NavMeshHit hit;

        if (NavMesh.SamplePosition(targetPosition, out hit, 5.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return targetPosition;
    }
}
