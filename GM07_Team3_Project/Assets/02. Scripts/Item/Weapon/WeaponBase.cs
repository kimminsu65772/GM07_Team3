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
    }

    private void Update()
    {
        if (upgradeData || owner == null)
        {
            return;
        }

        timer += Time.deltaTime;
        if(timer > attackInterval)
        {
            timer = 0.0f;
            Attack();
        }

     
    }
    
    protected virtual void Attack()
    {
        Vector3 direction = GetAttackDirection();
        Vector3 attackPosition = GetSpawnPosition(direction);

        GameObject attackObj = Instantiate(upgradeData.BulletPrefab,
            attackPosition,
            Quaternion.LookRotation(direction));

        AttackObject attackObject = attackObj.GetComponent<AttackObject>();

        if(attackObject != null ) 
            {
            attackObject.Init(upgradeData.Value, direction, 5.0f);
            }


    }

    protected virtual Vector3 GetAttackDirection()
    {
        //타겟이 없으면 전방에 발사
        if (target == null)
            return owner.forward.normalized;

        Vector3 direction = target.position - owner.position;
        return direction.normalized;
    }

    protected virtual Vector3 GetSpawnPosition(Vector3 direction)
    {
        return owner.position + direction;
    }



}