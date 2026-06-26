using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "Enemy/EnemyData")]

public class EnemyData : ScriptableObject
{
    [Header("HP")]
    [SerializeField] private float maxHp = 100f; //최대 체력

    [Header("Combat")]
    [SerializeField] private float attackPower = 10f; //공격력
    [SerializeField] private float defensePower = 3f; //방어력
    [SerializeField] private float attackSpeed = 1f; //공격속도
    [SerializeField] private float attackRange = 10f; //원거리 공격 사정거리
    [SerializeField] private EnemyBullet bulletPrefab; //원거리 투사체

    [Header("Exp Reward")]
    [SerializeField] private int experience = 10;

    public int Experience => experience;

    public EnemyBullet BulletPrefab => bulletPrefab;

    public float AttackRange => attackRange;


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f; //이동속도

    public float MaxHp => maxHp;
    public float AttackPower => attackPower;
    public float DefensePower => defensePower;
    public float AttackSpeed => attackSpeed;
    public float MoveSpeed => moveSpeed;
}