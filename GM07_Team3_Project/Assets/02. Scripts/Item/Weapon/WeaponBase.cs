using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    private UpgradeOption option;
    private UpgradeData upgradeData;
    private Transform owner;
    private Transform target;
    private float value;
    
    
    [SerializeField] private float attackInterval = 1.0f;

    private float timer = 0.0f;


    //데이터 가져오기
    public virtual void Init(UpgradeOption option, Transform owner)
    {
        this.upgradeData = option.Data;
        this.option = option;
        this.owner = owner;
        this.value = option.Value;
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

        //오브젝트풀이 널인지 검사
        if (ObjectPoolManager.Instance == null) return;
        //오브젝트 꺼내오기
        GameObject attackObj = ObjectPoolManager.Instance.GetAttackObject(upgradeData.BulletPrefab);

       
        //위치와 회전 세팅
        attackObj.transform.position = attackPosition;
        attackObj.transform.rotation = Quaternion.LookRotation(direction);

        AttackObject attackObject = attackObj.GetComponent<AttackObject>();
      
        if(attackObject != null)
        {
            attackObject.Init(value, direction);
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