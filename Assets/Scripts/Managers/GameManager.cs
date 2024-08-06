using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public Gamepad gamepad;
    public GameObject characterSelect;
    public int score;
}

public class GameManager : MonoBehaviour
{
    // Static reference
    public static GameManager Instance { get; private set; }

    public bool addingControllers;

    [Tooltip("The list of level scenes to be picked randomly (CAPITALIZATION MATTERS)")]
    [SerializeField]
    private string[] _levels;

    [Tooltip("the prefab players use to controll the character screen")]
    [SerializeField]
    private GameObject _cursorPrefab;

    [Tooltip("the prefab that will be used to spawn the players")]
    [SerializeField]
    private GameObject _playerPrefab;

    private Dictionary<CharacterBase, PlayerData> _alivePlayers = new Dictionary<CharacterBase, PlayerData>();
    private int _connectedPlayerCount;

    private int _currentRound = 0;

    public GameObject canvas;

    private List<PlayerData> _playerData = new List<PlayerData>();

    // Singleton instantiation
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(addingControllers)
        {
            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                bool alreadyContainsGamepad = false;
                for (int j = 0; j < _playerData.Count(); j++)
                {
                    if (_playerData[j].gamepad == Gamepad.all[i])
                        alreadyContainsGamepad = true;
                }

                if (alreadyContainsGamepad == true)
                    continue;

                //check if the current gamepad has a button pressed and is not stored
                if (Gamepad.all[i].allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic))
                {                    
                    GameObject newPlayer = PlayerInput.Instantiate(_cursorPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[i]).gameObject;
                    newPlayer.transform.SetParent(canvas.transform);

                    // Create player data, stores gamepad, character and score.
                    PlayerData newPlayerData = new PlayerData();
                    newPlayerData.gamepad = Gamepad.all[i];
                    newPlayerData.characterSelect = null;
                    newPlayerData.score = 0;
                    _playerData.Add(newPlayerData);

                    _connectedPlayerCount++;
                }
            }
        }
    }

    /// <summary>
    /// removes the players controller from the controller list and updates the player count
    /// </summary>
    /// <param name="player"></param>
    public void DissconectCursor(PlayerInput player)
    {
        foreach(InputDevice input in player.devices)
        {
            foreach(PlayerData p in _playerData)
            {
                if(input == p.gamepad)
                {
                    _playerData.Remove(p);
                    _connectedPlayerCount--;
                    return;
                }
            }
        }
    }

    public void DisconnectPlayer(CharacterBase player)
    {
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        foreach (InputDevice input in playerInput.devices)
        {
            foreach (PlayerData p in _playerData)
            {
                if (input == p.gamepad)
                {
                    _playerData.Remove(p);
                    _connectedPlayerCount--;
                    return;
                }
            }
        }
    }

    public int GetConnectedPlayerCount()
    {
        return _connectedPlayerCount;
    }

    public void PlayerDeath(CharacterBase player)
    {
        if (_alivePlayers.Count() > 1)
        {
            _alivePlayers.Remove(player);
        }
        else
        {
            // Handle win/lose
            _alivePlayers[player].score++;
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    public IEnumerator StartGameRoutine()
    {
        SceneManager.LoadScene(_levels[UnityEngine.Random.Range(0, _levels.Length)]);

        //theese are here so that the players get spawned in the new scene and not the old one
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < _playerData.Count; i++)
        {
            //here we would check a player data list at the same position to find this players character
            GameObject newPlayer = PlayerInput.Instantiate(_playerPrefab, controlScheme: "Gamepad", pairWithDevice: _playerData[i].gamepad).gameObject;
            newPlayer.name += (" > Player ID (" + i + ")");
            CharacterBase character = newPlayer.GetComponent<CharacterBase>();
            character.playerGamepad = _playerData[i].gamepad;
            _alivePlayers.Add(character, _playerData[i]);
        }

        addingControllers = false;
    }
}
