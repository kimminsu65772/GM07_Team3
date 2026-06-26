using UnityEngine;

public class ItemStatManager : MonoBehaviour
{
    [Header("아이템 공격 스탯")]
    [SerializeField] private float damageBonus = 0.0f;
    [SerializeField] private float attackSpeedBonus = 0.0f;
    [SerializeField] private float criticalChanceBonus = 0.0f;

    public float DamageBonus => damageBonus;
    public float AttackSpeedBonus => attackSpeedBonus;
    public float CriticalChanceBonus => criticalChanceBonus;

    //선택한 이벤트 구독해서 오브젝트 가져오기
    private void OnEnable()
    {
        if (UpgradeEventManager.Instance != null)
        {
            UpgradeEventManager.Instance.OnUpgradeSelected += HandleUpgradeSelected;
        }
    }
    //구독해제 오브젝트 사라지면 끊기
    private void OnDisable()
    {
        if (UpgradeEventManager.Instance != null)
        {
            UpgradeEventManager.Instance.OnUpgradeSelected -= HandleUpgradeSelected;
        }
    }

    //카드가 선택되었을 때 실행되는 친구
    private void HandleUpgradeSelected(UpgradeOption option)
    {
        if (option == null || option.Data == null)
        {
            return;
        }
        //스텟 카드인지 확인하기
        if (option.Data.UpgradeType != UpgradeType.Stat)
        {
            return;
        }

        //스텟카드면 어떤 스텟인지 구분
        switch (option.Data.StatType)
        {
            case StatType.Damage:
                damageBonus += option.Value;
                break;

            case StatType.AttackSpeed:
                attackSpeedBonus += option.Value;
                break;

            case StatType.Critical:
                criticalChanceBonus += option.Value;
                break;

            default:
                // MoveSpeed, MaxHp, Defense는 Player 담당 여기서 무시
                return;
        }
    }
}