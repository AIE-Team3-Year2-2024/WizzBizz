using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterSelectHandler : MonoBehaviour, ISelectHandler
{
    public UnityEvent<bool> selectCallback;
    public bool forwards = true;

    public void OnSelect(BaseEventData eventData)
    {
        selectCallback.Invoke(forwards);
    }
}
