using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour // Base menu.
{
    [Tooltip("This is the transform that'll move the whole menu for transitions. (Default position of this object should be set to zero)")]
    public Transform anchorObject; // Can't move canvas by empty parent, so need a UI empty to move the contents instead.

    [Tooltip("Which way the menu will transition towards when it is being swapped out.")]
    public MenuTransitionDirection transitionDirection = MenuTransitionDirection.MENU_MOVE_LEFT;

    [Header("Optional Stuff - ")]
    [Tooltip("A list of all the buttons in the menu. (Optional)")]
    public List<Button> menuButtons = new List<Button>();
    [Tooltip("What selectable should the controller land on by default. (Optional)")]
    public Selectable firstSelected = null;

    [HideInInspector] public Selectable _lastSelected; // The last selectable that the controller was on.

    [HideInInspector] public Vector2 _canvasReferenceResolution = Vector2.zero; // The canvas resolution.
    [HideInInspector] public CanvasGroup _canvasGroup = null;
    [HideInInspector] public MenuManager _menuManager = null; // MenuManager is a singleton but this is probably safer idk.

    [HideInInspector] public bool _alreadyInitialized = false;
    [HideInInspector] public bool _alreadyStarted = false;
    [HideInInspector] public bool _alreadyLoaded = false;

    public enum MenuTransitionDirection
    {
        MENU_MOVE_LEFT,
        MENU_MOVE_RIGHT
    }

    public virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        CanvasScaler cs = GetComponentInChildren<CanvasScaler>();
        if (cs)
            _canvasReferenceResolution = cs.referenceResolution;

        _lastSelected = firstSelected;

        _alreadyInitialized = true;
    }

    public virtual void Start()
    {
        /* Implemented in inherited classes. */
        _alreadyStarted = true;
    }

    public virtual void OnLoaded()
    {
        /* Implemented in inherited classes. */
        _alreadyLoaded = true;
    }

    // Wrapper functions so we don't have to rely on a MenuManager being in the scene.
    public void GoToMenu(Menu menuObj)
    {
        if (_menuManager == null)
        {
            Debug.LogError("Menu Manager is null on Menu: " + gameObject.name);
            return;
        }
        _menuManager.SetTargetMenu(menuObj);
    }
    
    public void GoBackScene()
    {
        if (_menuManager == null)
        {
            Debug.LogError("Menu Manager is null on Menu: " + gameObject.name);
            return;
        }
        _menuManager.GoBackScene();
    }

    public void GoBackMenu()
    {
        if (_menuManager == null)
        {
            Debug.LogError("Menu Manager is null on Menu: " + gameObject.name);
            return;
        }
        _menuManager.GoBackMenu();
    }

}
