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
    public List<RectTransform> portraits = new List<RectTransform>(); // Order must match the character prefabs list in the character menu.
    [Tooltip("The first portrait selected.")]
    public RectTransform defaultPortrait = null;

    [Tooltip("Should it loop through the portraits instead of jumping back to the start?")]
    public bool loopEffect = true;
    [Tooltip("The clone portrait for the loop effect when going backwards. Should be placed before the start.")]
    public RectTransform loopStartPortrait = null;
    [Tooltip("The clone portrait for the loop effect when going forwards. Should be placed after the end.")]
    public RectTransform loopEndPortrait = null;

    [Tooltip("The curve that dictates the smoothing of the transition.")]
    public AnimationCurve transitionCurve;
    [Tooltip("How long it will take for the menus to fully transition.")]
    public float transitionDuration = 0.5f;

    private int _activePortrait = 0; // The currently selected portrait.
    private bool _inTransition = false; // Are we currently transitioning between portraits?

    private List<RectTransform> _allThePortraits = new List<RectTransform>(); // All of the portraits including the clone ones for the looping effect.

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
                parentSlot.SelectCharacter(_activePortrait);

            // Setup the extended portrait list for loop effect.
            if (loopEffect && loopStartPortrait != null)
                _allThePortraits.Add(loopStartPortrait);
            _allThePortraits.AddRange(portraits);
            if (loopEffect && loopEndPortrait != null)
                _allThePortraits.Add(loopEndPortrait);
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
        bool shouldLoop = loopEffect && ((_activePortrait + 1 >= portraits.Count) || (_activePortrait - 1 < 0)); // Should we loop around or not?
        int allPortraitIndex = _activePortrait;
        if (shouldLoop) allPortraitIndex = TrueModulo(forwards ? _activePortrait+2 : _activePortrait, _allThePortraits.Count); // Gets the index in the extended list.

        _activePortrait = TrueModulo(forwards ? ++_activePortrait : --_activePortrait, portraits.Count); // The index in the normal portraits list.

        // Get the position to the next portrait.
        Vector3 newPosition = _theActualTransform.anchoredPosition;
        Vector3 newLoopedPosition = _theActualTransform.anchoredPosition;
        newPosition.x = -portraits[_activePortrait].anchoredPosition.x;
        if (shouldLoop) newLoopedPosition.x = -_allThePortraits[allPortraitIndex].anchoredPosition.x;

        Debug.Log(_activePortrait + ", " + allPortraitIndex + ", " + newPosition.x + ", " + newLoopedPosition.x);

        // Transition to the next portrait.
        Tween.AnchoredPosition(_theActualTransform, shouldLoop ? newLoopedPosition : newPosition,
            transitionDuration, 0.0f, transitionCurve, Tween.LoopType.None,
            () => { _inTransition = true; }, // Start transition.
            () =>
            {
                if (shouldLoop) _theActualTransform.anchoredPosition = newPosition; // Snap to real portrait instead of the fake portrait.

                // Set selected character.
                _inTransition = false;
                parentSlot.SelectCharacter(_activePortrait);
                parentSlot._controllerEventSystem.SetSelectedGameObject(_playerSelect.gameObject);
            }, // Complete transition.
            false);
    }
}
