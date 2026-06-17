using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData", 
    menuName = "Enemy/EnemyData")]

public class EnemyData : ScriptableObject
{
    [SerializeField] private float maxHp = 100f; //체력

    [SerializeField] private float attackPower = 10f; //공격력

    [SerializeField] private float defensePower = 3f; //방어력

    [SerializeField] private float attackSpeed = 1f; //공속

    [SerializeField] private float moveSpeed = 3f; //이속

    [SerializeField] private float attackRange = 1.5f; //공격범위

    [SerializeField] private float detectRange = 10f; //탐지범위

    public float MaxHp => maxHp;
    public float AttackPower => attackPower;
    public float DefensePower => defensePower;
    public float AttackSpeed => attackSpeed;
    public float MoveSpeed => moveSpeed;
    public float AttackRange => attackRange;
    public float DetectRange => detectRange;

}
