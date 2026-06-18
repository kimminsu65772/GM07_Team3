using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    private UpgradeData upgradeData;
    private Transform owner;
    private Transform target;

    [SerializeField] private float attackInterval = 1.0f;
    private float timer = 0.0f;


    //데이터 가져오기
    public virtual void Init(UpgradeData upgradeData, Transform owner)
    {
        this.upgradeData = upgradeData;
        this.owner = owner;
        timer = 0.0f;
    }

    private void Update()
    {
        if (upgradeData == null || owner == null)
        {
            return;
        }

        timer += Time.deltaTime;

        //공격속도
        if (timer >= attackInterval)
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

        if (upgradeData.BulletPrefab == null)
        {
            return;
        }

        GameObject attackObj = Instantiate(
            upgradeData.BulletPrefab,
            attackPosition,
            Quaternion.LookRotation(direction)
        );

        AttackObject attackObject = attackObj.GetComponent<AttackObject>();

        if (attackObject != null)
        {
            attackObject.Init(upgradeData.Value, direction);
        }
    }

    //투사체 방향
    protected virtual Vector3 GetAttackDirection()
    {
        if (target == null)
        {
            return owner.forward.normalized;
        }

        Vector3 direction = target.position - owner.position;
        return direction.normalized;
    }

    //투사체 생성 위치
    protected virtual Vector3 GetSpawnPosition(Vector3 direction)
    {
        return owner.position + direction;
    }
}