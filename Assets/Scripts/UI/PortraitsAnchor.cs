using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PortraitsAnchor : MonoBehaviour
{
    public List<RectTransform> portraits = new List<RectTransform>();
    public RectTransform defaultPortrait = null;

    public AnimationCurve transitionCurve;
    public float transitionDuration = 0.5f;

    private int _activePortrait = 0;
    private bool _inTransition = false;

    private RectTransform _theActualTransform;

    [HideInInspector] public CanvasGroup _playerSelect;

    void Awake()
    {
        _theActualTransform = (RectTransform)transform;
    }

    void Start()
    {
        if (portraits != null || portraits.Count > 0)
        {
            if (portraits.Contains(defaultPortrait) == false)
            {
                Debug.LogError("Specified default portrait is not in the list of portraits! + (" + gameObject.name + ")");
                return;
            }

            for (int i = 0; i < portraits.Count; i++)
            {
                if (portraits[i] == defaultPortrait)
                {
                    _activePortrait = i;
                    break;
                }
            }

            Vector3 newPosition = _theActualTransform.anchoredPosition;
            newPosition.x = -defaultPortrait.anchoredPosition.x;
            _theActualTransform.anchoredPosition = newPosition;
        }
    }

    public int TrueModulo(int left, int right) // Probably should be in a static utility class.
    {
        return ((left % right) + right) % right; // C# % operator is actually the remainder meaning that it doesn't actually wrap negatives like a modulo would.
    }

    public void SelectPortrait(bool forwards)
    {
        if (_inTransition == true)
            return;

        _activePortrait = TrueModulo(forwards ? ++_activePortrait : --_activePortrait, portraits.Count);

        Vector3 newPosition = _theActualTransform.anchoredPosition;
        newPosition.x = -portraits[_activePortrait].anchoredPosition.x;

        Debug.Log(_activePortrait + "," + newPosition);

        Tween.AnchoredPosition(_theActualTransform, newPosition, 
            transitionDuration, 0.0f, transitionCurve, Tween.LoopType.None,
            () => { _inTransition = true; },
            () => { 
                _inTransition = false;
                EventSystem.current.SetSelectedGameObject(_playerSelect.gameObject);
            },
            false);
    }
}
