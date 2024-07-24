using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStatManager : NetworkBehaviour
{
    [SerializeField] PlayerManager _playerManager;

    [SyncVar(hook = nameof(OnHealthChange))]
    public int currentHealth;
    [SerializeField] private int maxHealth;



    public override void OnStartServer()
    {
        Debug.Log("server worked");
        base.OnStartServer();
        currentHealth = maxHealth;
        //StartCoroutine(ChangeHealthRandomly());
    }
    
    
    private void OnHealthChange(int oldValue, int newValue)
    {
        _playerManager.GetPlayerObjectUIManager().SetHealthText(newValue);
        Debug.Log($"old value : {oldValue} \n new value {newValue} \n current value {currentHealth}");
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            DealDamage(); //on server // server to client sync
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReduceHealth(); //on client // client to server sync
        }
    }

    [Client]
    private void ReduceHealth()
    {
        currentHealth -= 10;
        _playerManager.GetPlayerObjectUIManager().SetHealthText(currentHealth);
    }

    [Command]
    private void DealDamage()
    {
        currentHealth -= 10;
        //DealDamageRpc(currentHealth);
    }

    IEnumerator ChangeHealthRandomly()
    {
        while (true)
        {
            currentHealth = Random.Range(0, 100);
            Debug.Log("coroutine ch : " + currentHealth);
            yield return new WaitForSeconds(2);
        }
    }
}
