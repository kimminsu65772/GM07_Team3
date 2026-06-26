using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class WeaponBase : MonoBehaviour
{
    private UpgradeOption option;
    private UpgradeData upgradeData;
    private Transform owner;
    private float value;
    private Transform target;
    private ItemStatManager itemStatManager;

    [SerializeField] private float spawnDistance = 1.0f;
    [SerializeField] private float spawnHeight = 1.0f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float targetSerchRadius = 500.0f;
    [SerializeField] private float attackInterval = 0.2f;

    private float timer = 0.0f;


    //데이터 가져오기
    public virtual void Init(UpgradeOption option, Transform owner)
    {
        this.upgradeData = option.Data;
        this.option = option;
        this.owner = owner;
        this.value = option.Value;

        timer = 0.0f;

        itemStatManager = owner.GetComponent<ItemStatManager>();


        if (targetLayer.value == 0)
        {
            targetLayer = LayerMask.GetMask("Target");
        }
    }

    private void Update()
    {
        if (upgradeData == null || owner == null)
        {
            return;
        }

        timer += Time.deltaTime;

        //공격속도
        float finalAttackInterval = GetFinalAttackInterval();

        if (timer >= finalAttackInterval)
        {
            timer = 0.0f;
            Attack();
        }
    }

    //공격
    protected virtual void Attack()
    {
        //만들어둔 방향 위치 사용
        Vector3 direction = GetAttackDirection();
        Vector3 attackPosition = GetSpawnPosition(direction);

        //오브젝트풀이 널인지 검사
        if (ObjectPoolManager.Instance == null) return;
        //오브젝트 꺼내오기
        GameObject attackObj = ObjectPoolManager.Instance.GetAttackObject(upgradeData.BulletPrefab);
        if (attackObj == null) return;


        //위치와 회전 세팅
        attackObj.transform.position = attackPosition;
        attackObj.transform.rotation = Quaternion.LookRotation(direction);

        AttackObject attackObject = attackObj.GetComponent<AttackObject>();

        if (attackObject != null)
        {
            float finalDamage = GetFinalDamage();
            attackObject.Init(finalDamage, direction);
        }
    }
    //공격 속도 증가 적용
    private float GetFinalAttackInterval()
    {
        if (itemStatManager == null)
        {
            return attackInterval;
        }

        float attackSpeedMultiplier = 1.0f + itemStatManager.AttackSpeedBonus;

        attackSpeedMultiplier = Mathf.Max(0.1f, attackSpeedMultiplier);

        return attackInterval / attackSpeedMultiplier;
    }


    //증가된 데미지/ 크리데미지 계산 및 적용 시키기
    private float GetFinalDamage()
    {
        float finalDamage = value;

        if (itemStatManager != null)
        {
            finalDamage += itemStatManager.DamageBonus;

            float criticalChance = itemStatManager.CriticalChanceBonus;

            if (Random.Range(0.0f, 100.0f) < criticalChance)
            {
                finalDamage *= 2.0f;
            }
        }

        return finalDamage;
    }

    //투사체 방향
    protected virtual Vector3 GetAttackDirection()
    {
        target = FindNearestTarget();

        if (target == null)
        {
            Vector3 forward = owner.forward;

            if (forward == Vector3.zero)
            {
                return Vector3.forward;
            }

            return forward.normalized;
        }

        Vector3 targetPosition = GetTargetAimPosition(target);
        Vector3 startPosition = GetFireStartPosition();



        Vector3 direction = targetPosition - startPosition;

        if (direction == Vector3.zero)
        {
            return owner.forward.normalized;
        }

        return direction.normalized;

    }

    //적 몸통 중앙으로 투사체 보내기
    private Vector3 GetTargetAimPosition(Transform target)
    {
        Collider targetCollider = target.GetComponentInChildren<Collider>();

        if (targetCollider != null)
        {
            return targetCollider.bounds.center;
        }

        return target.position + Vector3.up * 1.0f;
    }

    private Vector3 GetFireStartPosition()
    {
        return owner.position + Vector3.up * spawnHeight;
    }

    //투사체 생성 위치
    protected virtual Vector3 GetSpawnPosition(Vector3 direction)
    {
        return owner.position + Vector3.up * spawnHeight + direction * spawnDistance;
    }

    private Transform FindNearestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(owner.position, targetSerchRadius, targetLayer);

        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach(Collider hit in hits)
        {
            float distance = Vector3.Distance(owner.position,hit.transform.position);

            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = hit.transform;
            }
        }
        return nearestTarget;
    }
}