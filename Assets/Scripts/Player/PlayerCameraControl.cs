using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _targetObj;
    [SerializeField] private float _zCameraOffset = 10f;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        PlayerManager.OnClientConnectToServer += SetTargetObj;
    }

    private void OnDisable()
    {
        PlayerManager.OnClientConnectToServer -= SetTargetObj;
    }

    private void LateUpdate()
    {
        if(_targetObj == null)
            return;

        Vector3 targetPosition = new Vector3(_targetObj.transform.position.x, _targetObj.transform.position.y, _targetObj.transform.position.z - _zCameraOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1 * Time.deltaTime);
    }

    private void SetTargetObj(PlayerManager playerManager)
    {
        _targetObj = playerManager.gameObject;
    }

}
