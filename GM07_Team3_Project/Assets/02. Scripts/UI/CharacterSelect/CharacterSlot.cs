using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [Header("슬롯 UI 세팅")]
    [SerializeField] private Button btn;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private GameObject selectedFrame;

    public CharacterDataSO CharacterData { get; private set; }

    public void Init(CharacterDataSO characterData, Action<CharacterDataSO> onClicked)
    {
        CharacterData = characterData;

        if (characterName != null)
            characterName.text = CharacterData.CharacterName;
        
        if (icon != null)
        {
            icon.sprite = CharacterData.Icon;
            icon.enabled = icon.sprite != null;
        }

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onClicked?.Invoke(CharacterData));

        SetSelected(false);
    }

    public void SetSelected(bool isSelected)
    {
        if (selectedFrame != null)
            selectedFrame.SetActive(isSelected);
    }
}
