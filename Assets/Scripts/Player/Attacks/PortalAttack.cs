using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FrogID))]
public class PortalAttack : MonoBehaviour
{
    private CharacterBase _player;

    [Tooltip("range to spawn/lob too")]
    [SerializeField]
    private float _range;

    [Tooltip("the object that will show the player where there lob attack will go")]
    [SerializeField]
    private GameObject _lobAimer;

    private AimChecker _checker;

    [Tooltip("the prefab for the portals (make sure they have the portal component)")]
    [SerializeField]
    private GameObject _portalPrefab;

    private Portal _firstPortal;
    private Portal _secondPortal;

    [HideInInspector]
    public int frogID;

    [Tooltip("the name of the trigger to be set when this attack is triggerd")]
    [SerializeField]
    private string animationTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<CharacterBase>();

        if (_lobAimer)
        {
            if (_range != 0)
            {
                _lobAimer.SetActive(true);
            }
            else
            {
                _lobAimer.SetActive(false);
            }
            _checker = _lobAimer.GetComponent<AimChecker>();
            
        }

        frogID = _player.GetInstanceID();

        gameObject.GetComponent<FrogID>().ID = frogID;
    }

    /// <summary>
    /// changes the lob aimers position
    /// </summary>
    void Update()
    {
        if (_lobAimer && _range != 0)
        {
            _lobAimer.transform.localPosition = Vector3.forward * _range * _player.currentAimMagnitude;
        }
    }

    /// <summary>
    /// will make either the first or second portal depending on the existence of the first portal
    /// </summary>
    /// <param name="context"></param>
    public void SpawnObjectAtAimFunction(InputAction.CallbackContext context)
    {
        if (!_checker._colliding)
        {
            //if there is no active portal made by this component make the first one and set it up
            if(_firstPortal == null)
            {
                GameObject newPortal = Instantiate(_portalPrefab, _lobAimer.transform.position, _player.transform.rotation);
                _firstPortal = newPortal.GetComponent<Portal>();
                _firstPortal.trigger.enabled = false;
                _firstPortal.madeAttack = this;

                _firstPortal.frogID = frogID;
            } 
            //if the first portal does exist make the second set it up and connect the portals
            else if (_secondPortal == null)
            {
                GameObject newPortal = Instantiate(_portalPrefab, _lobAimer.transform.position, _player.transform.rotation);
                _secondPortal = newPortal.GetComponent<Portal>();
                _secondPortal.madeAttack = this;
                _secondPortal.frogID = frogID;

                _firstPortal.trigger.enabled = true;
                _secondPortal.trigger.enabled = true;

                _firstPortal.endPoint = _secondPortal.transform;
                _secondPortal.endPoint = _firstPortal.transform;
            }

            if (_player.animator != null && animationTrigger != null && animationTrigger.Length > 0)
            {
                _player.animator.SetTrigger(animationTrigger);
            }
        }
    }

    /// <summary>
    /// destroys the active portals resetting this attck so it can be used agian
    /// </summary>
    /// <param name="teleportee"></param>
    public void OnPortalUsed(GameObject teleportee)
    {
        Destroy(_firstPortal.gameObject);
        _firstPortal = null;
        Destroy(_secondPortal.gameObject);
        _secondPortal = null;
    }
}
