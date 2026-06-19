using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Pause Menu Button Type Settings")]
    [SerializeField] private PauseMenuType buttonType;

    [Header("Animation Scale Settings")]
    [SerializeField] private float hoverScale = 1.1f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    [SerializeField] private bool isClickable;

    public event Action<PauseMenuType> OnPauseMenuClicked;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.transform.localScale;
        isClickable = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClickable) return;
        rectTransform.DOKill();
        transform.DOKill();

        transform.DOScale(originalScale * hoverScale, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClickable) return;
        rectTransform.DOKill();
        transform.DOKill();
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void OnClickButton()
    {
        if (!isClickable) return;
        Debug.Log($"Button clicked: {buttonType}");
        OnPauseMenuClicked?.Invoke(buttonType);
    }
}
