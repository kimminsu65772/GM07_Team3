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

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f; //이동속도

    public float MaxHp => maxHp;
    public float AttackPower => attackPower;
    public float DefensePower => defensePower;
    public float AttackSpeed => attackSpeed;
    public float MoveSpeed => moveSpeed;
}