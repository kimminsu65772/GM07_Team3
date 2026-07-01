using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectView : MonoBehaviour
{

    [Header("캐릭터 선택 슬롯")]
    [SerializeField] private CharacterSlot[] characterSlots;

    [Header("선택된 캐릭터 정보")]
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterIcon;
    [SerializeField] private TextMeshProUGUI characterDescription;

    [Header("선택된 캐릭터의 스탯 리스트")]
    [SerializeField] private StatTextRow[] statRows;

    [Header("시작 아이템")]
    [SerializeField] private GameObject startItemRoot;
    [SerializeField] private Image startItemIcon;
    [SerializeField] private TextMeshProUGUI startItemName;
    [SerializeField] private TextMeshProUGUI startItemDescription;

    [Header("버튼")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button backButton;

    // 캔버스를 초기화하는 메서드
    public void Init(
        IReadOnlyList<CharacterDataSO> characters,
        Action<CharacterDataSO> onCharacterSelected,
        Action onGameStartBtnClicked,
        Action onBackBtnClicked)
    {
        // 캐릭터 슬롯 초기화
        for (int i = 0; i < characterSlots.Length; i++)
        {
            // 혹시 캐릭터 슬롯이 실제 캐릭터 수보다 많을 경우, 초과된 슬롯은 비활성화
            if (i >= characters.Count)
            {
                characterSlots[i].gameObject.SetActive(false);
                continue;
            }

            characterSlots[i].gameObject.SetActive(true);
            characterSlots[i].Init(characters[i], onCharacterSelected);
        }


        // 중복 구독을 방지하기 위해 기존 리스너를 모두 제거하고 새 리스너를 추가한다.
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => onGameStartBtnClicked?.Invoke());

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => onBackBtnClicked?.Invoke());
    }

    // 선택된 캐릭터의 정보를 갱신하는 메서드
    public void Refresh(CharacterDataSO selectedCharacter)
    {
        if (selectedCharacter == null) return;

        RefreshCharacterInfo(selectedCharacter);
        RefreshCharacterStat(selectedCharacter.PlayerStatData);
        RefreshSlots(selectedCharacter);
        RefreshStartItem(selectedCharacter);
    }

    private void RefreshCharacterInfo(CharacterDataSO selectedCharacter)
    {
        characterName.text = selectedCharacter.CharacterName;
        characterIcon.sprite = selectedCharacter.Icon;
        characterDescription.text = selectedCharacter.Description;
    }

    private void RefreshCharacterStat(PlayerStatSO playerStatData)
    {
        if (playerStatData == null) return;

        foreach (StatEntry stat in playerStatData.BaseStats)
        {
            StatTextRow row = FindStatRow(stat.StatType);

            if (row != null)
            {
                row.SetValue(stat.Value.ToString("0.#"));
            }
        }
    }

    // 스탯 타입에 맞는 스탯 설명 행을 찾는 메서드
    private StatTextRow FindStatRow(StatType statType)
    {
        foreach (StatTextRow row in statRows)
        {
            if (row.StatType == statType)
            {
                return row;
            }
        }

        return null;
    }

    // 선택된 캐릭터 슬롯을 갱신하는 메서드
    private void RefreshSlots(CharacterDataSO selectedCharacter)
    {
        foreach (CharacterSlot slot in characterSlots)
        {
            slot.SetSelected(slot.CharacterData == selectedCharacter);
        }
    }

    private void RefreshStartItem(CharacterDataSO selectedCharacter)
    {
        if (selectedCharacter == null || selectedCharacter.StartItemData == null)
        {
            ClearStartItem();
            
            if (startItemRoot != null)
            {
                startItemRoot.SetActive(false);
            }

            return;
        }

        UpgradeData startItemData = selectedCharacter.StartItemData;

        if (startItemRoot != null)
        {
            startItemRoot.SetActive(true);
        }


        if (startItemIcon != null)
        {
            startItemIcon.sprite = startItemData.Icon;
            startItemIcon.enabled = startItemIcon.sprite != null;
        }

        if (startItemName != null)
        {
            startItemName.text = startItemData.UpgradeName;
        }

        if (startItemDescription != null)
        {
            startItemDescription.text = startItemData.Description;
        }
    }

    private void ClearStartItem()
    {
        if (startItemIcon != null)
        {
            startItemIcon.sprite = null;
            startItemIcon.enabled = false;
        }

        if (startItemName != null)
        {
            startItemName.text = string.Empty;
        }
        
        if (startItemDescription != null)
        {
            startItemDescription.text = string.Empty;
        }
    }


    // 스탯 텍스트 행을 나타내는 클래스
    // 오브젝트를 직렬화하여 에디터에서 설정하고 이를 통해 스탯 타입과 텍스트 UI를 연결하도록 함.
    [Serializable]
    public sealed class StatTextRow
    {
        [SerializeField] private StatType statType;
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI statValueText;

        public StatType StatType => statType;

        public void SetValue(string value)
        {
            if (statNameText != null)
            {
                statNameText.text = statType.ToString();
            }

            if (statValueText != null)
            {
                statValueText.text = value;
            }
        }
    }
}
