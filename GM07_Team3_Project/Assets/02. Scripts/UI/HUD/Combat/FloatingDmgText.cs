using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class FloatingDmgText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;

    [Header("텍스트 애니메이션 설정")]
    [SerializeField] private float lifetime = 1.0f;
    [SerializeField] private float moveY = 80f;
    [SerializeField] private float scalePunch = 0.2f;

    private RectTransform rectTransform;
    private Coroutine returnCoroutine;
    private Sequence sequence;
    private WaitForSeconds wfs;
    private Color defaultTextColor;

    public bool IsInPool { get; private set; }

    // pool 설정
    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultTextColor = damageText.color;
        wfs = new WaitForSeconds(lifetime);
    }

    public void MarkInPool()
    {
        IsInPool = true;
    }

    public void MarkOutOfPool()
    {
        IsInPool = false;
    }

    // 데미지 텍스트를 설정하고 표시
    public void ShowDamage(float damage, Vector3 position)
    {
        transform.position = position;
        damageText.text = Mathf.RoundToInt(damage).ToString();

        gameObject.SetActive(true);

        PlayAnim();

        returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay());

    }

    private void PlayAnim()
    {
        sequence = DOTween.Sequence();

        sequence.Join(rectTransform.DOAnchorPosY(
            rectTransform.anchoredPosition.y + moveY,
            lifetime).
            SetEase(Ease.OutCubic));

        sequence.Join(damageText.DOFade(0, lifetime).SetEase(Ease.InCubic));

        sequence.Join(rectTransform.DOPunchScale(
            Vector3.one * scalePunch,
            0.25f,
            1,
            0.5f));
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return wfs;
        returnCoroutine = null;
        FloatingDmgTextPoolManager.Instance.Return(this);
    }

    // 풀에서 반환될 때 상태를 초기화하는 메서드
    public void ResetState()
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
        if (sequence != null && sequence.IsActive())
        {
            sequence.Kill();
            sequence = null;
        }
        
        damageText.text = string.Empty;
        damageText.color = defaultTextColor;

        rectTransform.localScale = Vector3.one;
    }
}
