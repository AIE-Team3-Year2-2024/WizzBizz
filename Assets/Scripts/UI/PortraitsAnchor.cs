using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Handle the transitions for the player selection.
public class PortraitsAnchor : MonoBehaviour
{
    [Tooltip("The player slot this is apart of.")]
    public PlayerSlot parentSlot = null;

    [Tooltip("List of the potraits to scroll through.")]
    public List<RectTransform> portraits = new List<RectTransform>();
    [Tooltip("The first portrait selected.")]
    public RectTransform defaultPortrait = null;

    [Tooltip("The curve that dictates the smoothing of the transition.")]
    public AnimationCurve transitionCurve;
    [Tooltip("How long it will take for the menus to fully transition.")]
    public float transitionDuration = 0.5f;

    private int _activePortrait = 0; // The currently selected portrait.
    private bool _inTransition = false; // Are we currently transitioning between portraits?

    private RectTransform _theActualTransform; // lol, Unity UI.

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
                if (portraits[i] == defaultPortrait) // Get index of the default portrait.
                {
                    _activePortrait = i;
                    break;
                }
            }

            // Setup defaults.
            Vector3 newPosition = _theActualTransform.anchoredPosition;
            newPosition.x = -defaultPortrait.anchoredPosition.x;
            _theActualTransform.anchoredPosition = newPosition;

            if (parentSlot != null)
                parentSlot._selectedCharacterIndex = _activePortrait;
        }
    }

    public int TrueModulo(int left, int right) // Probably should be in a static utility class.
    {
        return ((left % right) + right) % right; // C# % operator is actually the remainder meaning that it doesn't actually wrap negatives like a modulo would.
    }

    // Called by Unity UI interactables.
    public void SelectPortrait(bool forwards)
    {
        if (_inTransition == true) // Shouldn't transition if it already is.
            return;

        // Scroll through.
        _activePortrait = TrueModulo(forwards ? ++_activePortrait : --_activePortrait, portraits.Count);

        // Get the position to the next portrait.
        Vector3 newPosition = _theActualTransform.anchoredPosition;
        newPosition.x = -portraits[_activePortrait].anchoredPosition.x;

        Debug.Log(_activePortrait + "," + newPosition);

        // Transition to the next portrait.
        Tween.AnchoredPosition(_theActualTransform, newPosition, 
            transitionDuration, 0.0f, transitionCurve, Tween.LoopType.None,
            () => { _inTransition = true; }, // Start transition.
            () => { 
                // Set selected character.
                _inTransition = false;
                parentSlot._selectedCharacterIndex = _activePortrait;
                parentSlot._controllerEventSystem.SetSelectedGameObject(_playerSelect.gameObject);
            }, // Complete transition.
            false);
    }
}
