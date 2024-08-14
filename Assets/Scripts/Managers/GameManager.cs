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

    [Tooltip("The scene to load when the game is over")]
    [SerializeField]
    private string _endLevel;

    [Tooltip("the prefab players use to controll the character screen")]
    [SerializeField]
    private GameObject _cursorPrefab;

    [Tooltip("the prefab that will be used to spawn the players")]
    [SerializeField]
    private GameObject _playerPrefab;

    private Dictionary<CharacterBase, PlayerData> _alivePlayers = new Dictionary<CharacterBase, PlayerData>();
    private int _connectedPlayerCount;

    private int _currentRound = 0;

    [Tooltip("the amount of rounds needed to end the game")]
    [SerializeField]
    private int _scoreToWin;

    [Tooltip("how long before the players take damage over time")]
    [SerializeField]
    private float _roundTime;

    [Tooltip("the multiplyer for damage taken for the round being over")]
    [SerializeField]
    private float endGameDamageMult;

    private float _roundTimer;

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
                    newPlayer.GetComponent<CursorController>().playerID = _connectedPlayerCount;

                    // Create player data, stores gamepad, character and score.
                    PlayerData newPlayerData = new PlayerData();
                    newPlayerData.gamepad = Gamepad.all[i];
                    newPlayerData.characterSelect = null;
                    newPlayerData.score = 0;
                    _playerData.Add(newPlayerData);

                    _connectedPlayerCount++;
                }
            }
        } else
        {
            _roundTimer -= Time.deltaTime;
            if(_roundTimer <= 0)
            {
                foreach(KeyValuePair<CharacterBase, PlayerData> p in _alivePlayers)
                {
                    p.Key.TakeDamage(Time.deltaTime * endGameDamageMult);
                }
            }
        }
    }

    public void SetSelectedCharacter(int listPosition, GameObject selection)
    {
        _playerData[listPosition].characterSelect = selection;
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
                    _alivePlayers.Remove(player);
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
        
        if (_alivePlayers.Count() <= 1)
        {
            _roundTimer = _roundTime;
            // Handle win
            PlayerWin();
        }
    }

    public void PlayerWin()
    {
        foreach (KeyValuePair<CharacterBase, PlayerData> p in _alivePlayers)
        {
            p.Value.score++;
            _currentRound++;

            //if round over
            if (p.Value.score < _scoreToWin)
                StartGame();
            else // if game over
            {
                SceneManager.LoadScene(_endLevel);
                Destroy(gameObject);
            }
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

        _alivePlayers = new Dictionary<CharacterBase, PlayerData>();

        Spawn spawnInScene = FindAnyObjectByType<Spawn>();

        for (int i = 0; i < _playerData.Count; i++)
        {
            //here we would check a player data list at the same position to find this players character
            GameObject newPlayer = PlayerInput.Instantiate(_playerData[i].characterSelect, controlScheme: "Gamepad", pairWithDevice: _playerData[i].gamepad).gameObject;
            int random = UnityEngine.Random.Range(0, spawnInScene.spawns.Count);
            newPlayer.transform.position = spawnInScene.spawns[random].position;
            spawnInScene.spawns.Remove(spawnInScene.spawns[random]);
            newPlayer.name += (" > Player ID (" + i + ")");
            CharacterBase character = newPlayer.GetComponent<CharacterBase>();
            character.playerGamepad = _playerData[i].gamepad;
            _alivePlayers.Add(character, _playerData[i]);
        }

        addingControllers = false;

        _roundTimer = _roundTime;

        Destroy(spawnInScene.gameObject);
    }
}
