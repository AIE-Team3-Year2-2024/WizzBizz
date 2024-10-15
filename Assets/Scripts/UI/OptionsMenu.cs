using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsMenu : Menu
{
    [Header("Options Menu Stuff - ")]
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] _systemResolutions;
    private Resolution _currentResolution;
    private bool _isExclusiveFullscreen = false;
    private bool _isBorderlessFullscreen = false;

    public override void Awake()
    {
        base.Awake();

    }

    public override void Start()
    {
        base.Start();

        if (resolutionDropdown == null)
        {
            Debug.LogError("References haven't been set.");
            return;
        }
        if (resolutionDropdown.options.Count > 0)
            return;

        _systemResolutions = Screen.resolutions;
        _currentResolution = Screen.currentResolution;
        Debug.Log("Yea " + _systemResolutions.Length);

        int resolutionSelection = 0;
        List<TMP_Dropdown.OptionData> resolutionOptions = new();
        foreach (Resolution r in _systemResolutions)
        {
            resolutionOptions.Add(new (r.ToString()));
            if (!r.Equals(_currentResolution))
                resolutionSelection++;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.SetValueWithoutNotify(resolutionSelection);
    }

    public void Update()
    {
        if (resolutionDropdown.IsExpanded)
            _menuManager._controllerCancelCallback += OnControllerCancel;
        else
            _menuManager._controllerCancelCallback -= OnControllerCancel;
    }

    public void OnResolutionDropdown()
    {
        if (_systemResolutions.Length <= 0)
        {
            Debug.LogError("What");
            return;
        }
        if (_systemResolutions[resolutionDropdown.value].Equals(_currentResolution))
            return;

        _currentResolution = _systemResolutions[resolutionDropdown.value];
        Screen.SetResolution(_currentResolution.width, _currentResolution.height, false, _currentResolution.refreshRate);
    }

    public void OnControllerCancel(PlayerInput controller)
    {
        // Override controller cancel.
        return;
    }

}
