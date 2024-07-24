using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Mirror;

public class PlayerColorManager : NetworkBehaviour
{
    public SpriteRenderer _spriteRenderer;

    [SyncVar(hook = nameof(OnColorChange))]
    Color currentColor;

    [Command]
    public void OnPressedColorButton(Color color)
    {
        if (_spriteRenderer == null)
        {
            Debug.LogWarning("sprite renderer null");
            return;
        }
        //ChangePlayerColor(color);
        currentColor = color;
        Debug.Log("color from server : " + color.ToString());
    }

/*     [ClientRpc]
    private void ChangePlayerColor(Color color)
    {
        //_spriteRenderer.color = color;
        Debug.Log("color from Clients : " + color.ToString());
    } */

    private void OnColorChange(Color oldColor, Color newColor)
    {
        _spriteRenderer.color = newColor;
    }
}
