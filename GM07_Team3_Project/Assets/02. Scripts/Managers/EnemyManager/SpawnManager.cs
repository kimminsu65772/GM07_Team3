using UnityEngine;
using UnityEngine.AI; // NavMesh 사용위해 필요

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStatController playerStatController;

    [SerializeField] private float spawnRadius = 15f; // 플레이어 주변 반지름 15m
    [SerializeField] private float spawnInterval = 1f; // 1초당 생성

    private NavMeshAgent agent;

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

        // 계산 좌표를 spawnPosition에 저장
        if (!TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            return;
        }

        if (Random.value < 0.7f)
        {
            MeleeEnemy enemy =
            EnemyPoolManager.Instance.GetMeleeEnemy();

            enemy.Initialize(player, playerStatController);

            // NavMesh에 bake된 위치로 이동을 시도하고 실패한 경우 pool로 반환함.
            if (!enemy.WarpToNavMesh(spawnPosition))
            {
                EnemyPoolManager.Instance.ReturnMeleeEnemy(enemy);
            }
        }
        else
        {
            RangedEnemy enemy =
                EnemyPoolManager.Instance.GetRangedEnemy();

            enemy.Initialize(player, playerStatController);

            if (!enemy.WarpToNavMesh(spawnPosition))
            {
                EnemyPoolManager.Instance.ReturnRangedEnemy(enemy);
            }
        }

    }

    // Vector3 좌표 계산
    private bool TryGetSpawnPosition(out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;

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
            spawnPosition = hit.position;
            return true;
        }

        return false;
    }
}
