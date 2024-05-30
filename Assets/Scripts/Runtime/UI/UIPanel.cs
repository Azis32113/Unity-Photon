using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool IsShowing { get; private set; } = false;

    public void Init()
    {
        if (gameObject.activeInHierarchy) IsShowing = true;
        else IsShowing = false;
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void SetVisible(bool visible) 
    {
        if (IsShowing == visible) return;
        if (visible) Show();
        else Hide();
    }

    private void Show()
    {
        IsShowing = true;
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.2f);
    }

    private void Hide()
    {
        IsShowing = false;
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, 0.2f).OnComplete(() => gameObject.SetActive(false));
    }
}
