using UnityEngine;
[DefaultExecutionOrder(-100)]
public sealed class PlayerCharacterLoader : MonoBehaviour
{
    [Header("캐릭터 모델 생성 위치")]
    [SerializeField] private Transform modelRoot;

    [Header("선택 없이 게임 씬을 실행할 때 사용할 캐릭터")]
    [SerializeField] private CharacterDataSO defaultCharacter;

    private PlayerAnimationController animationController;
    private PlayerStatController statController;

    public CharacterDataSO CurrentCharacter { get; private set; }

    public GameObject CurrentModel { get; private set; }

    private void Awake()
    {
        animationController = GetComponent<PlayerAnimationController>();

        statController = GetComponent<PlayerStatController>();

        if (animationController == null)
        {
            Debug.LogError($"{name}: PlayerAnimationController가 없습니다.", this);

            return;
        }

        if (statController == null)
        {
            Debug.LogError($"{name}: PlayerStatController가 없습니다.", this);

            return;
        }
        LoadCharacter();
    }

    private void LoadCharacter()
    {
        CurrentCharacter = CharacterSelection.SelectedCharacter;

        if (CurrentCharacter == null)
        {
            CurrentCharacter = defaultCharacter;
        }

        if (CurrentCharacter == null)
        {
            Debug.LogError($"{name}: 선택된 캐릭터가 없고 기본 캐릭터도 없습니다.", this);

            enabled = false;
            return;
        }

        if (CurrentCharacter.ModelPrefab == null)
        {
            Debug.LogError($"{CurrentCharacter.name}: 모델 프리팹이 없습니다.", CurrentCharacter);

            enabled = false;
            return;
        }

        if (CurrentCharacter.PlayerStatData == null)
        {
            Debug.LogError($"{CurrentCharacter.name}: PlayerStatSO가 없습니다.", CurrentCharacter);

            enabled = false;
            return;
        }

        if (modelRoot == null)
        {
            Debug.LogError($"{name}: Model Root가 연결되지 않았습니다.", this);

            enabled = false;
            return;
        }

        ClearCurrentModel();

        CurrentModel = Instantiate(CurrentCharacter.ModelPrefab, modelRoot);

        CurrentModel.transform.localPosition = Vector3.zero;

        CurrentModel.transform.localRotation = Quaternion.identity;

        Animator newAnimator = CurrentModel.GetComponentInChildren<Animator>(true);

        if (newAnimator == null)
        {
            Debug.LogError($"{CurrentModel.name}: Animator가 없습니다.", CurrentModel);
        }
        else
        {
            animationController.SetAnimator(newAnimator);
        }

        statController.Initialize(CurrentCharacter.PlayerStatData);
    }

    private void ClearCurrentModel()
    {
        for (int i = modelRoot.childCount - 1; i >= 0; i--)
        {
            GameObject child = modelRoot.GetChild(i).gameObject;

            child.SetActive(false);

            Destroy(child);
        }

        CurrentModel = null;
    }
}