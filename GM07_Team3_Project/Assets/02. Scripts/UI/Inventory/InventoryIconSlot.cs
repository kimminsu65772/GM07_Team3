using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventoryIconSlot : MonoBehaviour
{
    [Header("Slot UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    public void SetSlot(UpgradeData upgradeData, int count)
    {
        if (upgradeData == null || upgradeData.Icon == null)
        {
            ClearSlot();
            return;
        }

        if (iconImage != null)
        {
            iconImage.sprite = upgradeData.Icon;
            iconImage.enabled = true;
        }

        if (countText != null)
        {
            countText.text = Mathf.Max(1, count).ToString();
            countText.gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (countText != null)
        {
            countText.text = string.Empty;
            countText.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
