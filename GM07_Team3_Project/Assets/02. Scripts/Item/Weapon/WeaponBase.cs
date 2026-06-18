using UnityEngine;


public class WeaponBase : MonoBehaviour
{
  private UpgradeData upgradeData;
    private Transform owner;
    private Transform target;

    [SerializeField] private float attackInterval = 1.0f;
    private float timer = 0.0f;



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
        if(timer >= attackInterval)
        {
            timer = 0.0f;
            Attack();
        }
    }
    
    protected virtual void Attack()
    {
        //공격 방향
        Vector3 direction = GetAttackDirection();
        //투사체 생성위치
        Vector3 attackPosition = GetSpawnPosition(direction);

        //일단 생성이후 풀링으로 관리
        GameObject attackObj = Instantiate(upgradeData.BulletPrefab,
            attackPosition,
            Quaternion.LookRotation(direction));

        if (upgradeData.BulletPrefab == null)
        {
            return;
        }

        AttackObject attackObject = attackObj.GetComponent<AttackObject>();

        if (attackObject != null)
        {
            attackObject.Init(upgradeData.Value, direction);
        }
    }

    //투사체 발사 방향
    protected virtual Vector3 GetAttackDirection()
    {
        //타겟이 없으면 전방에 발사
        if (target == null)
            return owner.forward.normalized;

        Vector3 direction = target.position - owner.position;
        return direction.normalized;
    }

    //투사체 생성위치
    protected virtual Vector3 GetSpawnPosition(Vector3 direction)
    {
        return owner.position + direction;
    }



}