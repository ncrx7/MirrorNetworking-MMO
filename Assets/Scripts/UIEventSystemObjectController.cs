using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventSystemObjectController : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    } 
}
