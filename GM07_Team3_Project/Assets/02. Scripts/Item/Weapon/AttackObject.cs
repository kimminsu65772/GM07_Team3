using UnityEngine;

public class AttackObject : MonoBehaviour
{
    protected float damage;
    protected Vector3 direction;
    protected float lifeTime;

    public virtual void Init(float  damage, Vector3 direction, float lifeTime)
    {
        this.damage = damage;
        this.direction = direction;
        this.lifeTime = lifeTime;
    }

    //적에게 주는 데미지
    //public virtual void Onhit(Enemy enemy)
    //{
    //   //적에게 데미지
    //}

    //일단 리턴으로 사용 이후 오브젝트풀링으로 관리
    protected virtual void Return()
    {
        Destroy(gameObject);
    }

}
