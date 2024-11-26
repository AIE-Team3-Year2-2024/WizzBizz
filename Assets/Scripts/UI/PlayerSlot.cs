using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    [Tooltip("Reference to the UI element telling the player to join.")]
    public CanvasGroup joinText;
    [Tooltip("Reference to the UI element containing the player select.")]
    public CanvasGroup playerSelect;
    [Tooltip("Reference to the UI element containing the ready overlay.")]
    public CanvasGroup readyOverlay;
    [Tooltip("Reference to the UI element containing the selection arrows.")]
    public CanvasGroup selectArrows;
    [Tooltip("Reference to the UI element containing player options.")]
    public CanvasGroup descriptionSection;

    [Tooltip("Reference to a sprite asset which will be the background image for this specific player slot.")]
    public Sprite backgroundImage;
    [Tooltip("What color coding is attributed to this player slot?")]
    public PlayerData.ColorCode colorCode;

    [HideInInspector] public CharacterMenu _characterMenu = null;

    [HideInInspector] public int _playerID = -1; // The player occupying the slot.
    [HideInInspector] public bool _playerJoined = false; // Slot is occupied.
    [HideInInspector] public bool _playerReady = false; // Player is ready.

    [HideInInspector] public int _selectedCharacterIndex = -1; // The index of the character that is currently selected.

    [HideInInspector]
    public PlayerInput _controller = null; // The player's controller.
    [HideInInspector]
    public MultiplayerEventSystem _controllerEventSystem = null;

    public void OnEnable()
    {
        Start(); // Make sure start is called. Possible that it isn't called if the slot game object is inactive on scene load.
    }

    public void Start()
    {
        if (!joinText || !playerSelect || !selectArrows || !descriptionSection)
        {
            Debug.LogError("Player slot has not been setup! (" + gameObject.name + ")");
            return;
        }

        // Setup defaults.
        _playerJoined = false;
        joinText.alpha = 1.0f;
        joinText.interactable = true;
        playerSelect.alpha = 0.0f;
        playerSelect.interactable = false;
        readyOverlay.alpha = 0.0f;
        readyOverlay.interactable = false;
        selectArrows.alpha = 0.0f;
        selectArrows.interactable = false;
        descriptionSection.alpha = 0.0f;
        descriptionSection.interactable = false;

        playerSelect.GetComponentInChildren<PortraitsAnchor>()._playerSelect = playerSelect;
    }

    public void JoinPlayer(PlayerInput cc, MultiplayerEventSystem mm)
    {
        if (_playerJoined == true) // Don't repeat stuff if player has already joined.
            return;

        _controller = cc;
        _controllerEventSystem = mm;

        // Setup player select UI.
        joinText.alpha = 0.0f;
        joinText.interactable = false;
        playerSelect.alpha = 1.0f;
        playerSelect.interactable = true;
        selectArrows.alpha = 1.0f;
        selectArrows.interactable = true;
        descriptionSection.alpha = 1.0f;

        // Set controller selection to the player select UI.
        mm.SetSelectedGameObject(playerSelect.gameObject);

        _playerJoined = true;
    }

    public void LeavePlayer()
    {
        // Reset slot.
        _playerJoined = false;
        _playerReady = false;
        _controllerEventSystem = null;
        _controller = null;
        _playerID = -1;
        
        joinText.alpha = 1.0f;
        joinText.interactable = true;
        playerSelect.alpha = 0.0f;
        playerSelect.interactable = false;
        selectArrows.alpha = 0.0f;
        selectArrows.interactable = false;
        descriptionSection.alpha = 0.0f;
    }

    // Handle player ready/unready.
    public void ReadyPlayer(bool isReady = true)
    {
        if (isReady == true)
        {
            // Setup ready UI.
            _playerReady = true;
            readyOverlay.alpha = 1.0f;
            readyOverlay.interactable = true;
            playerSelect.interactable = false;
            selectArrows.interactable = false;

            // Haptic feedback.
            Gamepad g = (Gamepad)_controller.devices[0];
            StartCoroutine(DoHaptics(g, 1.0f, 1.0f, 0.25f));
            
            // Set controller selection to the ready UI.
            _controllerEventSystem.SetSelectedGameObject(readyOverlay.GetComponentInChildren<Button>().gameObject); // TODO: Fix this. Make it a parameter or something.
        }
        else
        {
            // Reset ready UI.
            _playerReady = false;
            readyOverlay.alpha = 0.0f;
            readyOverlay.interactable = false;
            playerSelect.interactable = true;
            selectArrows.interactable = true;
            
            // Set controller selection back to the player select UI.
            _controllerEventSystem.SetSelectedGameObject(playerSelect.gameObject);
        }
    }

    public void SelectCharacter(int index)
    {
        _selectedCharacterIndex = index;

        descriptionSection.gameObject.GetComponent<CharacterAbilityDescription>().ChangeDescription(_characterMenu.characterDescriptions[index].description);
    }

    // Handle controller haptics.
    IEnumerator DoHaptics(Gamepad gamepad, float strengthLow, float strengthHigh, float time)
    {
        gamepad.SetMotorSpeeds(strengthLow, strengthHigh);
        yield return new WaitForSeconds(time);
        gamepad.ResetHaptics();
    }

}
