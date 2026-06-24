using UnityEngine;
using UnityEngine.AI; // NavMesh 사용위해 필요

public class PlayerTest : MonoBehaviour
{
    public float maxHp = 100;
    private float currentHp;

    [Header("Move Settings")]
    public float moveSpeed = 5f;
    private NavMeshAgent agent; // NavMeshAgent 컴포넌트 저장용 변수

    void Awake()
    {
        currentHp = maxHp;

        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = moveSpeed; // 에이전트 기본 속도를 5로 동기화
        }
    }

    private void OnAnimatorMove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // 좌우
        float vertical = Input.GetAxisRaw("Vertical"); // 상하

        // 입력 방향에 따른 이동 벡터 생성
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // 현재 위치에서 원하는 방향과 속도만큼 이동
            Vector3 targetPosition = 
                transform.position + moveDirection * moveSpeed 
                * Time.deltaTime;
            agent.SetDestination(targetPosition);

            // 플레이어가 이동방향을 바라보도록 설정
            Quaternion targetRotation = 
                Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        else
        {
            // 키 입력없을때 멈춤
            if (agent.hasPath)
            {
                agent.ResetPath();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHp <= 0f) return;

        currentHp -= damage;

        if (currentHp <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
            gameObject.SetActive(false);
    }
    
}
