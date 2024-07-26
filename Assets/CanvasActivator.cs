using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasActivator : MonoBehaviour
{
    [SerializeField] GameObject _canvas;

    private void Start()
    {
        _canvas.SetActive(true);
    }
}
