using UnityEngine;

public class GarlicWeaponBase : WeaponBase
{
    private UpgradeData upgradeData;
    private Transform owner;
    private float value;

    private GameObject auraObject;

    public override void Init(UpgradeOption option, Transform owner)
    {
        if (option == null || option.Data == null)
        {
            return;
        }

        this.upgradeData = option.Data;
        this.owner = owner;
        this.value = option.Value;

        CreatAura();
    }


    private void CreatAura()
    {
        if(auraObject != null)
        {
            return;
        }

        if(upgradeData.BulletPrefab ==null)
        {
            return;
        }

        //Player 자식으로 말고 월드로 생성해보기
        auraObject = Instantiate(
            upgradeData.BulletPrefab,
            owner.position,
            Quaternion.identity);

        GarlicAuraAttack auraAttack = auraObject.GetComponent<GarlicAuraAttack>();

        if(auraAttack == null)
        {
            return;
        }
        auraAttack.Init(value, owner);  


    }


   
}
