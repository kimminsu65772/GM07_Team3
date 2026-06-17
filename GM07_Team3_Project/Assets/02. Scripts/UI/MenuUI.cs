using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

/*
 * MenuUI
 * MenuUI는 메뉴 버튼 UI에 애니메이션 효과를 적용하는 스크립트입니다.
 * 마우스와의 상호작용과 버튼 클릭 시 애니메이션 효과를 어떤 방식으로 적용할 것인지 정의합니다.
 */
public class MenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 애니메이션 타겟이 되는 버튼의 텍스트
    [SerializeField] private TextMeshProUGUI buttonText; 

    // 버튼에 마우스가 올라갔을 때의 스케일을 정의하는 변수
    [Header("Hover Settings")]
    [SerializeField] private float hoverScale = 1.2f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private bool isClickable;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.transform.localScale;
        isClickable = true;
    }

    // 버튼 위에 마우스가 올라갔을 때 호출되는 메서드
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClickable) return;
        
        // 현재 진행중인 애니메이션이 있다면 중지 
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClickable) return;
        transform.DOKill();
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack);
    }
}
