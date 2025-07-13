using DG.Tweening;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private int maxPage;
    int currentPage = 1;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] Ease tweenEase = Ease.InOutSine;

    [SerializeField] float tweenTime = 0.5f;

    private void Awake()
    {
        targetPos = levelPagesRect.localPosition;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }
    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPagesRect.DOLocalMoveX(targetPos.x, tweenTime).SetEase(tweenEase);
    }
}
