using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    [Header("캐릭터 데이터 리스트")]
    [SerializeField] private CharacterListSO characterList;

    [Header("캐릭터 선택 캔버스")]
    [SerializeField] private CharacterSelectView view;

    [Header("캐릭터 프리뷰")]
    [SerializeField] private CharacterPreviewController characterPreview;

    private CharacterDataSO selectedCharacter;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (characterList == null || characterList.Characters.Count == 0) return;

        if (view == null) return;

        // 캔버스 초기화
        view.Init(characterList.Characters, SelectCharacter, StartGame, BackToMainMenu);

        SelectCharacter(characterList.Characters[0]);
    }

    private void SelectCharacter(CharacterDataSO characterData)
    {
        if (characterData == null) return;

        selectedCharacter = characterData;

        // 캐릭터 선택 컴포넌트에서 캐릭터 선택 메서드 실행
        CharacterSelection.SelectCharacter(selectedCharacter);

        // 캔버스 업데이트
        view.Refresh(selectedCharacter);

        if (characterPreview != null)
        {
            characterPreview.ShowCharacter(selectedCharacter);
        }
    }

    private void StartGame()
    {
        if (selectedCharacter == null) return;

        CharacterSelection.SelectCharacter(selectedCharacter);
        // UIManager에게 게임 씬으로 넘어갈 것을 요청 전달
        UIManager.Instance.HandleCharacterSelectMenuRequest(CharacterSelectMenuType.Select);
    }

    private void BackToMainMenu()
    {

        CharacterSelection.Clear();
        // UIManager에게 메인 메뉴 씬으로 넘어갈 것을 요청 전달
        UIManager.Instance.HandleCharacterSelectMenuRequest(CharacterSelectMenuType.Back);
    }

}
