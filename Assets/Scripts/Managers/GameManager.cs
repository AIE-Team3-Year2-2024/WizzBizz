using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

    [HideInInspector]
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

    [Tooltip("A reference to the Arena UI canvas.")]
    [SerializeField]
    private Canvas arenaUICanvas;

    [Tooltip("A reference to the UI text object for the round timer.")]
    [SerializeField]
    private TMP_Text roundTimerText;

    private float _roundTimer;

    [Tooltip("the wieght this players target in the target group will be set to")]
    [SerializeField]
    private float _playerCameraWeight;

    [Tooltip("the radius this players target in the target group will be set to")]
    [SerializeField]
    private float _playerCameraRadius;

    [Header("Slowdown effect")]

    [Tooltip("the radius this players target in the target group will be set to in slowdown")]
    [SerializeField]
    private float _slowPlayerCameraRadius;

    [Tooltip("the wieght this players target in the target group will be set to in slowdown")]
    [SerializeField]
    private float _slowPlayerCameraWeight;

    [Tooltip("the speed of time during slowdown")]
    [SerializeField]
    [Range(0, 1)]
    private float _slowTimeScale;

    [Tooltip("how long slowdown will last for in seconds")]
    [SerializeField]
    private float _slowdownLength;

    private float _currentTimeScale = 1;

    private CharacterBase _pausingPlayer;

    private CinemachineTargetGroup currentTargetGroup;

    [HideInInspector]
    public List<OrbSpawner> orbSpawners = new List<OrbSpawner>();

    public float orbSpawnerCooldown = 3.0f;
    private float _orbSpawnerTimer = 0.0f;
    private bool _orbCollected = true;

    public GameObject canvas;

    private List<PlayerData> _playerData = new List<PlayerData>();

    // Singleton instantiation
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// before the game starts this is in charge of adding controllers and during the game its in charge of the round timer and spawning orbs
    /// </summary>
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

            if (roundTimerText != null)
            {
                if (_roundTimer >= 0.0f)
                {
                    TimeSpan formattedTime = TimeSpan.FromSeconds(_roundTimer);
                    roundTimerText.text = formattedTime.ToString("mm':'ss");
                }
            }

            if (orbSpawners.Count > 0)
            {
                if (_orbSpawnerTimer <= 0.0f && _orbCollected == true)
                {
                    Debug.Log("Orb should spawn!");

                    Dictionary<float, OrbSpawner> spawnersByDistance = new Dictionary<float, OrbSpawner>();
                    foreach (OrbSpawner o in orbSpawners)
                    {
                        List<float> playerDistances = new List<float>();
                        foreach (CharacterBase c in _alivePlayers.Keys)
                        {
                            playerDistances.Add(Vector3.Distance(o.transform.position, c.transform.position));
                        }
                        spawnersByDistance.Add(playerDistances.Average(), o);
                    }
                    float furthestSpawnerDistance = spawnersByDistance.Keys.Max();
                    Debug.Log("Furthest distance: " + furthestSpawnerDistance);

                    spawnersByDistance[furthestSpawnerDistance].Reset();
                    _orbCollected = false;
                    _orbSpawnerTimer = orbSpawnerCooldown;
                }
                else
                {
                    _orbSpawnerTimer -= Time.deltaTime;
                }
            }
        }
    }

    /// <summary>
    /// is used by the cursour when they pick a character
    /// </summary>
    /// <param name="listPosition"></param>
    /// <param name="selection"></param>
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

    /// <summary>
    /// removes the player from alive players and player data
    /// </summary>
    /// <param name="player"></param>
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

    /// <summary>
    /// pauses time and deactivates input on all players other than the pauser
    /// </summary>
    /// <param name="pauser"></param>
    public void Pause(CharacterBase pauser)
    {
        Time.timeScale = 0;
        _pausingPlayer = pauser;
        foreach (CharacterBase c in _alivePlayers.Keys)
        {
            if(c != pauser)
            {
                c.GetComponent<PlayerInput>().DeactivateInput();
            }
        }
    }

    /// <summary>
    /// puts time back to normal and re activates all players
    /// </summary>
    /// <param name="pauser"></param>
    public void UnPause(CharacterBase pauser)
    {
        Time.timeScale = _currentTimeScale;
        _pausingPlayer = null;
        foreach (CharacterBase c in _alivePlayers.Keys)
        {
            if (c != pauser)
            {
                c.GetComponent<PlayerInput>().ActivateInput();
            }
        }
    }

    /// <summary>
    /// used to unpause the game from the game manager e.g. with a button
    /// </summary>
    public void UnPause()
    {
        _pausingPlayer.UnPause();
    }

    /// <summary>
    /// removes and destroys dead player and does a win check
    /// </summary>
    /// <param name="player"></param>
    public void PlayerDeath(CharacterBase player)
    {
        if (_alivePlayers.Count() > 1)
        {
            CharacterBase[] deadPlayer = new CharacterBase[]{ player };
            StartSlowDown(deadPlayer);
            _alivePlayers.Remove(player);
            Destroy(player.gameObject);
        }
        
        if (_alivePlayers.Count() <= 1)
        {
            _roundTimer = _roundTime;
            // Handle win
            PlayerWin();
        }
    }

    /// <summary>
    /// adds score to the first alive player and either loads the next round or the end level
    /// </summary>
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
                arenaUICanvas.gameObject.SetActive(false);
                SceneManager.LoadScene(_endLevel);
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// resets orb timer
    /// </summary>
    public void OrbSpawnerCollected()
    {
        _orbSpawnerTimer = orbSpawnerCooldown; // Reset timer.
        _orbCollected = true;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    public void StartSlowDown(CharacterBase[] players)
    {
        StartCoroutine(SlowDownEffect(players));
    }

    /// <summary>
    /// slows down time and then changes the transform group whan waits and reverts the time and transform group back to normal
    /// </summary>
    /// <param name="players"></param>
    /// <returns></returns>
    public IEnumerator SlowDownEffect(CharacterBase[] players)
    {
        CinemachineTargetGroup.Target[] oldTargets = currentTargetGroup.m_Targets;
        CinemachineTargetGroup.Target[] slowTargs = new CinemachineTargetGroup.Target[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            slowTargs[i].target = players[i].transform;
            slowTargs[i].radius = _slowPlayerCameraRadius;
            slowTargs[i].weight = _slowPlayerCameraWeight;
        }

        currentTargetGroup.m_Targets = slowTargs;

        Time.timeScale = _slowTimeScale;

        _currentTimeScale = _slowTimeScale;

        yield return new WaitForSeconds(_slowdownLength * _slowTimeScale);

        currentTargetGroup.m_Targets = oldTargets;

        CinemachineTargetGroup.Target[] aliveTargets = new CinemachineTargetGroup.Target[_alivePlayers.Count];

        int pos = 0;

        foreach (CharacterBase c in _alivePlayers.Keys)
        {
            aliveTargets[pos].target = c.transform;
            aliveTargets[pos].weight = _playerCameraWeight;
            aliveTargets[pos].radius = _playerCameraRadius;
            pos++;
        }

        currentTargetGroup.m_Targets = aliveTargets;

        Time.timeScale = 1;

        _currentTimeScale = 1;
    }

    /// <summary>
    /// sets the game manager back to normal then loads a scene waits for the scene to load and then sets up the scene
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartGameRoutine()
    {
        Time.timeScale = 1;
        orbSpawners.Clear(); // Should be cleared everytime a new arena is loaded.
        _orbSpawnerTimer = orbSpawnerCooldown;
        _orbCollected = true;

        SceneManager.LoadScene(_levels[UnityEngine.Random.Range(0, _levels.Length)]);

        //theese are here so that the players get spawned in the new scene and not the old one
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _alivePlayers = new Dictionary<CharacterBase, PlayerData>();

        Spawn spawnInScene = FindAnyObjectByType<Spawn>();

        currentTargetGroup = FindAnyObjectByType<CinemachineTargetGroup>();

        //targetGroup.m_PositionMode = 0;
        //targetGroup.m_RotationMode = 0;
        currentTargetGroup.m_UpdateMethod = CinemachineTargetGroup.UpdateMethod.LateUpdate;

        CinemachineTargetGroup.Target[] targs = new CinemachineTargetGroup.Target[_playerData.Count];

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
            character.playerNumber.text = "P" + (i + 1);
            _alivePlayers.Add(character, _playerData[i]);


            targs[i].target = newPlayer.transform;
            targs[i].radius = _playerCameraRadius;
            targs[i].weight = _playerCameraWeight;

        }

        currentTargetGroup.m_Targets = targs;

        addingControllers = false;

        _roundTimer = _roundTime;

        if (arenaUICanvas != null)
            arenaUICanvas.gameObject.SetActive(true);

        Destroy(spawnInScene.gameObject);
    }
}
