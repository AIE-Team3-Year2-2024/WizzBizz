using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterSelectHandler : MonoBehaviour, ISelectHandler
{
    [Tooltip("On selection callback.")]
    public UnityEvent<bool> selectCallback;
    [Tooltip("Selecting forwards or backwards.")]
    public bool forwards = true;

    // Stupid workaround to make the UnityUI element do something on selection.
    public void OnSelect(BaseEventData eventData)
    {
        selectCallback.Invoke(forwards);
    }
}
