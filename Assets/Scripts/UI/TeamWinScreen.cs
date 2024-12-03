using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TeamWinScreen : Menu
{
    [Tooltip("the array of images where the winners will be displayed MUST BE IN ORDER FROM WINNER TO FOURTH PLACE"), SerializeField]
    private RawImage[] _winnerImages;

    [Tooltip("the render texture of the raccoon"), SerializeField]
    private RenderTexture _raccoonRenderTexture;

    [Tooltip("the render texture of the penguin"), SerializeField]
    private RenderTexture _penguinRenderTexture;

    [Tooltip("the render texture of the lizard"), SerializeField]
    private RenderTexture _lizardRenderTexture;

    [Tooltip("the render texture of the frog"), SerializeField]
    private RenderTexture _frogRenderTexture;

    [Tooltip("how long this scene will show up for"), SerializeField]
    private float _sceneTime;

    [Tooltip("the scene to be loaded after this scene is done"), SerializeField]
    private string _afterWinScene;

    [Tooltip("the slider component showing visualaly how far the button has been held")]
    [SerializeField]
    private Slider _buttonSlider;

    [Tooltip("how long the button has to be held before this scene is skipped")]
    [SerializeField]
    private float _buttonHoldTime;

    [Tooltip("how long the button has currently been held for")]
    private float _currentButtonHoldTime = 0;

    private bool _shouldReturn = false;

    [Header("Optional Stuff - ")]
    [Tooltip("Should we use the drawn portraits or the render textures?"), SerializeField]
    private bool _useDrawnPortraits = false;

    [Tooltip("The drawn portrait of the raccoon"), SerializeField]
    private Sprite _raccoonDrawnPortrait;

    [Tooltip("The drawn portrait of the penguin"), SerializeField]
    private Sprite _penguinDrawnPortrait;

    [Tooltip("The drawn portrait of the lizard"), SerializeField]
    private Sprite _lizardDrawnPortrait;

    [Tooltip("The drawn portrait of the frog"), SerializeField]
    private Sprite _frogDrawnPortrait;

    [Tooltip("How the drawn portraits should be scaled and positioned."), SerializeField]
    private Rect _drawnPortraitsRect = new Rect(0.25f, 0.45f, 0.44444444f, 0.33333333f);

    private void OnEnable()
    {
        TeamData[] teamWonData = GameManager.Instance.GetSortedTeamData();

        int count = 0;

        for(int i = 0; i < teamWonData.Length; i++)
        {
            if (_winnerImages[count] == null)
            {
                break;
            }
            for(int j = 0; j < teamWonData[i].playerData.Length; j++)
            {
                switch (teamWonData[i].playerData[j].characterSelect.name)
                {
                    case ("Raccoon"):
                        {
                            _winnerImages[count].texture = _useDrawnPortraits ? _raccoonDrawnPortrait.texture : _raccoonRenderTexture;
                            break;
                        }
                    case ("Penguin"):
                        {
                            _winnerImages[count].texture = _useDrawnPortraits ? _penguinDrawnPortrait.texture : _penguinRenderTexture;
                            break;
                        }
                    case ("Lizard"):
                        {
                            _winnerImages[count].texture = _useDrawnPortraits ? _lizardDrawnPortrait.texture : _lizardRenderTexture;
                            break;
                        }
                    case ("Frog"):
                        {
                            _winnerImages[count].texture = _useDrawnPortraits ? _frogDrawnPortrait.texture : _frogRenderTexture;
                            break;
                        }
                }
                if (_useDrawnPortraits)
                    _winnerImages[count].uvRect = _drawnPortraitsRect;
                count++;
                
            }
            
        }

        _buttonSlider.maxValue = _buttonHoldTime;
        _buttonSlider.minValue = 0;
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

        if (_menuManager) _menuManager._primaryController.currentActionMap.FindAction("Submit").started += ControlButtonHold;
        if (_menuManager) _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled += ControlButtonHold;

        StartCoroutine(LoadScene());
    }

    public void ControlButtonHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _shouldReturn = true;
            _buttonSlider.gameObject.SetActive(true);
        }
        if (context.canceled)
        {
            _buttonSlider.gameObject.SetActive(false);
            _shouldReturn = false;
            _currentButtonHoldTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_buttonSlider.gameObject.activeInHierarchy && _shouldReturn)
        {
            _currentButtonHoldTime += Time.unscaledDeltaTime;
            _buttonSlider.value = _currentButtonHoldTime;

            if (_currentButtonHoldTime >= _buttonHoldTime)
            {
                _menuManager.FadeToScene(_afterWinScene);
                _menuManager._primaryController.currentActionMap.FindAction("Submit").started -= ControlButtonHold;
                _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled -= ControlButtonHold;
                _shouldReturn = false;
            }
        }
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(_sceneTime);
        _menuManager.FadeToScene(_afterWinScene);

        _menuManager._primaryController.currentActionMap.FindAction("Submit").started -= ControlButtonHold;
        _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled -= ControlButtonHold;
    }
}
