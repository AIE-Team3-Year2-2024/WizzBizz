using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class CharacterMenu : Menu
{
    [Header("Character Menu Stuff - ")]
    public GameObject controllerPrefab;

    [Tooltip("List of the available player slots that a player can join.")]
    public List<PlayerSlot> playerSlots = new List<PlayerSlot>();
    [Tooltip("List of the available characters that a player can select.")]
    public List<GameObject> characterPrefabs = new List<GameObject>();

    [Tooltip("Reference to the count down UI. (Optional)")]
    public UICountDown countDown = null;

    [HideInInspector] public int _joinedPlayers = 0; // How many players have joined?
    [HideInInspector] public int _readyPlayers = 0; // How many players are ready?
    [SerializeField] private int _minmumPlayerCount;

    private bool _addingControllers = false; // Are we accepted new players?
    private List<int> _addedGamepadIDs = new List<int>(); // List of the controller indexes that have joined.
    
    public void Update()
    {
        if (_addingControllers == false)
            return;
        
        // Check every connected gamepad.
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (_addedGamepadIDs.Contains(i)) // Have we already added it?
                continue;
            
            // On the south button.
            if (Gamepad.all[i].buttonSouth.isPressed && !Gamepad.all[i].buttonSouth.synthetic)
            {   
                _addedGamepadIDs.Add(i);
                JoinPlayer(i);
            }
        }
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

        // Setup callbacks.
        if (_menuManager)
        {
            _menuManager._controllerDisconnectCallback += ControllerDisconnect;
            _menuManager._controllerReconnectCallback += ControllerReconnect;
        }

        if (countDown != null)
            countDown.countdownEnd += OnCountDownEnd;

        _addingControllers = true;
    }

    public void JoinPlayer(int gamepadID = 0)
    {
        //Debug.Log(playerSlots.Count);

        PlayerSlot slot = getNextAvailableSlot(playerSlots);
        if (slot != null)
        {
            Gamepad gamepad = Gamepad.all[gamepadID];
            PlayerInput p = null;
            MultiplayerEventSystem mm = null;
            if (_menuManager._primaryController != null && _menuManager._primaryController.devices.Contains(Gamepad.all[gamepadID])) // Joining player is the primary controller.
            {
                p = _menuManager._primaryController;
                mm = _menuManager._primaryController.GetComponentInChildren<MultiplayerEventSystem>();
            }
            else if (controllerPrefab != null) // Joining player is a new controller.
            {
                // Create a new controller instance.
                p = PlayerInput.Instantiate(controllerPrefab, controlScheme: "Gamepad", pairWithDevice: gamepad);
                p.transform.SetParent(_menuManager.transform);
                p.gameObject.name = "Connected Controller [" + gamepadID + "]";
                p.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                p.onDeviceLost += _menuManager.ControllerDisconnect; // Setup callback events.
                p.onDeviceRegained += _menuManager.ControllerReconnect;
                p.SwitchCurrentActionMap(p.defaultActionMap);
                p.currentActionMap.Enable();
                _menuManager.AddController(p);
                mm = p.GetComponentInChildren<MultiplayerEventSystem>();
            }

            // Got a controller.
            if (mm != null && gamepad != null)
            {
                if (countDown != null)
                    countDown.StopCountDown(); // Shouldn't count down if there is a new player.

                // Join player.
                mm.playerRoot = slot.gameObject;
                slot._playerID = _joinedPlayers;
                _joinedPlayers++;
                
                GameManager.Instance.AddController(gamepad);
                slot.JoinPlayer(p, mm);
            }
            else
            {
                Debug.LogError("Could not find multiplayer event system or an available gamepad for player slot: " + slot.gameObject.name);
                _addedGamepadIDs.Remove(gamepadID); // Shouldn't be added if it failed.
            }
        }
        else
        {
            Debug.LogError("Not slot available for joining gamepad: " + gamepadID);
        }
    }

    public void ReadyPlayer(PlayerSlot slot) // Called by UI Button.
    {
        if (slot == null)
        {
            Debug.LogError("Someone forgot to pass the slot in: " + slot.gameObject.name);
            return;
        }
        if (playerSlots.Contains(slot) == false)
        {
            Debug.LogError("Slot " + slot.gameObject.name + " is not in the PlayerSlots list!");
            return;
        }
        
        if (slot._playerJoined == true && slot._playerReady == false) // Slot is occupied but not already ready.
        {
            // Ready up.
            slot.ReadyPlayer();
            _readyPlayers++;

            if (characterPrefabs != null && characterPrefabs.Count > 0 && 
                slot._selectedCharacterIndex > -1 && slot._selectedCharacterIndex < characterPrefabs.Count)
            {
                // Set the correct selected character prefab and pass it into the player data.
                GameObject selectedCharacter = characterPrefabs[slot._selectedCharacterIndex];
                Debug.Log(selectedCharacter.name + ", " + slot._selectedCharacterIndex);
                GameManager.Instance.SetSelectedCharacter(slot._playerID, selectedCharacter);
            }
            else
            {
                Debug.LogError("Character Prefabs list has not been setup or the selected character is incorrect.");
                return;
            }

            if (_readyPlayers >= _joinedPlayers && _joinedPlayers >= _minmumPlayerCount) // Has everyone readied up?
            {
                // Try starting the game.
                if (countDown != null)
                    countDown.StartCountDown();
            }
        }
    }

    public void UnreadyPlayer(PlayerSlot slot) // Called by UI Button.
    {
        if (slot == null)
        {
            Debug.LogError("Someone forgot to pass the slot in: " + slot.gameObject.name);
            return;
        }
        if (playerSlots.Contains(slot) == false)
        {
            Debug.LogError("Slot " + slot.gameObject.name + " is not in the PlayerSlots list!");
            return;
        }

        if (slot._playerJoined == true && slot._playerReady == true) // Slot is occupied and the player is ready.
        {
            // Unready.
            slot.ReadyPlayer(false);
            _readyPlayers--;

            // Make sure to stop the count down.
            if (countDown != null)
                countDown.StopCountDown();
        }
    }

    public void ControllerDisconnect(PlayerInput controller) // Handle controller disconnect.
    {
        // Find the slot associated with this controller.
        PlayerSlot associatedSlot = null;
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i]._controllerEventSystem == controller.GetComponentInChildren<MultiplayerEventSystem>())
            {
                associatedSlot = playerSlots[i];
                break; // We found it.
            }
        }

        if (associatedSlot != null) // Controller has an associated slot.
        {
            // Remove it.
            GameManager.Instance.RemoveController(controller);
            UnreadyPlayer(associatedSlot);
            associatedSlot.LeavePlayer();
            _joinedPlayers--;
        }
    }
    
    public void ControllerReconnect(PlayerInput controller) // Handle controller regained.
    {
        // Find a slot to add the player back.
        PlayerSlot slot = getNextAvailableSlot(playerSlots);
        if (slot != null)
        {
            Gamepad gamepad = Gamepad.all.Last(); // Really wish the event gave the device directly.
            MultiplayerEventSystem mm = controller.GetComponentInChildren<MultiplayerEventSystem>();

            if (mm != null && gamepad != null)
            {
                // Add the player back.
                slot._playerID = _joinedPlayers;
                _joinedPlayers++;

                GameManager.Instance.AddController(gamepad);
                slot.JoinPlayer(controller, mm);
            }
            else
            {
                Debug.LogError("Either gamepad or eventsystem returned null!");
            }
        }
        else
        {
            Debug.LogError("Slot returned null!");
        }
    }

    PlayerSlot getNextAvailableSlot(List<PlayerSlot> slots)
    {
        if (slots == null || slots.Count <= 0) // No slots at all.
            return null;

        PlayerSlot available = null;
        for (int i = 0; i < slots.Count; i++)
        {
            //Debug.Log(slots[i].name + ", " + slots[i]._playerJoined);
            if (slots[i] == null || slots[i]._playerJoined == true) // Slot is occupied or invalid.
                continue;
            if (slots[i] != null && slots[i]._playerJoined == false) // Slot is both not invalid and not occupied.
            {
                available = slots[i];
                break; // We found the next available slot.
            }
        }

        return available;
    }

    public void OnCountDownEnd() // Handle count down finish.
    {
        // Just start the game.
        _addingControllers = false;
        GameManager.Instance.StartGame();
    }
}
