using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ClientUIManager : NetworkBehaviour
{
    [SerializeField] private Button[] _colorChangeButtons;
    Dictionary<ColorType, Button> colorButtons = new Dictionary<ColorType, Button>();
    PlayerManager _playerManager;

    private Dictionary<ColorType, Color> colorMap = new Dictionary<ColorType, Color>
    {
        { ColorType.YELLOW, Color.yellow },
        { ColorType.RED, Color.red },
        { ColorType.GREEN, Color.green },
        { ColorType.NAVY_BLUE, new Color(0.0f, 0.0f, 0.5f) }, // Koyu mavi renk
        { ColorType.PURPLE, new Color(0.5f, 0.0f, 0.5f) }, // Mor renk
        { ColorType.BLUE, Color.blue },
        { ColorType.ORANGE, new Color(1.0f, 0.65f, 0.0f) }, // Turuncu renk
        { ColorType.GRAY, Color.gray }
    };

    private void OnEnable()
    {
        PlayerManager.OnClientConnectToServer += SetPlayerManagerOnPlayerStart;
    }

    private void OnDisable()
    {
        PlayerManager.OnClientConnectToServer -= SetPlayerManagerOnPlayerStart;
    }

    private void Awake()
    {
        ColorType[] colorTypes = (ColorType[])ColorType.GetValues(typeof(ColorType));
        Debug.Log($"color types lngt: {colorTypes.Length}----button lngt: {_colorChangeButtons.Length}");
        for (int i = 0; i < _colorChangeButtons.Length; i++)
        {
            if (i < colorTypes.Length)
            {
                colorButtons.Add(colorTypes[i], _colorChangeButtons[i]);
            }
            else
            {
                Debug.LogWarning("button amount is more than color type");
            }
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("started");

        foreach (KeyValuePair<ColorType, Button> colorButton in colorButtons)
        {
            Debug.Log("button color type : " + colorButton.Value.ToString() + colorButton.Key.ToString());
            colorButton.Value.onClick.AddListener(() => _playerManager.GetPlayerColorManager().OnPressedColorButton(colorMap[colorButton.Key]));
        }
    }

    private void SetPlayerManagerOnPlayerStart(PlayerManager playerManager)
    {
        _playerManager = playerManager;
    }
}

public enum ColorType
{
    YELLOW,
    RED,
    GREEN,
    NAVY_BLUE,
    PURPLE,
    BLUE,
    ORANGE,
    GRAY
}
