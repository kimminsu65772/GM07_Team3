using DamageNumbersPro;
using UnityEngine;

public class DmgTextSpawner : MonoBehaviour
{
    [SerializeField] private DamageNumber damagePopupPrefab;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.3f, 0f);


    private void Awake()
    {
        damagePopupPrefab.enablePooling = true;
        damagePopupPrefab.PrewarmPool();
    }

    private void OnEnable()
    {
        Enemy.OnDamaged += SpawnDamageText;
    }

    private void OnDisable()
    {
        Enemy.OnDamaged -= SpawnDamageText;
    }

    public void SpawnDamageText(float damage, Vector3 position)
    {
        Debug.Log($"Spawning damage text: {damage} at position: {position}");
        DamageNumber popup = damagePopupPrefab.Spawn(position + offset, damage);
        popup.SetColor(Color.white);
    }
}

    //public void SpawnDamageText(float damage, Vector3 position)
    //{
    //    FloatingDmgText dmgText = FloatingDmgTextPoolManager.Instance.GetText();
    //    dmgText.ShowDamage(damage, position);
    //}
