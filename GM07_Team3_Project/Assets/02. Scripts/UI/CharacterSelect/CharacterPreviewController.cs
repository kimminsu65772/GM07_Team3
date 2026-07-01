using UnityEngine;

public class CharacterPreviewController : MonoBehaviour
{
    [Header("캐릭터 프리뷰")]
    [SerializeField] private Transform previewRoot;
    [SerializeField] private float rotationSpeed = 30f;

    private GameObject currentModel;

    private void Update()
    {
        if (currentModel != null)
        {
            RotateModel();
        }
    }

    public void ShowCharacter(CharacterDataSO characterData)
    {
        ClearCurrentModel();

        if (characterData == null || characterData.ModelPrefab == null) return;

        if (previewRoot == null) return;

        currentModel = Instantiate(characterData.ModelPrefab, previewRoot);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;
        currentModel.transform.localScale = Vector3.one;
    }

    private void RotateModel()
    {
        if (currentModel == null) return;

        currentModel.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void ClearCurrentModel()
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
            currentModel = null;
        }
    }
}
