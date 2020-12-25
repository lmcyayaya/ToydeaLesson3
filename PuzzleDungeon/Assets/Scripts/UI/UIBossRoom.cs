using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBossRoom : MonoBehaviour
{
    public Image mask;
    public RectTransform askWindow;
    public void OpenWindow()
    {
        mask.raycastTarget = true;
        askWindow.DOScale(1,0.2f).SetEase(Ease.OutBack);
        mask.DOFade(0.7843f,0.2f);
    }
    public void CloseWindow()
    {
        mask.raycastTarget = false;
        askWindow.DOScale(0,0.2f);
        mask.DOFade(0,0.2f);
    }
}
