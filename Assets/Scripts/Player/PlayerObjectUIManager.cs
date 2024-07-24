using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerObjectUIManager : MonoBehaviour
{
    [SerializeField] TextMeshPro _healthText;

    public void SetHealthText(int value)
    {
        _healthText.text = value.ToString();
    }
}
