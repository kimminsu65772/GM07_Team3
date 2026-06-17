using UnityEngine;


public class WeaponBase : MonoBehaviour
{
    private UpgradeData upgradeData;
    private Transform owner;
    private Transform target;

    private float attackInterval = 1.0f;

    private float timer = 0.0f;

    private void Update()
    {
        if(upgradeData == null || owner == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= attackInterval )
        {
            timer = 0.0f;
           
        }

        Attack();

    }

    //총알이 날라가는 방향
    protected virtual Vector3 GetAttackDirection()
    {
        //없으면 앞으로
        if (owner == null)
            return Vector3.forward;
        //없으면 앞으로
        if (target == null)
            return owner.forward.normalized;

        Vector3 direction = target.position - owner.position;
        return direction.normalized;
    }

    public virtual void Init(UpgradeData upgradeData, Transform owner)
    {
        this.upgradeData = upgradeData;
        this.owner = owner;
    }

    protected virtual void Attack()
    {
        Vector3 direction = GetAttackDirection();

    }





}