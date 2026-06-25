using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player HUD 세팅")]
    [SerializeField] private Slider expBar;
    [SerializeField] private Slider hpBar;
    [SerializeField] PlayerStatController playerStatController;

    private float currentExp = 0;
    private float requiredExp;

    private float maxHp;
    private float currentHp;



    private void Start()
    {
        maxHp = playerStatController.GetStat(StatType.MaxHp);
        currentHp = maxHp;
        //requiredExp = playerStatController.
    }

    private void HandleExpBar()
    {

    }

    private void HandleHpBar()
    {
        currentHp = playerStatController.CurrentHealth;
        hpBar.value = currentHp / maxHp;
    }
}
