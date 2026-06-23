using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Spawn Point")]

    // 적이 생성될 위치들을 인스펙터에서 등록
    [SerializeField] private Transform[] spawnPoints;
    //웨이브 중복생성 방지용
    private bool wave1Spawned;
    // 2 웨이브 생성여부 저장
    private bool wave2Spawned;
    // 3 웨이브 생성여부 저장
    private bool wave3Spawned;

    private void OnEnable()
    {
        if (TimeManagerTest.Instance != null)
        {
            //타임매니저가 시간을 알려줄때 체크웨이브 실행
            TimeManagerTest.Instance.OnTimeChanged += CheckWave;
        }
    }
    private void OnDisable()
    {
        if (TimeManagerTest.Instance != null)
        {
            // 오브젝트 비활성화 시 이벤트 해제
            TimeManagerTest.Instance.OnTimeChanged -= CheckWave;
        }
    }

    private void CheckWave(int remainingTime)
    {
        // 남은 시간 1000초 이하, 1웨이브 생성이 아직 안된 상태면
        if (!wave1Spawned && remainingTime <= 1190)
        {
            SpawnMeleeWave(10); // 근거리 적 10마리 생성

            wave1Spawned = true; // 생성 이후 재생성 방지용 true 저장
        }

        if (!wave2Spawned && remainingTime <= 1188)
        {
            SpawnMeleeWave(10);

            wave2Spawned = true;
        }

        if (!wave3Spawned && remainingTime <= 1186)
        {
            SpawnRangedWave(10); // 원거리 적 10마리 생성

            wave3Spawned = true;
        }
    }

    private void SpawnMeleeWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 오브젝트 풀에서 근거리 적을 하나 꺼낸다
            MeleeEnemy enemy = 
                EnemyPoolManager.Instance.GetMeleeEnemy();

            // 등록된 스폰 포인트 중 랜덤으로 선택함
            Transform spawnPoint = 
                spawnPoints[Random.Range(0, spawnPoints.Length)];

            // 적 위치를 스폰 포인트 위치로 이동
            enemy.transform.position = spawnPoint.position;
        }
    }

    private void SpawnRangedWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RangedEnemy enemy =
                EnemyPoolManager.Instance.GetRangedEnemy();

            Transform spawnPoint =
                spawnPoints[Random.Range(0, spawnPoints.Length)];

            enemy.transform.position = spawnPoint.position;

        }
    }

}
