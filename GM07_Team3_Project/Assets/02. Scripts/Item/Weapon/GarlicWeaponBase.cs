using UnityEngine;

public class GarlicWeaponBase : WeaponBase
{
    private UpgradeData garlicUpgradeData;
    private Transform garlicOwner;
    private float garlicValue;

    private GameObject auraObject;

    public override void Init(UpgradeOption option, Transform garlicOwner)
    {
        if (option == null || option.Data == null)
        {
            return;
        }

        this.garlicUpgradeData = option.Data;
        this.garlicOwner = garlicOwner;
        this.garlicValue = option.Value;

        CreatAura();
    }


    private void CreatAura()
    {
        if(auraObject != null)
        {
            return;
        }

        if(garlicUpgradeData.BulletPrefab ==null)
        {
            return;
        }

        //Player 자식으로 말고 월드로 생성해보기
        auraObject = Instantiate(
            garlicUpgradeData.BulletPrefab,
            garlicOwner.position,
            Quaternion.identity);

        GarlicAuraAttack auraAttack = auraObject.GetComponent<GarlicAuraAttack>();

        if(auraAttack == null)
        {
            return;
        }
        auraAttack.Init(garlicValue, garlicOwner);  


    }


   
}
