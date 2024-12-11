using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using System.Collections.Generic;

public class CreditsMenu : Menu
{
    [Header("Credits Menu Stuff - ")]
    public RectTransform creditsScroll;

    [Tooltip("The time it takes to scroll the credits in seconds.")]
    public float scrollTime = 30.0f;

    [Tooltip("The time it takes for the credits scroll to start in seconds.")]
    public float scrollDelay = 1.0f;

    [Tooltip("The curve that dictates the smoothing of the transition.")]
    public AnimationCurve transitionCurve;

    private TweenBase _tween = null;
    private List<CanvasGroup> _creditsElements = new();

    public override void Start()
    {
        base.Start();
        
        CanvasGroup[] el = creditsScroll.gameObject.GetComponentsInChildren<CanvasGroup>();
        _creditsElements.AddRange(el);
    }

    void OnEnable()
    {
        if (!_alreadyInitialized || !_alreadyStarted)
            return;

        foreach (CanvasGroup c in _creditsElements)
        {
            c.alpha = 1.0f;
        }

        if (_tween != null)
            _tween.Cancel();

        float halfHeight = ((800.0f / (Screen.width / Screen.height)) / 2.0f) - (creditsScroll.rect.height / 2.0f);

        if (creditsScroll)
        {
            _tween = Tween.AnchoredPosition(creditsScroll,
                Vector2.up * -halfHeight, Vector2.up * creditsScroll.rect.height,
                scrollTime, scrollDelay,
                transitionCurve,
                Tween.LoopType.None,
                () => { /* Unused. */ },
                () => { /* Unused. */ },
                false);
        }
    }

    void Update()
    {
        if (_creditsElements.Count <= 0) return;

        float distToFade = 800.0f;
        foreach (CanvasGroup c in _creditsElements)
        {
            float dist = (c.transform.position.y - distToFade);
            c.alpha = 1.0f - Mathf.Clamp(dist / 100.0f, 0.0f, 1.0f);
            Debug.Log(c.gameObject.name + ": " + c.transform.position.y + ", " + dist + " : " + c.alpha);
        }
    }
}
