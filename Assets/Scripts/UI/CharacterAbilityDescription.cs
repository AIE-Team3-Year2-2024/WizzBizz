using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterAbilityDescription : MonoBehaviour
{
    [Tooltip("the text feild that shows the characters abilities"), SerializeField]
    private TMP_Text _descrptionText;

    public void ChangeDescription(string inputDescription)
    {
        _descrptionText.text = inputDescription;
    }
}
