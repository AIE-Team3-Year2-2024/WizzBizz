using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MenuManager : MonoBehaviour
{
    static public MenuManager Instance { get; private set; } // Singleton.

    // TODO: Maybe get primary controller by first press?
    public PlayerInput primaryController = null; // The first controller that is connected.

    [Tooltip("Should be the scene the game starts on.")]
    [HideInInspector]
    public string rootSceneName = ""; // Makes sure that the scene transitions can't go back further than the first scene.
    
    [Tooltip("The curve that dictates the smoothing of the transition.")]
    public AnimationCurve transitionCurve;
    [Tooltip("How long it will take for the menus to fully transition.")]
    public float transitionDuration = 1.5f;

    [Tooltip("A reference to the canvas group that holds the fade effect.")]
    public CanvasGroup fadeCanvas;
    [Tooltip("How long it will take to fade.")]
    public float fadeDuration = 3.0f;

    [HideInInspector]
    public event Action<PlayerInput> _controllerDisconnectCallback = null;
    [HideInInspector]
    public event Action<PlayerInput> _controllerReconnectCallback = null;
    [HideInInspector]
    public event Action<PlayerInput> _controllerCancelCallback = null;

    private bool _goingBack = false; // Are we currently transitioning back a menu/scene?
    
    private Menu _activeMenu = null; // The current menu.
    private Menu _targetMenu = null; // Menu to transition to.
    private Menu _lastActiveMenu = null; // The last menu.

    // TODO: Make this a tree or stack so that menus can be nested properly.
    private string _lastActiveScene = string.Empty; // The last scene we were in.

    private List<Menu> _menus = new List<Menu>(); // List of menus in the current scene.

    private List<PlayerInput> _controllers = new List<PlayerInput>(); // List of active controllers.

    private MultiplayerEventSystem _primaryEventSystem = null; // Event system of the primary controller.
    private InputSystemUIInputModule _primaryInputModule = null; // Input module of the primary controller.

    void Awake()
    {
        // Singleton.
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rootSceneName = SceneManager.GetActiveScene().name;
        
        // Make sure we have the primary controller.
        if (primaryController != null)
        {
            _controllers.Add(primaryController);
            _primaryEventSystem = primaryController.GetComponentInChildren<MultiplayerEventSystem>();
            _primaryInputModule = primaryController.GetComponentInChildren<InputSystemUIInputModule>();
        }
        
        InitializeManager();

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0.0f; // Make sure that the fade isn't obscuring the screen by default.
        }
    }

    public void InitializeManager()
    {
        _controllerDisconnectCallback = null; // Reset callbacks on scene load (avoids missing references)
        _controllerReconnectCallback = null;
        _controllerCancelCallback = null;
        _primaryEventSystem.playerRoot = null; // Reset this too.

        // Destroy any new controllers, should only be setup on the character select. (Doesn't destroy the primary controller)
        if (_controllers.Count > 0)
        {
            for (int i = _controllers.Count-1; i >= 0; --i)
            {
                if (_controllers[i] == primaryController)
                    continue;
                PlayerInput p = _controllers[i];
                _controllers.Remove(p);
                Destroy(p.gameObject);
            }
        }

        PopulateMenus(); // Find all the menus in the scene.

        // Setup menus.
        Menu firstMenu = null; // The first menu that should be selected in the scene.
        foreach (Menu m in _menus)
        {
            if (m.gameObject.activeInHierarchy == false)
            {
                m.gameObject.SetActive(true);
                m.Awake(); // Make sure this is called so that the component gets its required references since awake isn't called when the game object itself is disabled.
            }

            m._menuManager = this;
            m._canvasGroup.interactable = false;
            m._canvasGroup.blocksRaycasts = true;

            m.Start();
            m.gameObject.SetActive(false);
        }

        SceneInfo info = FindObjectOfType<SceneInfo>();
        if (info) // Is there any scene info?
        {
            firstMenu = info.firstMenu; // Set first menu that should be active.
        }

        // Setup first menu.
        if (firstMenu && _menus.Contains(firstMenu))
        {
            _activeMenu = firstMenu;
            _activeMenu._canvasGroup.interactable = true;
            _activeMenu._canvasGroup.blocksRaycasts = false;
            _activeMenu.gameObject.SetActive(true);

            if (_activeMenu.firstSelected)
                _primaryEventSystem.SetSelectedGameObject(_activeMenu.firstSelected.gameObject);
        }
    }

    public void AddController(PlayerInput controller)
    {
        if (controller != null && _controllers.Contains(controller) == false)
        {
            _controllers.Add(controller);
        }
    }

    // Handle controller disconnect.
    public void ControllerDisconnect(PlayerInput controller)
    {
        if (_controllers.Contains(controller) == false)
            return;
        
        _controllers.Remove(controller);
        if (controller == primaryController)
        {
            primaryController = null;
            _primaryEventSystem = null;
            _primaryInputModule = null;
        }

        if (_controllerDisconnectCallback != null)
            _controllerDisconnectCallback.Invoke(controller);
    }

    // Handle controller regained.
    public void ControllerReconnect(PlayerInput controller)
    {
        if (_controllers.Contains(controller) == true)
            return;
        
        if (primaryController == null)
        {
            // Add a new primary controller.
            primaryController = controller;
            _primaryEventSystem = primaryController.GetComponentInChildren<MultiplayerEventSystem>();
            _primaryInputModule = primaryController.GetComponentInChildren<InputSystemUIInputModule>();
        }
        AddController(controller);

        if (_controllerReconnectCallback != null)
            _controllerReconnectCallback.Invoke(controller);
    }

    // Handle controller cancel button.
    public void ControllerCancel(PlayerInput controller)
    {
        if (controller != primaryController) // Probably shouldn't be done by other controllers.
            return;

        if (_controllerCancelCallback != null)
            _controllerCancelCallback.Invoke(controller); // Do this if there's a callback setup.
        else // Otherwise go back instead.
        {
            GoBackMenu(); // Try this. Will do nothing if there's no last menu.
            GoBackScene(); // Also, try this.
        }
    }

    // Transition to another menu.
    public void SetTargetMenu(Menu menuObj)
    {
        if (menuObj == null || menuObj == _activeMenu)
            return;

        _targetMenu = menuObj;
        _lastActiveMenu = _activeMenu;

        _activeMenu._lastSelected = _primaryEventSystem.currentSelectedGameObject.GetComponent<Selectable>();

        switch (_activeMenu.transitionDirection)
        {
            case Menu.MenuTransitionDirection.MENU_MOVE_LEFT:
            case Menu.MenuTransitionDirection.MENU_MOVE_RIGHT:
            {
                int direction = _activeMenu.transitionDirection == Menu.MenuTransitionDirection.MENU_MOVE_LEFT ? -1 : 1;
                TransitionHorizontalToTarget(_activeMenu, _targetMenu, direction);
            } break;
            // TODO: Vertical transitions.
        }
    }

    public void GoBackMenu()
    {
        if (_lastActiveMenu == null || _goingBack == true) 
            return;
        _goingBack = true;
        SetTargetMenu(_lastActiveMenu);
    }

    public void GoBackScene()
    {
        if (_lastActiveScene == string.Empty || SceneManager.GetActiveScene().name == rootSceneName || _goingBack == true)
            return;
        _goingBack = true;
        FadeToScene(_lastActiveScene);
    }

    // Go to another scene. (no transition)
    public void SwitchToScene(string sceneName)
    {
        if (sceneName.Length <= 0)
            return;

        // Disable/reset stuff.
        if (_activeMenu != null)
        {
            _activeMenu._canvasGroup.interactable = false;
            _activeMenu._canvasGroup.blocksRaycasts = true;
            _activeMenu.gameObject.SetActive(false);
            _activeMenu = null;
        }
        _lastActiveMenu = null;
        _targetMenu = null;

        _lastActiveScene = SceneManager.GetActiveScene().name;

        _goingBack = false;

        StartCoroutine(LoadSpecifiedScene(sceneName)); // Load new scene.
    }

    // Go to another scene. (Fade in/out)
    public void FadeToScene(string sceneName)
    {
        if (fadeCanvas == null || sceneName.Length <= 0)
            return;

        // Disable menu interaction.
        if (_activeMenu != null)
        {
            _activeMenu._canvasGroup.interactable = false;
            _activeMenu._canvasGroup.blocksRaycasts = true;
        }

        // Fade out.
        Tween.CanvasGroupAlpha(fadeCanvas, 0.0f, 1.0f,
            fadeDuration, 0.0f, transitionCurve, Tween.LoopType.None, 
            () => { /* Unused. */ }, // Start callback.
            () => {
                // Reset stuff.
                if (_activeMenu != null)
                {
                    _activeMenu.gameObject.SetActive(false);
                    _activeMenu = null;
                }
                _lastActiveMenu = null;
                _targetMenu = null;

                _lastActiveScene = SceneManager.GetActiveScene().name;
                
                _goingBack = false;

                StartCoroutine(LoadSpecifiedScene(sceneName, true)); // Load new scene.
            }, // Complete callback. 
            false);
    }

    private void TransitionHorizontalToTarget(Menu currentMenu, Menu nextMenu, int direction)
    {
        if (currentMenu == null || nextMenu == null)
            return;

        // Disable interaction with current menu.
        currentMenu._canvasGroup.interactable = false;
        currentMenu._canvasGroup.blocksRaycasts = true;
        
        // Current menu moves out of screen.
        float tweenDuration = transitionDuration * 0.5f; // Since it'll do two tweens for each menu technically.
        Tween.LocalPosition(currentMenu.anchorObject, Vector3.zero, new Vector3(currentMenu._canvasReferenceResolution.x * direction, 0, 0),
            tweenDuration, 0.0f, transitionCurve, Tween.LoopType.None, 
            () => { /* Unused. */ }, // Start callback.
            () => { 
                // Switch visible menus.
                currentMenu.gameObject.SetActive(false);
                nextMenu.gameObject.SetActive(true);

                // Next menu moves into screen.
                Tween.LocalPosition(nextMenu.anchorObject, new Vector3(nextMenu._canvasReferenceResolution.x * direction, 0, 0), Vector3.zero,
                    tweenDuration, 0.0f, transitionCurve, Tween.LoopType.None, 
                    () => { /* Unused. */ }, // Start callback.
                    () => {
                        // Setup next menu as current menu.
                        _activeMenu = _targetMenu;
                        _targetMenu = null;

                        // Enable interaction.
                        nextMenu._canvasGroup.interactable = true;
                        nextMenu._canvasGroup.blocksRaycasts = false;

                        // Controller select new menu.
                        Selectable selectedObject = nextMenu._lastSelected == null ? nextMenu.firstSelected : nextMenu._lastSelected;
                        _primaryEventSystem.SetSelectedGameObject(selectedObject.gameObject);

                        _goingBack = false;
                    }, // Complete callback. 
                    false);
            }, // Complete callback.
            false);
    }

    private void PopulateMenus()
    {
        // Find all menus in the scene.
        _menus.Clear();
        Menu[] arr = FindObjectsByType<Menu>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _menus.AddRange(arr);
    }

    IEnumerator LoadSpecifiedScene(string sceneName, bool fade = false)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone) // Wait until loaded.
        {
            yield return null;
        }

        Debug.Log("Scene Loaded: " + sceneName);

        InitializeManager(); // Reset menu manager.

        if (fade) // Fade back in.
        {
            if (fadeCanvas.alpha >= 1.0f)
            {
                Tween.CanvasGroupAlpha(fadeCanvas, 1.0f, 0.0f,
                    fadeDuration, 0.0f, transitionCurve, Tween.LoopType.None,
                    () => { /* Unused. */ }, // Start callback.
                    () => { /* Unused. */ }, // Complete callback.
                    false);
            }
        }
    }
}
