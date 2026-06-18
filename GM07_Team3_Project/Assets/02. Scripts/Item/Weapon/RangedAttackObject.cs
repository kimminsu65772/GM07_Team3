using UnityEngine;

public class RangedAttackObject : AttackObject
{
    [Header("투사체 속도")]
    [SerializeField] private float speed = 10.0f;
    

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime; 
    }
}
