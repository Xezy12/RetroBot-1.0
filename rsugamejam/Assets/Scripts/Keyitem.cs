using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyitem : MonoBehaviour
{
    private PlayerInventory playerinvent;
    void Start()
    {
        playerinvent = FindObjectOfType<PlayerInventory>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            playerinvent.AddItem("Keyitem");
            Destroy(gameObject);
        }        
    }
}
