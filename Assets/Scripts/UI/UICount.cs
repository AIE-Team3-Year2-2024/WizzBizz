using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

[RequireComponent(typeof(CanvasGroup))]
public class UICount : MonoBehaviour
{
    private CanvasGroup _canvasGroup = null;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        Tween.LocalScale(transform, Vector3.one * 2.0f, Vector3.one, 1.0f, 0.0f, Tween.EaseInOut, Tween.LoopType.None, 
            () => { /* Unused. */ }, 
            () => { /* Unused. */ }, 
            false);
        Tween.CanvasGroupAlpha(_canvasGroup, 1.0f, 0.0f, 1.0f, 0.0f, Tween.EaseInOut, Tween.LoopType.None, 
            () => { /* Unused. */ }, 
            () => { gameObject.SetActive(false); }, 
            false);
    }
}
