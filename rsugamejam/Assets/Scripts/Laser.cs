using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private PlayerController player; 
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Laser") || other.CompareTag("Laser2")){
            player.movementQueue.Clear();
        }
    }
}
