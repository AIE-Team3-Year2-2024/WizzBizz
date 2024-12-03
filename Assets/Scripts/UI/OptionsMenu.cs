using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsMenu : Menu
{
    [Header("Options Menu Stuff - ")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown windowModeDropdown;
    public Toggle vsyncToggle;
    public Slider masterAudioSlider;
    public Slider musicAudioSlider;
    public Slider sfxAudioSlider;

    public AudioMixerGroup masterAudioGroup;
    public AudioMixerGroup musicAudioGroup;
    public AudioMixerGroup sfxAudioGroup;

    private Resolution[] _systemResolutions;
    private Resolution _currentResolution;
    private bool _isExclusiveFullscreen = false;
    private bool _isBorderlessFullscreen = false;
    private bool _isVSync = false;

    public override void Awake()
    {
        base.Awake();

    }

    public override void Start()
    {
        base.Start();

        if (resolutionDropdown == null || windowModeDropdown == null)
        {
            Debug.LogError("References haven't been set.");
            return;
        }

        // TODO: Read settings from file, and save to file.

        _isBorderlessFullscreen = (Screen.fullScreenMode == FullScreenMode.FullScreenWindow);
        _isExclusiveFullscreen = (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen);
        _isVSync = (QualitySettings.vSyncCount > 0);

        vsyncToggle.isOn = _isVSync;

        int defaultWindowModeValue = Convert.ToInt32(_isBorderlessFullscreen) + (Convert.ToInt32(_isExclusiveFullscreen) * 2);
        windowModeDropdown.SetValueWithoutNotify(defaultWindowModeValue);
        
        if (resolutionDropdown.options.Count > 0)
            return;
        
        _systemResolutions = Screen.resolutions;
        _currentResolution = Screen.currentResolution;

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

        masterAudioSlider.maxValue = 1.0f;
        masterAudioSlider.minValue = 0.0001f;
        musicAudioSlider.maxValue = 1.0f;
        musicAudioSlider.minValue = 0.0001f;
        sfxAudioSlider.maxValue = 1.0f;
        sfxAudioSlider.minValue = 0.0001f;

        masterAudioSlider.SetValueWithoutNotify(masterAudioSlider.maxValue);
        musicAudioSlider.SetValueWithoutNotify(masterAudioSlider.maxValue);
        sfxAudioSlider.SetValueWithoutNotify(masterAudioSlider.maxValue);
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
        Screen.SetResolution(_currentResolution.width, _currentResolution.height, Screen.fullScreenMode, _currentResolution.refreshRate);
    }

    public void OnWindowModeDropdown()
    {
        switch (windowModeDropdown.value)
        {
            case 0: // Windowed
            {
                if (_isBorderlessFullscreen == false && _isExclusiveFullscreen == false)
                    break;
                _isBorderlessFullscreen = false;
                _isExclusiveFullscreen = false;

                Screen.fullScreenMode = FullScreenMode.Windowed;
            } break;
            case 1: // Borderless Fullscreen
            {
                if (_isBorderlessFullscreen == true)
                    break;
                _isBorderlessFullscreen = true;
                _isExclusiveFullscreen = false;
                
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            } break;
            case 2: // Exclusive Fullscreen
            {
                if (_isExclusiveFullscreen == true)
                    break;
                _isBorderlessFullscreen = false;
                _isExclusiveFullscreen = true;
                
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            } break;
            default:
            {
              Debug.LogError("Invalid window mode! (" + windowModeDropdown.value + ")");      
            } break;
        }
    }

    public void OnVSyncToggle()
    {
        if (vsyncToggle.isOn == _isVSync)
            return;

        _isVSync = vsyncToggle.isOn;
        if (_isVSync) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
    }

    public void OnControllerCancel(PlayerInput controller)
    {
        // Override controller cancel.
        return;
    }

    public void OnMasterAudioChange()
    {
        // Power/dB = Log10(x), Power = Voltage^2
        // Voltage/dB = Log20(x) = Log10(x)*20
        // Mixer Volume (dB) = Signal Voltage
        if (masterAudioGroup)
            masterAudioGroup.audioMixer.SetFloat("masterVolume", Mathf.Log10(masterAudioSlider.value) * 20.0f);
    }

    public void OnMusicAudioChange()
    {
        if (musicAudioGroup)
            musicAudioGroup.audioMixer.SetFloat("musicVolume", Mathf.Log10(musicAudioSlider.value) * 20.0f);
    }

    public void OnSFXAudioChange()
    {
        if (sfxAudioGroup)
            sfxAudioGroup.audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxAudioSlider.value) * 20.0f);
    }

}
