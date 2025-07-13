using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerHealth))]
public class HitEffects : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Image targetImage;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Punch Scale Settings")]
    [SerializeField] private float punchScale = 0.2f;
    [SerializeField] private float punchDuration = 0.25f;
    [SerializeField] private int punchVibrato = 10;
    [SerializeField] private float punchElasticity = 1f;

    [Header("Death Jump Settings")]
    [SerializeField] private float deathJumpHeight = 100f;
    [SerializeField] private float deathJumpDuration = 0.5f;
    [SerializeField] private Ease deathJumpEase = Ease.OutQuad;
    [SerializeField] private float deathFadeDuration = 0.3f;

    private PlayerHealth playerHealth;
    private SoundData soundData;

    private Color originalColor;
    private RectTransform rt;

    private void Awake()
    {
        if (targetImage == null)
        {
            targetImage = GetComponentInChildren<Image>();
        }
        originalColor = targetImage.color;
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerHealth.OnHealthChanged += HandleHit;
        playerHealth.OnPlayerDied += HandleDeath;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= HandleHit;
        playerHealth.OnPlayerDied -= HandleDeath;
    }

    private void Start()
    {
        soundData = DataManager.Instance.soundData;
    }

    private void HandleHit(int newHealth)
    {
        if (newHealth <= 0 || playerHealth.maxHealth == newHealth)
        {
            return;
        }
        SoundManager.Instance.PlaySFX(soundData.hit);
        targetImage.DOKill();
        targetImage.DOColor(flashColor, flashDuration * 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() => targetImage.color = originalColor);

        rt.DOKill(true);
        rt.DOPunchScale(Vector3.one * punchScale, punchDuration, punchVibrato, punchElasticity);

    }

    private void HandleDeath()
    {
        playerHealth.OnPlayerDied -= HandleDeath;
        targetImage?.DOKill(true);
        rt?.DOKill(true);
        var seq = DOTween.Sequence();

        if (rt != null)
        {
            Vector2 startPos = rt.anchoredPosition;
            Vector2 jumpTarget = startPos + Vector2.up * deathJumpHeight;
            //seq.Append(rt.DOAnchorPosY(jumpTarget.y, deathJumpDuration * 0.5f).SetEase(Ease.InBounce).SetLoops(1, LoopType.Yoyo)); 
            // better way
            seq.Append(rt.DOAnchorPos(jumpTarget, deathJumpDuration * 0.5f).SetEase(deathJumpEase));
            seq.Append(rt.DOAnchorPos(startPos, deathJumpDuration * 0.5f).SetEase(Ease.InQuad));
        }

        if (targetImage != null)
        {
            seq.Join(targetImage.DOFade(0f, deathFadeDuration));
        }

        seq.OnComplete(() =>
        {
            Destroy(gameObject);
            GameEvents.OnWavesEnd?.Invoke();
        });
    }

    private void OnDestroy()
    {
        rt.DOKill(true);
        targetImage?.DOKill(true);
    }
}
